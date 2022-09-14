namespace SLMP {
    /// <summary>
    /// This class is intended to be passed to the `SlmpClient` class. It describes the required
    /// configuration for the SLMP client in an abstracted way, which is way better than having
    /// them as random attributes scattered around in the `SlmpClient` class.
    /// </summary>
    public class SlmpConfig {
        public string Address { get; set; }
        public int Port { get; set; }
        public int ConnTimeout { get; set; } = 1000;
        public int RecvTimeout { get; set; } = 1000;
        public int SendTimeout { get; set; } = 1000;
    }
}
