using SLMP;

class Program
{
    static void Main(string[] args)
    {
        var config = new Config(6000, 1000, 1000, 1000);
        var client = new Client(config);

        client.Connect("192.168.3.201");
    }
}
