namespace SLMP {
    /// <summary>
    /// This class is intended to be passed to the `Client` class. It describes the required
    /// configuration for the SLMP client in an abstracted way, which is way better than having
    /// them as random attributes scattered around in the `Client` class.
    /// </summary>
    public class Config {
        public string addr;
        public int port = 6307;
        public int? connTimeout = null;
        public int? recvTimeout = null;
        public int? sendTimeout = null;

        /// <summary>
        /// <summary>Initializes a new instance of the <see cref="Config" /> class with default values.</summary>
        /// </summary>
        /// <param name="addr">The addr.</param>
        public Config(string addr) {
            this.addr = addr;
        }

        /// <summary>
        /// Set the connection `port` to something other than the default value.
        /// </summary>
        public Config Port(int value) {
            port = value;
            return this;
        }

        /// <summary>
        /// Set the `connTimeout` to something other than the default value.
        /// </summary>
        public Config ConnTimeout(int? value) {
            connTimeout = value;
            return this;
        }

        /// <summary>
        /// Set the `recvTimeout` to something other than the default value.
        /// </summary>
        public Config RecvTimeout(int? value) {
            recvTimeout = value;
            return this;
        }

        /// <summary>
        /// Set the `sendTimeout` to something other than the default value.
        /// </summary>
        public Config SendTimeout(int? value) {
            sendTimeout = value;
            return this;
        }
    }
}
