namespace SimpleServer.Examples
{
    public class BasicServer : Server
    {
        public BasicServer(int port, int bufferSize) : base(port, bufferSize)
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
            Logger.Log($"[Server]: Client {clientId} disconnected");
        }

        protected override void OnRecieveMessage(long clientId, string message)
        {
            if(message == "exit")
                this.Stop();

            Logger.Log($"[Server]: Client {clientId} send a message: \"{message}\". Sending back");
            this.Send(clientId, message);
        }

        protected override void OnServerStop()
        {
            Logger.Log("[Server]: Stopped");
        }
    }
}
