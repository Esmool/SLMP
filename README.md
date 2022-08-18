# SLMP
SLMP (a subset of it) client library for C#

**WARNING:** This is my first C# project and despite being a fairly proficient programmer, I'm skeptical of the code and my understanding of SLMP. Therefore I'd not advise you to use this library in production without proper testing. Please implement proper security measures to mitigate any unintended consequences that using this library can cause, e.g. worst case, someone's death. There are no unit tests as of now, use this library at your own risk.

# Examples

### Reading/writing into registers
```C#
Config plcConfig = new Config("192.168.3.201")
    .Port(6000)
    .ConnTimeout(500)
    .RecvTimeout(500)
    .SendTimeout(500);
Client plcClient = new Client(plcConfig);
plcClient.Connect();

// reading form word/bit devices
var _ = plcClient.ReadDevice(BitDevice.M, 200, 18); // an array of bools starting from 
                                                    // M200 and ending on M217
var _ = plcClient.ReadDevice(WordDevice.D, 200, 8); // an array of `ushort`s starting from
                                                    // D200 and ending on D207

// reading to word/bit devices
plcClient.WriteDevice(BitDevice.M, 200, new bool[]{ true, false });        // write `true, false` to `BitDevice.M`
                                                                           // starting from M200
plcClient.WriteDevice(WordDevice.D, 200, new ushort[] { 0xdead, 0xbeef }); // write `0xdead, 0xbeef` to `WordDevice.D`
                                                                           // starting from D200

// read/write strings
plcClient.WriteString(WordDevice.D, 200, "SLMPSTRING"); // write a string to `WordDevice.D`
var _ = plcClient.ReadString(WordDevice.D, 200, 10);    // read a string of length `10`
```

### Reading structures
```C#
public struct ExampleStruct
{
    public bool boolean_word;
    public int signed_double_word;
    public uint unsigned_double_word;
    public short short_signed_word;
    public ushort ushort_unsigned_word;
    [SLMPString(length = 6)]
    public string even_length_string;
    [SLMPString(length = 5)]
    public string odd_length_string;
}

Config plcConfig = new Config("192.168.3.201")
    .Port(6000)
    .ConnTimeout(500)
    .RecvTimeout(500)
    .SendTimeout(500);
Client plcClient = new Client(plcConfig);
plcClient.Connect();

var _ = plcClient.ReadStruct<ExampleStruct>(STRUCT_DEVICE, STRUCT_ADDR);
```
