using System.Net.Sockets;

namespace SLMP
{
    public class Client
    {
        /// <summary>
        /// This `HEADER` array contains the shared (header) data between
        /// commands that are supported in this library.
        /// </summary>
        private readonly byte[] HEADER = {
            0x50, 0x00,     // subheader: no serial no.
            0x00,           // request destination network no.
            0xff,           // request destination station no.
            0xff, 0x03,     // request destination module I/O no.: 0x03ff (own station)
            0x00,           // request destination multidrop station no.
        };

        private Config config;
        private TcpClient client;
        private NetworkStream? stream;

        public Client(Config cfg)
        {
            config = cfg;
            client = new TcpClient();
        }

        public void Connect(string addr)
        {
            bool result = client.ConnectAsync(addr, config.port).Wait(config.conn_timeout);
            if (!result)
                throw new Exception("Connection error");

            // connection is successful
            client.SendTimeout = config.send_timeout;
            client.ReceiveTimeout = config.recv_timeout;

            stream = client.GetStream();
        }

        public List<bool> ReadBitDevice(Device device, UInt16 addr, UInt16 count)
        {
            SendReadDeviceCommand(device, addr, count);
            List<byte> response = ReceiveResponse();
            List<bool> result = new();

            response.ForEach(delegate(byte a) {
                result.Add((a & 0x10) != 0);
                result.Add((a & 0x01) != 0);
            });

            return result.GetRange(0, count);
        }

        public List<UInt16> ReadWordDevice(Device device, UInt16 addr, UInt16 count)
        {
            SendReadDeviceCommand(device, addr, count);
            List<byte> response = ReceiveResponse();
            List<UInt16> result = new();

            // if the length of the response isn't even
            // then the response is invalid and we can't
            // construct an array of `UInt16`s from it
            if (response.Count() % 2 != 0)
                throw new Exception("invalid response");

            // word data is received in little endian format
            // which means the lower byte of a word comes first
            // and upper byte second
            response
                .Chunk(2)
                .ToList()
                .ForEach(n => result.Add((UInt16)(n[1] << 8 | n[0])));

            return result;

        }

        /// <summary>
        /// Gets the subcommand.
        /// </summary>
        /// <exception cref="System.ArgumentException">invalid device type provided</exception>
        private UInt16 GetSubcommand(DeviceType type)
        {
            switch (type)
            {
                case DeviceType.Bit:
                    return 0x0001;
                case DeviceType.Word:
                    return 0x0000;
                default:
                    throw new ArgumentException("invalid device type provided");
            }
        }

        /// <summary>
        /// This function exists because `NetworkStream` doesn't
        /// have a `recv_exact` method.
        /// </summary>
        private byte[] ReceiveBytes(int count)
        {
            if (stream == null)
                throw new Exception("connection isn't established");

            int offset = 0, toRead = count;
            int read;
            byte[] buffer = new byte[count];

            while (toRead > 0 && (read = stream.Read(buffer, offset, toRead)) > 0)
            {
                toRead -= read;
                offset += read;
            }
            if (toRead > 0) throw new EndOfStreamException();

            return buffer;
        }

        /// <summary>
        /// Sends the read device command.
        /// </summary>
        private void SendReadDeviceCommand(Device device, UInt16 adr, UInt16 cnt)
        {
            if (stream == null)
                throw new Exception("connection isn't established");

            List<byte> rawRequest = HEADER.ToList();

            UInt16 cmd = (UInt16)Command.DeviceRead;
            UInt16 sub = GetSubcommand(DeviceExt.GetDeviceType(device));

            rawRequest.AddRange(new List<byte>(){
                // request data length (in terms of bytes): fixed size (12) for the read command
                0x0c, 0x00,
                // monitoring timer. TODO: make this something configurable instead of hard-coding it.
                0x00, 0x10,
                (byte)(cmd & 0xff), (byte)(cmd >> 0x8),
                (byte)(sub & 0xff), (byte)(sub >> 0x8),
                (byte)(adr & 0xff), (byte)(adr >> 0x8),
                (byte)(0x00),
                (byte)device,
                (byte)(cnt & 0xff), (byte)(cnt >> 0x8),
            });


            stream.Write(rawRequest.ToArray());
        }

        private List<byte> ReceiveResponse()
        {
            if (stream == null)
                throw new Exception("connection isn't established");

            // read a single byte to determine 
            // if a serial no. is included or not
            int value = stream.ReadByte();
            byte[] hdrBuf;
            switch (value)
            {
                // if value is 0xd0, there's no serial no. included
                // in the response
                case 0xd0:
                    hdrBuf = ReceiveBytes(8);
                    break;
                // if value is 0xd4, there's a serial no. included
                // in the response
                case 0xd4:
                    hdrBuf = ReceiveBytes(12);
                    break;
                // in the case where we receive some other data, we mark it
                // as invalid and throw an `Exception`
                default:
                    throw new Exception($"invalid start byte received: {value}");
            }

            int dataSize = hdrBuf[^1] << 8 | hdrBuf[^2];
            List<byte> responseBuffer = ReceiveBytes(dataSize).ToList();

            int endCode = responseBuffer[1] << 8 | responseBuffer[0];
            if (endCode != 0)
                throw new Exception($"non-zero end code: {endCode:X}H");

            responseBuffer.RemoveRange(0, 2);
            return responseBuffer;
        }
    }
}
