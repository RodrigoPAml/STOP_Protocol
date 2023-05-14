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
            Logger.Log("[Server]: Chat aberto");
        }

        protected override void OnClientConnected(long clientId)
        {
            // Avisa novo client sobre quem já estava no chat
            foreach (var client in _initilizedClients)
                this.Send(clientId, $"Cliente {client} entrou no chat");

            _initilizedClients.Add(clientId);

            // Avisa clientes sobre novo cliente
            foreach (var client in _initilizedClients)
                this.Send(client, $"Cliente {clientId} entrou no chat");

            Logger.Log($"[Server]: Cliente {clientId} entrou no chat");
        }

        protected override void OnClientDisconnected(long clientId)
        {
            _initilizedClients.Remove(clientId);

            foreach (var client in _initilizedClients)
                this.Send(client, $"Cliente {clientId} saiu no chat");

            Logger.Log($"[Server]: Cliente {clientId} saiu");
        }

        protected override void OnRecieveMessage(long clientId, string message)
        {
            foreach(var client in _initilizedClients)
                this.Send(client, $"Cliente {clientId} disse \"{message}\"");

            Logger.Log($"[Server]: Cliente {clientId} enviou \"{message}\"");
        }

        protected override void OnServerStop()
        {
            Logger.Log("[Server]: Chat fechado");
            _initilizedClients.Clear();
        }
    }
}
