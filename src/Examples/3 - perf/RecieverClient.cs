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
            Logger.Log($"[Client]: Connected in server at {Host}:{Port}, my id is {Id}");

            _watch = new Stopwatch();
            _watch.Start(); 
        }

        protected override void OnDisconnect(Exception e)
        {
            Logger.Log($"[Client]: Disconnected from server at {Host}:{Port}");
        }

        protected override void OnRecieveMessage(string message)
        {
            lock (_lock)
            {
                _recievedBytes += (ulong)(message.Length);
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

            Logger.Log($"[Client]: Throughput was {(bytes / 1_000_000) / elapsedTime} megabytes per second");
        }
    }
}
