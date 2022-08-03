using Spectre.Console;

namespace SLMP.Test {
    class Program
    {
        private Config config;
        private Client client;
        private string ADDRESS;

        BitDevice[] BitDevices = (BitDevice[])Enum.GetValues(typeof(BitDevice));
        WordDevice[] WordDevices = (WordDevice[])Enum.GetValues(typeof(WordDevice));

        Program(string addr, int port)
        {
            ADDRESS = addr;
            config = new Config()
                .Port(port)
                .ConnTimeout(500)
                .RecvTimeout(1000)
                .SendTimeout(1000);
            client = new Client(config);
        }

        public static void Main(string[] args)
        {
            Log(LogType.INFO, "Test programi baslatiliyor");
            Program program = new Program(args[0], Int32.Parse(args[1]));
            program.Connect(); Console.WriteLine("-------");
            program.ReadBitDevices(); Console.WriteLine("-------");
            program.ReadWordDevices(); Console.WriteLine("-------");
            program.WriteBitDevices(); Console.WriteLine("-------");
            program.WriteWordDevices(); Console.WriteLine("-------");
            program.ExitTest();
        }

        private void ExitTest()
        {
            Log(LogType.INFO, "Test programi bitiriliyor");
            System.Environment.Exit(0);
        }

        /// baglanti testi
        private void Connect()
        {
            Log(LogType.INFO, "PLC'e baglanti testi baslatiliyor");
            Log(LogType.DEBUG, $"Baglanilan adres: {ADDRESS}:{config.port}");
            try
            {
                client.Connect(ADDRESS);
                Log(LogType.INFO, "Baglanti basarili");
            }
            catch (Exception ex)
            {
                Log(LogType.ERROR, $"Baglanti saglanamadi: {ex.Message}");
                Console.WriteLine("-------");
                ExitTest();
            }
        }

        private void ReadBitDevices()
        {
            bool errorOccured = false;
            Log(LogType.INFO, "Bit cihazlarindan okuma testi baslatiliyor");
            foreach (BitDevice device in BitDevices)
            {
                try
                {
                    Log(LogType.DEBUG, $"Read({device.ToString()}, addr=0, count=32)");
                    client.ReadDevice(device, 0, 32);
                }
                catch (Exception ex)
                {
                    Log(LogType.ERROR, $"{device.ToString()} cihazindan okuma yapilamadi: {ex.Message}");
                    errorOccured = true;
                    continue;
                }
            }
            
            if (!errorOccured)
                Log(LogType.INFO, "ReadBitDevices testi basari ile tamamlandi");
            else
                Log(LogType.ERROR, "ReadBitDevices testi hata ile sounclandi");
        }

        private void ReadWordDevices()
        {
            bool errorOccured = false;
            Log(LogType.INFO, "Word cihazlarindan okuma testi baslatiliyor");
            foreach (WordDevice device in WordDevices)
            {
                try
                {
                    Log(LogType.DEBUG, $"Read({device.ToString()}, addr=0, count=32)");
                    client.ReadDevice(device, 0, 32);
                }
                catch (Exception ex)
                {
                    Log(LogType.ERROR, $"{device.ToString()} cihazindan okuma yapilamadi: {ex.Message}");
                    errorOccured = true;
                    continue;
                }
            }
            
            if (!errorOccured)
                Log(LogType.INFO, "ReadWordDevices testi basari ile tamamlandi");
            else
                Log(LogType.ERROR, "ReadWordDevices testi hata ile sounclandi");
        }

        private void WriteBitDevices()
        {
            bool errorOccured = false;
            Log(LogType.INFO, "Bit cihazlarina yazma testi baslatiliyor");
            foreach (BitDevice device in BitDevices)
            {
                try
                {
                    Log(LogType.DEBUG, $"Write({device.ToString()}, addr=0, [[true, false, true, false]])");
                    client.WriteDevice(device, 0, new bool[]{ true, false, true, false });
                }
                catch (Exception ex)
                {
                    Log(LogType.ERROR, $"{device.ToString()} cihazina yazma yapilamadi: {ex.Message}");
                    errorOccured = true;
                    continue;
                }
            }
            
            if (!errorOccured)
                Log(LogType.INFO, "WriteBitDevices testi basari ile tamamlandi");
            else
                Log(LogType.ERROR, "WriteBitDevices testi hata ile sounclandi");
        }

        private void WriteWordDevices()
        {
            bool errorOccured = false;
            Log(LogType.INFO, "Word cihazlarina yazma testi baslatiliyor");
            foreach (WordDevice device in WordDevices)
            {
                try
                {
                    Log(LogType.DEBUG, $"Write({device.ToString()}, addr=0, [[0xdead, 0xbeef]])");
                    client.WriteDevice(device, 0, new ushort[]{ 0xdead, 0xbeef });
                }
                catch (Exception ex)
                {
                    Log(LogType.ERROR, $"{device.ToString()} cihazina yazma yapilamadi: {ex.Message}");
                    errorOccured = true;
                    continue;
                }
            }
            
            if (!errorOccured)
                Log(LogType.INFO, "WriteWordDevices testi basari ile tamamlandi");
            else
                Log(LogType.ERROR, "WriteWordDevices testi hata ile sounclandi");
        }

        private static void Log(LogType type, string message)
        {
            string mrkpStr = "none";
            switch (type)
            {
                case LogType.DEBUG: mrkpStr = "blue"; break;
                case  LogType.INFO: mrkpStr = "green"; break;
                case  LogType.WARN: mrkpStr = "yellow"; break;
                case LogType.ERROR: mrkpStr = "red"; break;
            }
            string typeStr = type.ToString();
            string dateStr = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            AnsiConsole.MarkupLine($"[[[{mrkpStr}]{typeStr,5}[/]]] [[{dateStr}]] {message}");
        }
    }

    enum LogType
    {
        DEBUG,
        INFO,
        WARN,
        ERROR,
    }
}
