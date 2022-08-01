using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMP
{
    /// <summary>
    /// This class is intended to be passed to the `Client` class.
    /// It describes the required configuration for the SLMP client in an
    /// abstracted way, which is way better than having them as attributes in the `Client` class.
    /// </summary>
    public class Config
    {
        public int port;
        public int conn_timeout;
        public int recv_timeout;
        public int send_timeout;
        /// we also need to have a field for the type of the PLC at hand
        /// since subcommand stuff depends on the PLC type
    }
}
