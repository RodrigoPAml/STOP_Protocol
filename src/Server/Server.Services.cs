using System.Net.Sockets;

namespace STOP
{
    /// <summary>
    /// Functions provided by the server
    /// </summary>
    public abstract partial class Server
    {
        /// <summary>
        /// Send data to some client
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="message"></param>
        /// <exception cref="Exception"></exception>
        protected void Send(long clientId, string message)
        {
            TcpClient tcpClient = null;

            lock(_clientsLock)
            {
                if(_clients.ContainsKey(clientId)) 
                    tcpClient = _clients[clientId];
            }

            if (tcpClient != null)
                SendData(tcpClient, message);
            else
                throw new Exception($"Can't find client with id {clientId}");
        }

        /// <summary>
        /// Close connection with client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <exception cref="Exception"></exception>
        protected void Close(long clientId)
        {
            TcpClient tcpClient = null;

            lock (_clientsLock)
            {
                if (_clients.ContainsKey(clientId))
                    tcpClient = _clients[clientId];
            }

            if (tcpClient != null)
               tcpClient.Close();
            else
                throw new Exception($"Can't find client with id {clientId}");
        }

        /// <summary>
        /// Stop the server
        /// </summary>
        protected void Stop()
        {
            lock (_runLock)
                _run = false;

            try
            {
                _server.Stop();
            }
            catch(Exception e) 
            {
#if DEBUG
                Logger.Log($"Exception when stoping server: {e.Message}");
#endif
            }
        }
    }
}