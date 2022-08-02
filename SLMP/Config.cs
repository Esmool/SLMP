namespace SLMP
{
    /// <summary>
    /// This class is intended to be passed to the `Client` class.
    /// It describes the required configuration for the SLMP client in an
    /// abstracted way, which is way better than having them as attributes in the `Client` class.
    /// </summary>
    public class Config
    {
        /// we also need to have a field for the type of the PLC at hand
        /// since subcommand stuff depends on the PLC type
        public int port;
        public int conn_timeout;
        public int recv_timeout;
        public int send_timeout;

        public Config(int port, int conn_timeout, int recv_timeout, int send_timeout)
        {
            this.port = port;
            this.conn_timeout = conn_timeout;
            this.recv_timeout = recv_timeout;
            this.send_timeout = send_timeout;
        }
    }
}
