using Spectre.Console;

namespace SLMP.Test {
    class Program
    {
        private Config config;
        private Client client;
        private string ADDRESS = "192.168.3.201";

        Program()
        {
            config = new Config()
                .Port(6000)
                .ConnTimeout(500)
                .RecvTimeout(1000)
                .SendTimeout(1000);
            client = new Client(config);
        }

        public static void Main(string[] args)
        {
            Log(LogType.INFO, "Test programi baslatiliyor");
            var program = new Program();
            program.Connect();
        }

        private void ExitTest()
        {
            Log(LogType.INFO, "Test programi bitiriliyor");
        }

        private void Connect()
        {
            Log(LogType.INFO, "PLC'e baglanti testi baslatiliyor");
            try
            {
                client.Connect(ADDRESS);
            }
            catch (Exception ex)
            {
                Log(LogType.ERROR, $"Baglanti saglanamadi: {ex.Message}");
                ExitTest();
            }
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
            AnsiConsole.Markup($"[[[{mrkpStr}]{typeStr,5}[/]]] [[{dateStr}]] {message}");
            Console.WriteLine($"");
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
