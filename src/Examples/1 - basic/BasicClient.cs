namespace SimpleServer.Examples
{
    public class BasicClient : Client
    {
        public BasicClient(int bufferSize) : base(bufferSize)
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
