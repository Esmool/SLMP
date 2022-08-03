using Spectre.Console;
using System.Diagnostics;

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

            program.Connect(); Console.WriteLine("-------");
            program.BenchRead(); Console.WriteLine("-------");
            program.BenchWrite();
        }

        private void Connect()
        {
            Log(LogType.INFO, "PLC'e baglanti testi baslatiliyor");
            Log(LogType.DEBUG, $"Baglanilan adres: {ADDRESS}:{slmpConfig.port}");
            try
            {
                slmpClient.Connect(ADDRESS);
                Log(LogType.INFO, "Baglanti basarili");
            }
            catch (Exception ex)
            {
                Log(LogType.ERROR, $"Baglanti saglanamadi: {ex.Message}");
                System.Environment.Exit(0);
            }
        }

        private void BenchRead()
        {
            Log(LogType.WARN, "BenchRead");

            int loop_count = 500;
            int regr_count = 960;

            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < loop_count; i++)
            {
                slmpClient.ReadDevice(
                    WordDevice.D, (ushort)0, (ushort)regr_count);
            }
            watch.Stop();

            Log(LogType.DEBUG, $"loop_count: {loop_count }");
            Log(LogType.DEBUG, $"register_count: {regr_count}");
            Log(LogType.INFO, $"calisma suresi: {watch.ElapsedMilliseconds}ms");
            Log(LogType.INFO, $"komut sayisi/sn: {1000 * loop_count / watch.ElapsedMilliseconds}");
        }

        private void BenchWrite()
        {
            Log(LogType.WARN, "BenchWrite");

            int loop_count = 500;
            ushort[] wdata = { 0, 0, 0, 0, 0, 0, };

            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < loop_count; i++)
            {
                slmpClient.WriteDevice(WordDevice.D, (ushort)0, wdata);
            }
            watch.Stop();

            Log(LogType.DEBUG, $"loop_count: {loop_count }");
            Log(LogType.INFO, $"calisma suresi: {watch.ElapsedMilliseconds}ms");
            Log(LogType.INFO, $"komut sayisi/sn: {1000 * loop_count / watch.ElapsedMilliseconds}");
        }

        private static void Log(LogType type, string message)
        {
            string mrkpStr = "none";
            switch (type)
            {
                case LogType.DEBUG: mrkpStr = "blue"; break;
                case LogType.INFO:  mrkpStr = "green"; break;
                case LogType.WARN:  mrkpStr = "yellow"; break;
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
