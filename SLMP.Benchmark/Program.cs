using Spectre.Console;

namespace SLMP.Benchmark
{ 
    class Program
    {
        private Config slmpConfig;
        private Client slmpClient;
        private string ADDRESS;

        Program(string addr, int port)
        {
            ADDRESS = addr;
            slmpConfig = new Config()
                .Port(port)
                .ConnTimeout(500)
                .RecvTimeout(1000)
                .SendTimeout(1000);
            slmpClient = new Client(slmpConfig);
        }

        public static void Main(string[] args)
        {
            Log(LogType.INFO, "Benchmark programi baslatiliyor");

            Program program;
            if (args.Length == 3)
                program = new Program(args[0], Int32.Parse(args[1]));
            else
                program = new Program("192.168.3.201", 6000);

            program.Connect();
        }

        private void Connect()
        {
            slmpClient.Connect(ADDRESS);
            slmpClient.ReadDevice(WordDevice.D, 200, 10)
                .ForEach(p => Console.WriteLine("${p:X2}" ));
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
