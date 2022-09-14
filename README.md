# SLMP
This project implements a client library that supports a subset of the functionality described in the [SLMP reference manual](https://www.allied-automation.com/wp-content/uploads/2015/02/MITSUBISHI_manual_plc_iq-r_slmp.pdf), mainly regarding reading from and writing to `Device`s.

**WARNING:** I'd not advise you to use this library in production without testing your software throughly. Please implement proper security measures to mitigate any unintended consequences that using this library can cause, e.g. worst case, someone's death. There are no unit tests as of now, use this library at your own risk.

### Documentation

[Auto-generated documentation](https://brkp.github.io/SLMP/) with the help of [Natural Docs](https://www.naturaldocs.org/).

# Examples

### Connecting to and Disconnecting from an SLMP Server
```C#
SlmpConfig cfg = new SlmpConfig("192.168.3.39", 6000) {
    ConnTimeout = 1000,
    RecvTimeout = 1000,
    SendTimeout = 1000,
};
SlmpClient plc = new SlmpClient(cfg);

plc.Connect();
plc.Disconnect();
```

### Reading/writing into registers
```C#
// reading form word/bit devices
plc.ReadDevice(BitDevice.M, 200, 18); // an array of bools starting from M200 and ending on M217
plc.ReadDevice(WordDevice.D, 200, 8); // an array of `ushort`s starting from D200 and ending on D207

// reading to word/bit devices
plc.WriteDevice(BitDevice.M, 200, new bool[]{ true, false });        // write `true, false` to `BitDevice.M` starting from M200
plc.WriteDevice(WordDevice.D, 200, new ushort[] { 0xdead, 0xbeef }); // write `0xdead, 0xbeef` to `WordDevice.D` starting from D200

// read/write strings (they're null terminated in the slmp device's memory)
plc.WriteString(WordDevice.D, 200, "SLMPSTRING"); // write a string to `WordDevice.D`
plc.ReadString(WordDevice.D, 200, 10);            // read a string of length `10`
```

### Reading structures
```C#
public struct ExampleStruct {
    public bool boolean_word;               // 2 bytes, 1 word
    public int signed_double_word;          // 4 bytes, 2 words
    public uint unsigned_double_word;       // 4 bytes, 2 words
    public short short_signed_word;         // 2 bytes, 1 word
    public ushort ushort_unsigned_word;     // 2 bytes, 1 word
    [SLMPString(length = 6)]
    public string even_length_string;       // 6 bytes, 3 words (there's an extra 0x0000 right after the string in the plc memory)
    [SLMPString(length = 5)]
    public string odd_length_string;        // 5 bytes, 3 words (upper byte of the 3rd word is 0x00)
}

plc.ReadStruct<ExampleStruct>(WordDevice.D, 200);
```
