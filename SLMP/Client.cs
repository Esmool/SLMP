using System.Net.Sockets;

namespace SLMP
{
    public class Client
    {
        private TcpClient stream;
        private Config config;

        public Client(Config cfg)
        {
            config = cfg;
            stream = new TcpClient();
        }

        public void Connect(string address)
        {
            var result = this.stream.ConnectAsync(address, this.config.port).Wait(config.conn_timeout);
            if (!result)
                throw new Exception("Connection error");

            // connection is successful
            stream.SendTimeout = config.send_timeout;
            stream.ReceiveTimeout = config.recv_timeout;
        }
    }
}