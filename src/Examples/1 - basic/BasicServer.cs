namespace SimpleServer.Examples
{
    public class BasicServer : Server
    {
        public BasicServer(int port, int bufferSize) : base(port, bufferSize)
        {
        }

        protected override void OnServerStart()
        {
            Logger.Log("[Server]: Comecei de funcionar");
        }

        protected override void OnClientConnected(long clientId)
        {
            Logger.Log($"[Server]: Cliente {clientId} se conectou");
        }

        protected override void OnClientDisconnected(long clientId)
        {
            Logger.Log($"[Server]: Cliente {clientId} se desconectou");
        }

        protected override void OnRecieveMessage(long clientId, string message)
        {
            if(message == "sair")
                this.Stop();

            Logger.Log($"[Server]: Cliente {clientId} mandou mensagem \"{message}\". Mandando de volta");
            this.Send(clientId, message);
        }

        protected override void OnServerStop()
        {
            Logger.Log("[Server]: Parei de funcionar");
        }
    }
}
