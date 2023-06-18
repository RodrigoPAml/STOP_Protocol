namespace STOP.Examples
{
    public class SafeClient : Client
    {
        public SafeClient(int bufferSize) : base(bufferSize)
        {
        }

        protected override void OnConnect()
        {
            Logger.Log($"[Client]: Connected in the server at {Host}:{Port}, my id is {Id}");
        }

        protected override void OnDisconnect(Exception e)
        {
            Logger.Log($"[Client]: Disconnected from server at {Host}:{Port}");
        }

        protected override void OnRecieveMessage(string message)
        {
        }

        public new void Send(string content)
        {
            base.Send(content);
        }
    }
}
