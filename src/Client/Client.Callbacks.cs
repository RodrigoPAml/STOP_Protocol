namespace SimpleServer
{
    public abstract partial class Client
    {
        /// <summary>
        /// When the client connect on server
        /// </summary>
        protected abstract void OnConnect();

        /// <summary>
        /// When the disconnect from server
        /// </summary>
        protected abstract void OnDisconnect(Exception e);

        /// <summary>
        /// When the recieves a message from server
        /// </summary>
        protected abstract void OnRecieveMessage(string message);
    }
}