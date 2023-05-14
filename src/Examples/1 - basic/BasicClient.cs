namespace SimpleServer.Examples
{
    public class BasicClient : Client
    {
        public BasicClient(int bufferSize) : base(bufferSize)
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
            Logger.Log($"[Client]: Recieved message from server: \"{message}\"");
        }

        public new void Send(string content)
        {
            base.Send(content);
        }
    }
}
