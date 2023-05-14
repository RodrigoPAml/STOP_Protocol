namespace SimpleServer.Examples
{
    public class PerfServer : Server
    {
        public PerfServer(int port, int bufferSize) : base(port, bufferSize)
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
            if(ConnectedClients.Count > 1)
            {
                this.Send(ConnectedClients[1], message);
            }
        }

        protected override void OnServerStop()
        {
            Logger.Log("[Server]: Parei de funcionar");
        }
    }
}
