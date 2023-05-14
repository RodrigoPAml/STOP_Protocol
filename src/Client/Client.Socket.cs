using System.Net.Sockets;
using System.Text;

namespace SimpleServer
{
    public abstract partial class Client
    {
        private string beginLeftOver = string.Empty;

        private string ReadData(TcpClient client, byte[] buffer)
        {
            if(beginLeftOver.Length > 0)
            {
                string tempData = beginLeftOver;
                beginLeftOver = string.Empty;
                return tempData;
            }

            NetworkStream stream = client.GetStream();
            var size = stream.Read(buffer, 0, buffer.Length);

            if (size == 0)
                throw new Exception("Read empty message from server");

            return Encoding.ASCII.GetString(buffer, 0, size);
        }

        protected void Send(string content)
        {
            if(content== null) 
                throw new ArgumentNullException("content is null");  

            string data = $"BEG{(content.Length + 16).ToString("D10")}" + content + "END";

            NetworkStream stream = _client.GetStream();

            byte[] buffer = Encoding.ASCII.GetBytes(data);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}