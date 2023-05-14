namespace SimpleServer.Examples
{
    public class SenderClient : Client
    {
        public new bool IsConnected => base.IsConnected;

        public SenderClient(int bufferSize) : base(bufferSize) 
        { 
        }

        protected override void OnConnect()
        {
            Logger.Log($"[Cliente]: Me conectei ao servidor em {Host}:{Port}, meu id é {Id}");
        }

        protected override void OnDisconnect()
        {
            Logger.Log($"[Cliente]: Me desconectei do servidor em {Host}:{Port}");
        }

        protected override void OnRecieveMessage(string message)
        {
            Logger.Log($"[Cliente]: Recebido do servidor em {Host}:{Port} mensagem \"{message}\"");
        }
    }
}
