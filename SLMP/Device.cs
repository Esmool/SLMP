using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMP
{
    public enum Device
    {
        SM = 0x91,
        SD = 0xa9,
        X  = 0x9c,
        Y  = 0x9d,
        M  = 0x90,
        B  = 0xa0,
        D  = 0xa8,
        W  = 0xb4,
        Z  = 0xcc,
        R  = 0xaf,
        ZR = 0xb0,
    }

    public enum DeviceType
    {
        Bit,
        Word,
        // DoubleWord,
    }

    public class DeviceExt
    {
        public static DeviceType GetDeviceType(Device device)
        {
            switch (device)
            {
                case Device.X:
                case Device.Y:
                case Device.M:
                case Device.B:
                case Device.SM:
                    return DeviceType.Bit;

                case Device.D:
                case Device.W:
                case Device.Z:
                case Device.R:
                case Device.ZR:
                case Device.SD:
                    return DeviceType.Word;

                default:
                    throw new ArgumentException("invalid device provided");
            }
        }
    }
}
