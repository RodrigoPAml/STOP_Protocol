using SimpleServer.Examples;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Digite 1 - servidor básico!");
        Console.WriteLine("Digite 2 - cliente básico!");
        Console.WriteLine();

        Console.WriteLine("Digite 3 - servidor do chat!");
        Console.WriteLine("Digite 4 - cliente do chat!");
        Console.WriteLine();

        Console.WriteLine("Digite 5 - servidor de performance!");
        Console.WriteLine("Digite 6 - cliente envio performance!");
        Console.WriteLine("Digite 7 - cliente recebimento performance!");
        Console.WriteLine();

        Console.WriteLine("Digite 8 - servidor recebe objeto!");
        Console.WriteLine("Digite 9 - cliente envia objeto!");
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

        Console.WriteLine("Digite e de enter para enviar ao servidor!");
        Console.WriteLine("Se quer que o servidor finalize digite sair");

        string text = Console.ReadLine();
        while (text != "sair")
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

        Console.WriteLine("Digite e de enter para enviar ao servidor!");
        Console.WriteLine("Para sair digite sair");

        string text = Console.ReadLine();
        while (text != "sair")
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
        Console.WriteLine("Digite o tamanho do buffer em bytes: ");
        string text = Console.ReadLine();

        if (!uint.TryParse(text, out uint bufferSize))
            bufferSize = 0;

        PerfServer server = new PerfServer(55555, (int)bufferSize);
        server.Run();
    }

    static void Option6()
    {
        // You can change the buffer size and payload size to improve performance
        Console.WriteLine("Digite o tamanho do buffer em bytes: ");
        string text = Console.ReadLine();

        if (!uint.TryParse(text, out uint bufferSize))
            bufferSize = 64_000;

        Console.WriteLine("Digite o tamanho do payload (char): ");
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
        Console.WriteLine("Digite o tamanho do buffer em bytes: ");
        string text = Console.ReadLine();

        if (!uint.TryParse(text, out uint bufferSize))
            bufferSize = 0;

        RecieverClient reciever = new RecieverClient((int)bufferSize);
        reciever.Connect("127.0.0.1", 55555);

        Console.WriteLine("Mostrando resultados a cada 1 segundo!");

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
}
