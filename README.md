# STOP  (Simple Text Oriented Protocol)

## Description

STOP is a server and client implementation over TCP in C# with: text based comunication, client identification and safe thread operations

It was built for studies porpuses but it's useful for builindg more complex things on top of it.

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

## Implemeting a client for the server in other language

The layout to send a message to server is a ASCII message in the format "BEG" + message size with 10 chars + message + "END".
When connected into the server it will return an id for the client and then start listening for the client messages

```C#
string data = $"BEG{(content.Length + 16).ToString("D10")}" + content + "END";
```

## Possible Improvements

- SSL TCP Connection
- Possibility to send UDP packets
- Use a ThreadPool instead of one thread per client to improve performance
- Performance test over LAN/WAN

## Examples

### 1 - Basic Client
In this example the client send a message to the server and then  the server return the message to the same client
### 2 - Chat
This example simulates a chat between the clients
### 3 - Performance
This examples calculates the throughput between two clients, a sender and a reciever

#### Local host tests

With 64000 buffer size and payload
```
14/05/2023 04:50:08 - [Client]: Throughput was 433,625 megabytes per second
14/05/2023 04:50:09 - [Client]: Throughput was 434,32 megabytes per second
14/05/2023 04:50:10 - [Client]: Throughput was 434,9230769230769 megabytes per second
14/05/2023 04:50:11 - [Client]: Throughput was 434,9259259259259 megabytes per second
14/05/2023 04:50:12 - [Client]: Throughput was 434,14285714285717 megabytes per second
14/05/2023 04:50:13 - [Client]: Throughput was 434,44827586206895 megabytes per second
14/05/2023 04:50:14 - [Client]: Throughput was 434 megabytes per second
14/05/2023 04:50:15 - [Client]: Throughput was 434,2258064516129 megabytes per second
14/05/2023 04:50:16 - [Client]: Throughput was 434,46875 megabytes per second
14/05/2023 04:50:17 - [Client]: Throughput was 434,8484848484849 megabytes per second
14/05/2023 04:50:18 - [Client]: Throughput was 434,8235294117647 megabytes per second
14/05/2023 04:50:19 - [Client]: Throughput was 435,25714285714287 megabytes per second
14/05/2023 04:50:20 - [Client]: Throughput was 435,5833333333333 megabytes per second
14/05/2023 04:50:24 - [Client]: Throughput was 438,02564102564105 megabytes per second
```

With only 1024 buffer size and payload we can see the difference
```
14/05/2023 04:51:42 - [Client]: Throughput was 118,23333333333333 megabytes per second
14/05/2023 04:51:43 - [Client]: Throughput was 118,19354838709677 megabytes per second
14/05/2023 04:51:44 - [Client]: Throughput was 118,09375 megabytes per second
14/05/2023 04:51:45 - [Client]: Throughput was 118,0909090909091 megabytes per second
14/05/2023 04:51:46 - [Client]: Throughput was 118,1470588235294 megabytes per second
14/05/2023 04:51:47 - [Client]: Throughput was 118,14285714285714 megabytes per second
14/05/2023 04:51:48 - [Client]: Throughput was 118,13888888888889 megabytes per second
14/05/2023 04:51:49 - [Client]: Throughput was 118,16216216216216 megabytes per second
14/05/2023 04:51:50 - [Client]: Throughput was 118,23684210526316 megabytes per second
14/05/2023 04:51:51 - [Client]: Throughput was 118,25641025641026 megabytes per second
14/05/2023 04:51:52 - [Client]: Throughput was 118,25 megabytes per second
14/05/2023 04:51:53 - [Client]: Throughput was 118,21951219512195 megabytes per second
14/05/2023 04:51:54 - [Client]: Throughput was 118,21428571428571 megabytes per second
```

![image](https://github.com/RodrigoPAml/SimpleServer/assets/41243039/95618d8d-b9a1-43a9-9c58-a0c59e92959c)

### 4 - Sending a object
In this example the client send a class object to the server

### 5 - Thread safe test

In this example many clients increment a value in the server to test if the operations are thread safe

You can change the bool in the server class to see the difference

