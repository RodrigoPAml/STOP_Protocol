namespace SimpleServer.Examples
{
    public class SafeServer : Server
    {
        private int _value = 0;

        public SafeServer(int port, int bufferSize, bool safe) : base(port, bufferSize, safe)
        {
        }

        protected override void OnServerStart()
        {
            Logger.Log("[Server]: Started");
        }

        protected override void OnClientConnected(long clientId)
        {
            Logger.Log($"[Server]: Client {clientId} connected");
        }

        protected override void OnClientDisconnected(long clientId, Exception e)
        {
            Logger.Log($"[Server]: Client {clientId} disconnected {e?.Message}");
        }

        protected override void OnRecieveMessage(long clientId, string message)
        {
            _value++;

            Logger.Log($"[Server]: Value is {_value} {message}");
        }

        protected override void OnServerStop()
        {
            Logger.Log("[Server]: Stopped");
        }
    }
}
