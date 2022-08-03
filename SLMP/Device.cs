namespace SLMP
{
    /// <summary>
    /// This enum encodes the supported word devices that is available to operate on.
    /// </summary>
    public enum WordDevice
    {
        D  = 0xa8,
        W  = 0xb4,
        R  = 0xaf,
        ZR = 0xb0,
        SD = 0xa9,
    }

    /// <summary>
    /// This enum encodes the supported bit devices that is available to operate on.
    /// </summary>
    public enum BitDevice
    {
        X  = 0x9c,
        Y  = 0x9d,
        M  = 0x90,
        B  = 0xa0,
        SM = 0x91,
    }
}
