using System.Net.Sockets;

namespace SLMP
{
    public class Client
    {
        private TcpClient? stream;
        private Config config;

        public Client(Config config)
        {
            this.config = config;
        }
    }
}