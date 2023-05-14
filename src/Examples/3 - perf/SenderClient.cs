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
            Logger.Log($"[Client]: Connected in the server at {Host}:{Port}, my id is {Id}");
        }

        protected override void OnDisconnect()
        {
            Logger.Log($"[Client]: Disconnected from server at {Host}:{Port}");
        }

        protected override void OnRecieveMessage(string message)
        {
            Logger.Log($"[Client]: Recived from server at {Host}:{Port}, message: \"{message}\"");
        }

        public new void Send(string message)
        {
            base.Send(message);
        }
    }
}
