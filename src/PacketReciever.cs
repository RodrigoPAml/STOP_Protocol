namespace STOP
{
    /// <summary>
    /// Class to handle the recieve of a message
    /// Format is BEG + SIZE + MESSAGE + END
    /// </summary>
    public class PacketReciever
    {
        public int ExpectedSize { get; private set; } = 0;

        public bool HasHeader { get; private set; } = false;

        public bool HasSize { get; private set; } = false;

        public bool IsReady { get; private set; } = false;

        public bool HasLeftOver { get; private set; } = false;

        public string RawData { get; private set; } = string.Empty;

        public string Content => RawData.Substring(13, RawData.Length - 16);

        public string LeftOver { get; private set; } = string.Empty;

        public void Recieve(string data)
        {
            if (IsReady)
                ResetState();

            RawData += data;

            // Wait for header
            if (!HasHeader && RawData.Length >= 3)
            {
                if (!RawData.StartsWith("BEG"))
                    throw new Exception("Recieved an invalid packet format");

                HasHeader = true;
            }

            // Wait for size, has the max int lenght (2147483647) with is 10
            if (!HasSize && RawData.Length >= 13)
            {
                if (!int.TryParse(RawData.Substring(3, 10), out int size))
                    throw new Exception("Recieved an invalid packet size");

                ExpectedSize = size;
                HasSize = true;
            }

            if (!HasSize)
                return;

            // Case 1 missing content, just wait for more
            if (RawData.Length < ExpectedSize)
                return;
            // Case 2 content is exactly expected here
            else if (RawData.Length == ExpectedSize)
            {
                if (!RawData.EndsWith("END"))
                    throw new Exception("Recieved an invalid packet format");

                IsReady = true;
            }
            // Case 3 content is more than expected here
            else
            {
                string leftOver = RawData.Substring(ExpectedSize);
                RawData = RawData.Substring(0, ExpectedSize);

                if (!RawData.EndsWith("END"))
                    throw new Exception("Recieved an invalid packet format");

                IsReady = true;
                LeftOver = leftOver;
                HasLeftOver = true;
            }

            if (IsReady && Content.Length == 0)
                throw new Exception("Recieved an empty packet");
        }

        public void ResetState()
        {
            ExpectedSize = 0;
            HasHeader = false;
            HasSize = false;
            IsReady = false;
            HasLeftOver = false;
            RawData = string.Empty;
            LeftOver = string.Empty;
        }

        public override string ToString()
        {
            string fmt = string.Empty;

            fmt += "Content:" + Content + '\n';
            fmt += "CurrentSize:" + Content.Length + '\n';
            fmt += "HasHeader:" + HasHeader + '\n';
            fmt += "HasSize:" + HasSize + '\n';
            fmt += "IsReady:" + IsReady + '\n';
            fmt += "HasLeftOver:" + HasLeftOver + '\n';
            fmt += "LeftOver:" + LeftOver + '\n';
            fmt += "RawData:" + RawData + '\n';
            fmt += "ExpectedTotalSize:" + ExpectedSize + '\n';
            return fmt;
        }
    }
}