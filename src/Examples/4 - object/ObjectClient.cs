using Newtonsoft.Json;

namespace SimpleServer.Examples
{
    public class ObjectClient : Client
    {
        public ObjectClient(int bufferSize) : base(bufferSize) 
        { 
        }   

        protected override void OnConnect()
        {
            Logger.Log($"[Client]: Connected in the server at {Host}:{Port}, my id is {Id}");
        }

        protected override void OnDisconnect(Exception e)
        {
            Logger.Log($"[Client]: Disconnected in the server at {Host}:{Port}");
        }

        protected override void OnRecieveMessage(string message)
        {
        }

        public void SendPeople(People p)
        {
            var people = JsonConvert.SerializeObject(p);
            this.Send(people);
        }
    }
}
