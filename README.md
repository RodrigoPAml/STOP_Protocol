# Simple Server

## Description
Simple Server is a server and client implementation over TCP in C# with: UTF-8 message based comunication, client identification and safe thread operations
It's ideal for builindg more complex things on top of it.

## Features

  - Customizable server with thread safe operations and UTF-8 comunnication
  - Each client is identified with an id provided by the server
  - Customizable client

## Usage of custom server

```C#
static void Main()
{
    ExampleServer server = new ExampleServer(55555);
    server.Run();
} 
```

```C# 
public class ExampleServer : Server
{
    public ExampleServer(int port) : base(port)
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

    protected override void OnClientDisconnected(long clientId)
    {
        Logger.Log($"[Server]: Client {clientId} disconnected");
    }

    protected override void OnRecieveMessage(long clientId, string message)
    {
        Logger.Log($"[Server]: Client {clientId} send message \"{message}\"");
    }

    protected override void OnServerStop()
    {
        Logger.Log("[Server]: Stopped");
    }
}
```

## Usage of custom client

```C#
static void Main()
{
    ExampleClient client = new ExampleClient();
  
    client.Connect("127.0.0.1", 55555);
    client.Disconnect();
} 
```

```C#
public class ExampleClient : Client
{
    public ExampleClient() : base(bufferSize)
    {
    }

    protected override void OnConnect()
    {
        Logger.Log($"[Client]: Connected into the server in {Host}:{Port} with id {Id}");
    }

    protected override void OnDisconnect()
    {
        Logger.Log($"[Client]: Disconnected from server in {Host}:{Port}");
    }

    protected override void OnRecieveMessage(string message)
    {
        Logger.Log($"[Client]: Recieved message from server: \"{message}\"");
    }
}
```

## Sever Overview
All protected members can be turned public

### Constructor

```C#
  // Port to listen
  // bufferSize for each client to recieve messages
  // If the implemented callbacks of the server will be thread safe between they
  public Server(int port, int bufferSize = 4096, bool syncronizeCallbacks = true)
```

### Running

```C#
  // If the server is running and listening in the port
  protected bool Running { get; }
```

### Port

```C#
  // The server listening port
  protected readonly int Port;
```

### BufferSize

```C#
  // The server buffer size used by each client in bytes
  protected readonly int BufferSize;
```

### SyncronedCallbacks

```C#
  // If the implemented server callbacks are thread safe between they
  protected readonly bool SyncronizedCallbacks;
```

### ConnectedClients

```C#
  // The current id of the connected clients
  protected List<long> ConnectedClients { get; }
```

### Lock

```C#
  // The lock used to syncronize the server callbacks
  // You can use it manually if you disable the automatic use via constructor (syncronizeCallbacks)
  protected object Lock { get; private set; }
```
### Send

```C#
  // Send a message to some client
  protected void Send(long clientId, string message)
```

### Close

```C#
  // Close the connection with some client
  protected void Close(int clientId)
```

### Stop

```C#
  // Stop the server
  protected void Stop()
```

## Client Overview
All protected members can be turned public
  
### Constructor

```C#
  public Client(int bufferSize = 64_000) // The buffer size for recieving messages from server
```

### Connect

```C#
  public void Connect(string host, int port) // Connect into server with ip and port
```

### Disconnect

```C#
  // Disconnect from server, wait thread means join the client thread until everything if finished
  // If called from the inside of the callbacks of the client, this should be false to avoid deadlock
  public void Disconnect(bool waitThread = true) 
```

### Id

```C#
  // The id provided by the server for this client
  protected long Id { get; private set; }
```

### IsConnected

```C#
  // If the client is currently connected
  protected bool IsConnected { get; private set; }
```

### BufferSize

```C#
  // the current buffer size in bytes to recieve messages
  protected int BufferSize { get; private set; }
```

### Host

```C#
  // The connected server ip address
  protected int Port { get; private set; }
```

### Port

```C#
  // The connected server port 
  protected string Host { get; private set; }
```

### Send

```C#
  // Send message to the server
   protected void Send(string content)
```

## Implemeting a client for the server in other language

The layout to send a message to server is a UTF-8 message in the format "BEG" + message size with 10 chars + message + "END"
When connected into the server it will return an id for the client and then start listening for the client messages

```C#
string data = $"BEG{(content.Length + 16).ToString("D10")}" + content + "END";
```

## Improvements

- SSL TCP Connection
- Possibility to send UDP packets
- Use a ThreadPool instead of one thread per client to improve performance

## Examples

### 1 - Basic Client
In this example the client send a message to the server and then  the server return the message to the same client
### 2 - Chat
This example simulates a chat between the clients
### 3 - Performance
This examples calculates the throughput between two clients, a sender and a reciever
### 4 - Sending a object
This examples the client send a class object to the server

