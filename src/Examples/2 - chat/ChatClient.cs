namespace STOP.Examples
{
    public class ChatClient : Client
    {
        public ChatClient(int bufferSize) : base(bufferSize) 
        { 
        }

        protected override void OnConnect()
        {
            Logger.Log($"[Client]: Connected in the server at {Host}:{Port}, my id is {Id}");
        }

        protected override void OnDisconnect(Exception e)
        {
            Logger.Log($"[Client]: Disconnected from the server at {Host}:{Port}");
        }

        protected override void OnRecieveMessage(string message)
        {
            Logger.Log($"[Message]: {message}");
        }

        public new void Send(string content)
        {
            base.Send(content);
        }
    }
}
