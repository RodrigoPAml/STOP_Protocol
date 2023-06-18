using System.Net.Sockets;
using System.Text;

namespace STOP
{
    /// <summary>
    /// Handle basic sockets operations
    /// </summary>
    public abstract partial class Server
    {
        /// <summary>
        /// Accepts a client, if success return it, else null
        /// </summary>
        /// <returns></returns>
        private TcpClient? AcceptClient()
        {
            try
            {
#if DEBUG
                Logger.Log($"[Server]: Waiting for new client");
#endif
                TcpClient client = _server.AcceptTcpClient();

                if (client != null)
                {
                    var endpoint = client?.Client?.RemoteEndPoint;
#if DEBUG
                    Logger.Log($"[Server]: Connection accepted from {endpoint?.ToString()}");
#endif
                    return client;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Logger.Log($"[Server]: Failed to try to accept client: {e.Message}");
#endif
            }

            return null;
        }

        /// <summary>
        /// Read data from client and return it
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private string ReadData(TcpClient client, byte[] buffer)
        {
            if(client == null)
                throw new Exception("Reading data from null client");

            NetworkStream stream = client.GetStream();
            var size = stream.Read(buffer, 0, buffer.Length);

            if (size == 0)
                throw new Exception("Recieved empty message");

            return Encoding.ASCII.GetString(buffer, 0, size);
        }

        /// <summary>
        /// Send data to some client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="content"></param>
        private void SendData(TcpClient client, string content)
        {
            if(client == null)
                throw new Exception("Sending data to null client");

            string data = $"BEG{(content.Length + 16).ToString("D10")}" + content + "END";

            NetworkStream stream = client.GetStream();

            byte[] buffer = Encoding.ASCII.GetBytes(data);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}