using System.Net;
using System.Net.Sockets;

namespace SimpleServer
{
    /// <summary>
    /// Client to connect into server
    /// </summary>
    public abstract partial class Client
    {
        /// <summary>
        /// The id provided by the server to this client
        /// </summary>
        protected long Id { get; private set; } = 0;

        /// <summary>
        /// If the client is current connected into the server
        /// </summary>
        protected bool IsConnected
        {
            get
            {
                lock(_connectLock)
                    return Id > 0 && _client != null && _client.Connected;
            }
        }

        private object _connectLock = new object();

        /// <summary>
        /// Current port connected
        /// </summary>
        protected int Port { get; private set; } = 0;

        /// <summary>
        /// Current host connected
        /// </summary>
        protected string Host { get; private set; } = string.Empty;

        /// <summary>
        /// Buffer size to recieve messages in bytes
        /// </summary>
        protected int BufferSize { get; private set; } = 64_000;

        private TcpClient _client;
        private Thread _thread;
        private PacketReciever _reciever;

        public Client(int bufferSize = 64_000)
        {
            _client = new TcpClient();

            BufferSize = bufferSize;
        }

        /// <summary>
        /// Connect into the server else throw error
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void Connect(string host, int port)
        {
            Host = host;
            Port = port;
            
            _reciever = new PacketReciever();

            IPAddress address = IPAddress.Parse(host);
            var endpoint = new IPEndPoint(address, port);

#if DEBUG
            Logger.Log($"[Client]: Connecting in {Host}:{Port}");
#endif

            _client.Connect(address, port);

            Id = GetIdFromServer();

            try
            {
                _thread = new Thread(() => ClientThread());
                _thread.Start();
            }
            catch(Exception ex)
            {
#if DEBUG
                Logger.Log($"[Client]: Error in start client thread: {ex.Message}");
#endif

                _thread = null;
                _client.Close();

                throw;
            }
        }

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        /// <param name="waitThread"> Join the client thread if true </param>
        public void Disconnect(bool waitThread = true)
        {
#if DEBUG
            Logger.Log($"[Client]: Disconnecting from {Host}:{Port}");
#endif

            _client.Close();

            if(waitThread)
                _thread.Join();
#if DEBUG
            Logger.Log($"[Client]: Disconnected from {Host}:{Port}");
#endif
        }

        private long GetIdFromServer()
        {
            byte[] buffer = new byte[BufferSize];

#if DEBUG
            Logger.Log("[Client]: Waiting for server to authenticate");
#endif
             
            while (!_reciever.IsReady)
            {
                try
                {
                    _reciever.Recieve(ReadData(_client, buffer));
                }
                catch (Exception e)
                {
                    _client.Close();
                    throw new Exception($"Error when reading client id from server", e);
                }
            }

            if(!long.TryParse(_reciever.Content, out long id))
            {
                _client.Close();
#if DEBUG
                Logger.Log($"[Client]: Server return an invalid client id: {_reciever.Content}");
#endif
                throw new Exception($"Server return an invalid client id: {_reciever.Content}");
            }

#if DEBUG
            Logger.Log($"[Client]: Server return client id: {id}");
#endif
            beginLeftOver = _reciever.LeftOver;
            _reciever.ResetState();

            return id;
        }

        private void ClientThread()
        {
            byte[] buffer = new byte[BufferSize];

            try
            {
                OnConnect();
            }
            catch(Exception e)
            {
#if DEBUG
                Logger.Log($"[Client]: OnConnect throw an exception: {e.Message}");
#endif
            }

            while (true)
            {
                try
                {
                    _reciever.Recieve(ReadData(_client, buffer));

                    while (_reciever.IsReady)
                    {
#if DEBUG
                        Logger.Log($"[Client]: Recieved data {_reciever.Content} from server {Host}:{Port}");
#endif
                        try
                        {
                            OnRecieveMessage(_reciever.Content);
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            Logger.Log($"[Client]: OnRecieveMessage throw an exception: " + ex.Message);
#endif
                        }

                        if (_reciever.HasLeftOver)
                            _reciever.Recieve(_reciever.LeftOver);
                        else
                            _reciever.ResetState();
                    }
                }
                catch (Exception e)
                {
#if DEBUG
                    Logger.Log($"[Client]: Server throw an exception: {e.Message}: Connection closed!");
                    _client.Close();
#endif
                    try
                    {
                        OnDisconnect();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Logger.Log($"[Server]: OnDisconnect throw an exception: {ex.Message}");
#endif
                    }

                    return;
                }
            }
        }
    }
}