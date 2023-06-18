using Newtonsoft.Json;

namespace STOP.Examples
{
    public class ObjectServer : Server
    {
        public ObjectServer(int port, int bufferSize) : base(port, bufferSize)
        {
        }

        protected override void OnServerStart()
        {
            Logger.Log("[Server]: Started");
        }

        protected override void OnClientConnected(long clientId)
        {
            Logger.Log($"[Server]: Client {clientId} connected");
        }

        protected override void OnClientDisconnected(long clientId, Exception e)
        {
            Logger.Log($"[Server]: Client {clientId} disconnected");
        }

        protected override void OnRecieveMessage(long clientId, string message)
        {
            var people = JsonConvert.DeserializeObject<People>(message);
            Logger.Log($"[Server]: Client {clientId} send people \"{people.Name}\"");
        }

        protected override void OnServerStop()
        {
            Logger.Log("[Server]: Stopped");
        }
    }
}
