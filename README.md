# Simple Server

## Description
Simple Server is a server and client implementation over TCP in C# with: text based comunication, client identification and safe thread operations

It's ideal for builindg more complex things on top of it and in an agile way.

The text encoding used is ASCII but can be changed in the source code if needed.

## Features

  - Customizable server with thread safe operations and ASCII comunnication
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

    protected override void OnClientDisconnected(long clientId, Exception e)
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

    protected override void OnDisconnect(Exception e)
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

The layout to send a message to server is a ASCII message in the format "BEG" + message size with 10 chars + message + "END".
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

#### Local host tests

With 64000 buffer size and 32000 length string payload (1 char is 2 bytes)
```
14/05/2023 02:46:56 - [Client]: or 743,5 megabytes per second
14/05/2023 02:46:57 - [Client]: Throughput was 745813333,3333334 bytes per second
14/05/2023 02:46:57 - [Client]: or 745,6666666666666 megabytes per second
14/05/2023 02:46:58 - [Client]: Throughput was 747488000 bytes per second
14/05/2023 02:46:58 - [Client]: or 747,25 megabytes per second
14/05/2023 02:46:59 - [Client]: Throughput was 748032000 bytes per second
14/05/2023 02:46:59 - [Client]: or 748 megabytes per second
14/05/2023 02:47:00 - [Client]: Throughput was 747968000 bytes per second
14/05/2023 02:47:00 - [Client]: or 747,8333333333334 megabytes per second
14/05/2023 02:47:01 - [Client]: Throughput was 748324571,4285715 bytes per second
14/05/2023 02:47:01 - [Client]: or 748,2857142857143 megabytes per second
14/05/2023 02:47:02 - [Client]: Throughput was 748536000 bytes per second
14/05/2023 02:47:02 - [Client]: or 748,5 megabytes per second
14/05/2023 02:47:03 - [Client]: Throughput was 748920888,8888888 bytes per second
14/05/2023 02:47:03 - [Client]: or 748,8888888888889 megabytes per second
14/05/2023 02:47:04 - [Client]: Throughput was 748556800 bytes per second
14/05/2023 02:47:04 - [Client]: or 748,5 megabytes per second
14/05/2023 02:47:05 - [Client]: Throughput was 749934545,4545455 bytes per second
14/05/2023 02:47:05 - [Client]: or 749,9090909090909 megabytes per second
14/05/2023 02:47:06 - [Client]: Throughput was 749349333,3333334 bytes per second
14/05/2023 02:47:06 - [Client]: or 749,3333333333334 megabytes per second
14/05/2023 02:47:07 - [Client]: Throughput was 748435692,3076923 bytes per second
14/05/2023 02:47:07 - [Client]: or 748,3846153846154 megabytes per second
14/05/2023 02:47:08 - [Client]: Throughput was 747812571,4285715 bytes per second
14/05/2023 02:47:10 - [Client]: or 747,7857142857143 megabytes per second
14/05/2023 02:47:11 - [Client]: Throughput was 785688000 bytes per second
14/05/2023 02:47:11 - [Client]: or 785,6875 megabytes per second
```

With only 1024 buffer size and 512 string payload size we can see the difference
```
14/05/2023 02:50:02 - [Client]: Connected in server at 127.0.0.1:55555, my id is 2
14/05/2023 02:50:03 - [Client]: Throughput was 141268992 bytes per second
14/05/2023 02:50:03 - [Client]: or 141 megabytes per second
14/05/2023 02:50:04 - [Client]: Throughput was 139730432 bytes per second
14/05/2023 02:50:04 - [Client]: or 139,5 megabytes per second
14/05/2023 02:50:05 - [Client]: Throughput was 139813205,33333334 bytes per second
14/05/2023 02:50:05 - [Client]: or 139,66666666666666 megabytes per second
14/05/2023 02:50:06 - [Client]: Throughput was 139462656 bytes per second
14/05/2023 02:50:06 - [Client]: or 139,25 megabytes per second
14/05/2023 02:50:07 - [Client]: Throughput was 139188428,8 bytes per second
14/05/2023 02:50:07 - [Client]: or 139 megabytes per second
14/05/2023 02:50:08 - [Client]: Throughput was 138722304 bytes per second
14/05/2023 02:50:08 - [Client]: or 138,66666666666666 megabytes per second
14/05/2023 02:50:09 - [Client]: Throughput was 138613028,57142857 bytes per second
14/05/2023 02:50:09 - [Client]: or 138,57142857142858 megabytes per second
14/05/2023 02:50:10 - [Client]: Throughput was 138498688 bytes per second
14/05/2023 02:50:10 - [Client]: or 138,375 megabytes per second
14/05/2023 02:50:11 - [Client]: Throughput was 138360832 bytes per second
14/05/2023 02:50:11 - [Client]: or 138,33333333333334 megabytes per second
14/05/2023 02:50:12 - [Client]: Throughput was 138553446,4 bytes per second
14/05/2023 02:50:12 - [Client]: or 138,5 megabytes per second
14/05/2023 02:50:13 - [Client]: Throughput was 138551668,36363637 bytes per second
14/05/2023 02:50:13 - [Client]: or 138,54545454545453 megabytes per second
14/05/2023 02:50:14 - [Client]: Throughput was 138631594,66666666 bytes per second
14/05/2023 02:50:14 - [Client]: or 138,58333333333334 megabytes per second
14/05/2023 02:50:15 - [Client]: Throughput was 138685597,53846154 bytes per second
14/05/2023 02:50:15 - [Client]: or 138,6153846153846 megabytes per second
```

### 4 - Sending a object
In this example the client send a class object to the server

### 5 - Thread safe test

In this example many clients increment a value in the server to test if the operations are thread safe

You can change the bool in the server class to see the difference

