using SimpleServer.Examples;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("1 - basic server");
        Console.WriteLine("2 - basic client");
        Console.WriteLine();

        Console.WriteLine("3 - server chat");
        Console.WriteLine("4 - client chat");
        Console.WriteLine();

        Console.WriteLine("5 - performance server");
        Console.WriteLine("6 - sender client");
        Console.WriteLine("7 - reciever client");
        Console.WriteLine();

        Console.WriteLine("8 - server object");
        Console.WriteLine("9 - client object");
        Console.WriteLine();

        Console.WriteLine("10 - server safe test");
        Console.WriteLine("11 - client");
        Console.WriteLine();

        string text = Console.ReadLine();

        if (!int.TryParse(text, out int number))
            number = 0;

        switch (number)
        {
            case 1:
                Option1();
                break;
            case 2:
                Option2();
                break;
            case 3:
                Option3();
                break;
            case 4:
                Option4();
                break;
            case 5:
                Option5();
                break;
            case 6:
                Option6();
                break;
            case 7:
                Option7();
                break;
            case 8:
                Option8();
                break;
            case 9:
                Option9();
                break;
            case 10:
                Option10();
                break;
            case 11:
                Option11();
                break;
        }
    }

    static void Option1()
    {
        BasicServer server = new BasicServer(55555, 64_000);
        server.Run();
    }

    static void Option2()
    {
        BasicClient client = new BasicClient(64_000);
        client.Connect("127.0.0.1", 55555);

        Console.WriteLine("Type a message to send to the server!");
        Console.WriteLine("Type exit to end the server and the client");

        string text = Console.ReadLine();
        while (text != "exit")
        {
            if (text != null)
                client.Send(text);

            text = Console.ReadLine();
        }

        client.Disconnect(true);
    }

    static void Option3()
    {
        ChatServer server = new ChatServer(55555, 64_000);
        server.Run();
    }

    static void Option4()
    {
        ChatClient client = new ChatClient(64_000);
        client.Connect("127.0.0.1", 55555);

        Console.WriteLine("Type a message to send into the chat!");
        Console.WriteLine("Type exit to exit the chat");

        string text = Console.ReadLine();
        while (text != "exit")
        {
            if (text != null)
                client.Send(text);

            text = Console.ReadLine();
        }

        client.Disconnect(true);
    }

    static void Option5()
    {
        // You can change the buffer size to improve performance
        Console.WriteLine("Write the buffer size in bytes: ");
        string text = Console.ReadLine();

        if (!uint.TryParse(text, out uint bufferSize))
            bufferSize = 64_000;

        PerfServer server = new PerfServer(55555, (int)bufferSize);
        server.Run();
    }

    static void Option6()
    {
        // You can change the buffer size and payload size to improve performance
        Console.WriteLine("Write the buffer size in bytes: ");
        string text = Console.ReadLine();

        if (!uint.TryParse(text, out uint bufferSize))
            bufferSize = 64_000;

        Console.WriteLine("Write the payload size (in char): ");
        text = Console.ReadLine();

        if (!int.TryParse(text, out int payloadSize))
            payloadSize = 1000;

        SenderClient sender = new SenderClient((int)bufferSize);
        sender.Connect("127.0.0.1", 55555);

        string payload = "";
        for (int i = 0; i < payloadSize; i++)
            payload += "a";
         
        while (true)
        {
            sender.Send(payload);
        }
    }

    static void Option7()
    {
        // You can change the buffer size to improve performance
        Console.WriteLine("Write the buffer size in bytes: ");
        string text = Console.ReadLine();

        if (!uint.TryParse(text, out uint bufferSize))
            bufferSize = 0;

        RecieverClient reciever = new RecieverClient((int)bufferSize);
        reciever.Connect("127.0.0.1", 55555);

        while(true) 
        {
            Thread.Sleep(1000);
            reciever.LogResults();
        }
    }

    static void Option8()
    {
        ObjectServer server = new ObjectServer(55555, 64_000);
        server.Run();
    }

    static void Option9()
    {
        ObjectClient client = new ObjectClient(64_000);
        client.Connect("127.0.0.1", 55555);
        client.SendPeople(new People()
        {
            Name = "Maria",
            Age = 33
        });

        client.Disconnect(true);
        Console.ReadLine();
    }

    static void Option10()
    {
        // Change the boolean to see the thread safety difference
        SafeServer server = new SafeServer(55555, 64_000, true);
        server.Run();
    }

    static void Option11()
    {
        Thread.Sleep(1000);

        SafeClient client = new SafeClient(64_000);
        client.Connect("127.0.0.1", 55555);

        for (int i = 0; i < 200000; i++)
        {
            client.Send($"dummy{i}");
        }

        Console.ReadLine();
        client.Disconnect(true);
        Console.ReadLine();
    }
}
