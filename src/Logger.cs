namespace SimpleServer
{
    public static class Logger
    {
        private static StreamWriter writer = null;

        public static void Log(string message)
        {
            if (writer == null)
                writer = new StreamWriter($"{DateTime.Now.ToString("dd-MM-yyyy ffff-ss-mm-HH")}_log.txt");

            Console.WriteLine($"{DateTime.Now} - {message}");

            writer.Write($"{DateTime.Now} - {message}");
            writer.Flush();
        }
    }
}
