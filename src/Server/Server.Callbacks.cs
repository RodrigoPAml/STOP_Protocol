namespace SimpleServer
{
    /// <summary>
    /// Callbacks to personalize the server behaviour
    /// </summary>
    public abstract partial class Server
    {
        /// <summary>
        /// Lock to make callbacks operations safe
        /// Can be used manually by the implementation
        /// </summary>
        protected object Lock { get; private set; } = new object();

        /// <summary>
        /// Called when the server starts
        /// </summary>
        protected abstract void OnServerStart();

        /// <summary>
        /// Called when a client is connected
        /// </summary>
        protected abstract void OnClientConnected(long clientId);

        /// <summary>
        /// Called when a client is disconnected
        /// </summary>
        protected abstract void OnClientDisconnected(long clientId);

        /// <summary>
        /// Called when the server recieves a message from a client
        protected abstract void OnRecieveMessage(long clientId, string message);

        /// <summary>
        /// Called when the server stops
        /// </summary>
        protected abstract void OnServerStop();
    }
}