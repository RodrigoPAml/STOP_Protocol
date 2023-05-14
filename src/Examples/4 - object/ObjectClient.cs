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
            Logger.Log($"[Cliente]: Me conectei ao servidor em {Host}:{Port}, meu id é {Id}");
        }

        protected override void OnDisconnect()
        {
            Logger.Log($"[Cliente]: Me desconectei do servidor em {Host}:{Port}");
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
