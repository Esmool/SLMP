using SLMP;

class Program
{
    static void Main(string[] args)
    {
        var config = new Config(6000, 1000, 1000, 1000);
        var client = new Client(config);

        client.Connect("192.168.3.201");
        client.ReadWordDevice(Device.D, 200, 2)
            .ForEach(p => Console.Write($"{p:X} "));
        Console.WriteLine();
        client.ReadBitDevice(Device.M, 200, 6)
            .ForEach(p => Console.Write($"{p} "));
    }
}
