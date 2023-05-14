namespace SimpleServer.Examples
{
    public class ChatServer : Server
    {
        private List<long> _initilizedClients = new List<long>();    

        public ChatServer(int port, int bufferSize) : base(port, bufferSize)
        {
        }

        protected override void OnServerStart()
        {
            Logger.Log("[Server]: Chat open");
        }

        protected override void OnClientConnected(long clientId)
        {
            foreach (var client in _initilizedClients)
                this.Send(clientId, $"Client {client} enter the chat");

            _initilizedClients.Add(clientId);

            foreach (var client in _initilizedClients)
                this.Send(client, $"Client {clientId} enter the chat");

            Logger.Log($"[Server]: Client {clientId} enter the chat");
        }

        protected override void OnClientDisconnected(long clientId, Exception e)
        {
            _initilizedClients.Remove(clientId);

            foreach (var client in _initilizedClients)
                this.Send(client, $"Client {clientId} left the chat");

            Logger.Log($"[Server]: Client {clientId} left");
        }

        protected override void OnRecieveMessage(long clientId, string message)
        {
            foreach(var client in _initilizedClients)
                this.Send(client, $"Client {clientId} send \"{message}\"");

            Logger.Log($"[Server]: Client {clientId} send \"{message}\"");
        }

        protected override void OnServerStop()
        {
            Logger.Log("[Server]: Chat closed");
            _initilizedClients.Clear();
        }
    }
}
