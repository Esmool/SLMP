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
        public int port = 6307;
        public int? connTimeout = null;
        public int? recvTimeout = null;
        public int? sendTimeout = null;

        public Config() {}

        public Config Port(int value)
        {
            port = value;
            return this;
        }

        public Config ConnTimeout(int value)
        {
            connTimeout = value;
            return this;
        }

        public Config RecvTimeout(int value)
        {
            recvTimeout = value;
            return this;
        }

        public Config SendTimeout(int value)
        {
            sendTimeout = value;
            return this;
        }
    }
}
