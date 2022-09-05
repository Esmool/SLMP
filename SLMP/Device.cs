namespace SLMP {
    /// <summary>
    /// This enum encodes the supported word devices that is available to operate on.
    /// </summary>
    public enum WordDevice {
        D = 0xa8,
        W = 0xb4,
        R = 0xaf,
        ZR = 0xb0,
        SD = 0xa9,
    }

    /// <summary>
    /// This enum encodes the supported bit devices that is available to operate on.
    /// </summary>
    public enum BitDevice {
        X = 0x9c,
        Y = 0x9d,
        M = 0x90,
        B = 0xa0,
        SM = 0x91,
    }

    public class DeviceMethods {
        public static bool FromString(string device, out WordDevice? value) {
            switch (device) {
                case "D": value = WordDevice.D; break;
                case "W": value = WordDevice.W; break;
                case "R": value = WordDevice.R; break;
                case "ZR": value = WordDevice.ZR; break;
                case "SD": value = WordDevice.SD; break;
                default:
                    value = null;
                    return false;
            };

            return true;
        }

        public static bool FromString(string device, out BitDevice? value) {
            switch (device) {
                case "X": value = BitDevice.X; break;
                case "Y": value = BitDevice.Y; break;
                case "M": value = BitDevice.M; break;
                case "B": value = BitDevice.B; break;
                case "SM": value = BitDevice.SM; break;
                default:
                    value = null;
                    return false;
            };

            return true;
        }
    }
}
