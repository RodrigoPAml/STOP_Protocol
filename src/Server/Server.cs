using System.Net;
using System.Net.Sockets;

namespace STOP
{
    /// <summary>
    /// Implementation of a customizable server 
    /// </summary>
    public abstract partial class Server
    {
        /// <summary>
        /// Port used by the server
        /// </summary>
        protected readonly int Port;

        /// <summary>
        /// Buffer size used by the server, by each client
        /// </summary>
        protected readonly int BufferSize = 64_000;

        /// <summary>
        /// If callbacks are syncronized
        /// </summary>
        protected readonly bool SyncronizedCallbacks = true;

        /// <summary>
        /// TCP Listener from C# Socket
        /// </summary>
        private readonly TcpListener _server;

        /// <summary>
        /// Variable to control whether  the server executes or not
        /// This variable is thread safe (Access by Running)
        /// </summary>
        private bool _run = true;
        private object _runLock = new object();

        /// <summary>
        /// If the server is running into some port
        /// </summary>
        protected bool Running
        {
            get
            {
                bool value = false;
                
                lock(_runLock)
                    value = _run && _server != null && _server.Server.IsBound;

                return value;
            }
        }

        /// <summary>
        /// Each client mapped with it given id
        /// This variable is thread safe (Access by Clients)
        /// </summary>
        private Dictionary<long, TcpClient> _clients;
        private object _clientsLock = new object();

        /// <summary>
        /// Provide the current connected users with the server.
        /// Keep in min that a client in this array might not already generated an OnClientConnect event
        /// </summary>
        protected List<long> ConnectedClients
        {
            get
            {
                List<long> clients = new List<long>();

                lock (_clientsLock)
                    clients.AddRange(_clients.Keys.ToList());

                return clients;
            }
        }

        /// <summary>
        /// Each client next id given is control by this variable
        /// This variable is thread safe (Access by GetNewId)
        /// </summary>
        private long _currentId = 0;
        private object _currentIdLock = new object();

        private long GetNewId 
        { 
            get 
            {
                lock(_currentIdLock)
                    _currentId++;

                return _currentId; 
            } 
        }

        /// <summary>
        /// Initiliaze server
        /// </summary>
        /// <param name="port"> Port to listen </param>
        /// <param name="bufferSize"> Buffer size for each client in bytes </param>
        /// <param name="syncronizeCallbacks"> If callback will be auto sincronized via lock </param>
        public Server(int port, int bufferSize = 64_000, bool syncronizeCallbacks = true)
        {
            _server = new TcpListener(new IPEndPoint(IPAddress.Any, port));

            SyncronizedCallbacks = syncronizeCallbacks;
            BufferSize = bufferSize;
            Port = port;
        }

        /// <summary>
        /// Thread that handles the client (recieves message from client)
        /// </summary>
        private void ClientThread(TcpClient client)
        {
            // When start gets a new id for this client
            long clientId = GetNewId;
            var endpoint = client?.Client?.RemoteEndPoint;
            
            // Send the id to the client
            try
            {
                SendData(client, clientId.ToString());
            }
            catch (Exception ex)
            {
#if DEBUG
                Logger.Log($"[Server]: Client from {endpoint?.ToString()} failed to handshake: " + ex.Message);
#endif
                client?.Close();
                return;
            }

            // Add to client list
            lock (_clientsLock)
                _clients.Add(clientId, client);

#if DEBUG
            Logger.Log($"[Server]: Client from {endpoint?.ToString()} assigned with id {clientId}");
#endif

            try
            {
                if(SyncronizedCallbacks)
                    lock (Lock)
                        OnClientConnected(clientId);
                else 
                    OnClientConnected(clientId);
            }
            catch (Exception ex)
            {
#if DEBUG
                Logger.Log($"[Server]: OnClientConnected({clientId}) throw an exception: " + ex.Message);
#endif
            }

            // Instantiate packet reciever and buffer
            PacketReciever reciever = new PacketReciever();
            byte[] buffer = new byte[BufferSize];

            // Recieve message and call callbacks
            while (Running)
            {
                try  
                {
                    reciever.Recieve(ReadData(client, buffer));

                    while (reciever.IsReady)
                    {
#if DEBUG
                        Logger.Log($"[Server]: Recieved data {reciever.Content} from {endpoint?.ToString()} with id {clientId}");
#endif
                        try
                        {
                            if (SyncronizedCallbacks)
                                lock (Lock)
                                    OnRecieveMessage(clientId, reciever.Content);
                            else 
                                OnRecieveMessage(clientId, reciever.Content);
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            Logger.Log($"[Server]: OnRecieveMessage({clientId}) throw an exception: " + ex.Message);
#endif
                        }

                        if (reciever.HasLeftOver)
                            reciever.Recieve(reciever.LeftOver);
                        else
                            reciever.ResetState();
                    }
                }
                catch (Exception e)
                {
#if DEBUG
                    Logger.Log($"[Server]: Client from {endpoint?.ToString()} with id {clientId} throw and exception: {e.Message}: Connection closed!");
#endif
                    lock (_clientsLock)
                        _clients.Remove(clientId);

                    client.Close();

                    try
                    {
                        if (SyncronizedCallbacks)
                            lock (Lock)
                                OnClientDisconnected(clientId, e);
                        else
                            OnClientDisconnected(clientId, e);
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Logger.Log($"[Server]: OnClientDisconnected({clientId}) throw an exception: " + ex.Message);
#endif
                    }

                    return;
                }
            }

            lock (_clientsLock)
                _clients.Remove(clientId);

            client.Close();
#if DEBUG
            Logger.Log($"[Server]: Client from {endpoint?.ToString()} with id {clientId} disconnected");
#endif

            try
            {
                if (SyncronizedCallbacks)
                    lock (Lock)
                        OnClientDisconnected(clientId, null);
                else
                    OnClientDisconnected(clientId, null);
            }
            catch (Exception ex)
            {
#if DEBUG
                Logger.Log($"[Server]: OnClientDisconnected({clientId}) throw an exception: " + ex.Message);
#endif
            }
        }

        /// <summary>
        /// Blocking thread to accept clients
        /// This function blocks the thread until server stops
        /// </summary>
        public void Run()
        {
            _clients = new Dictionary<long, TcpClient>();

#if DEBUG
            Logger.Log($"[Server]: Starting in {Port}");
#endif
            _server.Start();
#if DEBUG
            Logger.Log($"[Server]: Started");
#endif
            if (Running == false)
            {
#if DEBUG
                Logger.Log($"[Server]: Server is not running, please check the firewall");
#endif
                throw new Exception("Server is not running, please check the firewall");
            }

            try
            {
                if (SyncronizedCallbacks)
                    lock (Lock)
                        OnServerStart();
                else 
                    OnServerStart();
            }
            catch(Exception e) 
            {
#if DEBUG
                Logger.Log("[Server]: OnServerStart throw an exception: " + e.ToString());
#endif
            }

            List<Thread> _threads = new List<Thread>();
            while (Running)
            {
                var client = AcceptClient();

                var thread = new Thread(() => ClientThread(client));
                thread.Start();

                if (client != null )
                    _threads.Add(thread);
            }

            foreach (var thread in _threads)
                thread.Join();

            try
            {
                _server.Stop();
            }
            catch (Exception e)
            {
#if DEBUG
                Logger.Log("[Server]: Error when stoping server: " + e.ToString());
#endif
            }

            try
            {
                if (SyncronizedCallbacks)
                    lock (Lock)
                        OnServerStop();
                else
                    OnServerStop();
            }
            catch (Exception e)
            {
#if DEBUG
                Logger.Log("[Server]: OnServerStop throw an exception: " + e.ToString());
#endif
            }
        }
    }
}