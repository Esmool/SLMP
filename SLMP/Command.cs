using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMP
{
    public enum Command
    {
        DeviceRead  = 0x0401,
        DeviceWrite = 0x1401,
        ClearError  = 0x1617,
    }
}
