using System.Diagnostics;

namespace SimpleServer.Examples
{
    public class RecieverClient : Client
    {
        private Stopwatch _watch = null;

        private object _lock = new object();    

        private ulong _recievedBytes = 0;

        public RecieverClient(int bufferSize) : base(bufferSize) 
        { 
        }

        protected override void OnConnect()
        {
            Logger.Log($"[Cliente]: Me conectei ao servidor em {Host}:{Port}, meu id é {Id}");

            _watch = new Stopwatch();
            _watch.Start(); 
        }

        protected override void OnDisconnect()
        {
            Logger.Log($"[Cliente]: Me desconectei do servidor em {Host}:{Port}");
        }

        protected override void OnRecieveMessage(string message)
        {
            lock (_lock)
            {
                _recievedBytes += (ulong)(message.Length * sizeof(char));
            }
        }

        public void LogResults()
        {
            ulong bytes = 0;
            lock (_lock)
            {
                bytes = _recievedBytes;
            }

            double elapsedTime = (_watch.ElapsedMilliseconds/1000);

            Logger.Log($"[Cliente]: Throughput foi {bytes / elapsedTime} bytes por segundo");
            Logger.Log($"[Cliente]: ou { (bytes / 1_000_000) / elapsedTime} megabytes por segundo");
        }
    }
}
