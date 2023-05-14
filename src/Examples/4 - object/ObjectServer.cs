using Newtonsoft.Json;

namespace SimpleServer.Examples
{
    public class ObjectServer : Server
    {
        public ObjectServer(int port, int bufferSize) : base(port, bufferSize)
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
            var people = JsonConvert.DeserializeObject<People>(message);
            Logger.Log($"[Server]: Cliente {clientId} mandou pessoa \"{people.Name}\"");
        }

        protected override void OnServerStop()
        {
            Logger.Log("[Server]: Parei de funcionar");
        }
    }
}
