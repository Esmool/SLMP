using System.Net.Sockets;

namespace SLMP
{
    public class Client
    {
        /// <summary>This `HEADER` array contains the shared (header) data between
        /// commands that are supported in this library.</summary>
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
            var result = client.ConnectAsync(addr, config.port).Wait(config.conn_timeout);
            if (!result)
                throw new Exception("Connection error");

            // connection is successful
            client.SendTimeout = config.send_timeout;
            client.ReceiveTimeout = config.recv_timeout;

            stream = client.GetStream();
        }

        public List<UInt16> ReadWordDevice(Device device, UInt16 addr, UInt16 count)
        {
            SendReadDevice(device, addr, count);
            var response = RecvResponse();
            var result = new List<UInt16>();

            response
                .Chunk(2)
                .ToList()
                .ForEach(n => result.Add((UInt16)(n[1] << 8 | n[0])));

            return result;

        }

        /// <summary>
        /// Gets the subcommand.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">invalid device type provided</exception>
        private UInt16 GetSubcommand(DeviceType type)
        {
            switch(type)
            {
                case DeviceType.Bit:
                    return 0x0001;
                case DeviceType.Word:
                    return 0x0000;
                default:
                    throw new ArgumentException("invalid device type provided");
            }
        }

        private byte[] RecvBytes(int count)
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
        /// <param name="device">The device.</param>
        /// <param name="adr">The adr.</param>
        /// <param name="cnt">The count.</param>
        /// <exception cref="System.Exception">connection isn't established</exception>
        private void SendReadDevice(Device device, UInt16 adr, UInt16 cnt)
        {
            if (stream == null)
                throw new Exception("connection isn't established");

            var raw_data = HEADER.ToList();

            // request data length (in terms of bytes): fixed size (12) for the read command
            raw_data.Add(0x0c); raw_data.Add(0x00); 
            // monitoring timer. TODO: make this something configurable instead of hard-coding it.
            raw_data.Add(0x00); raw_data.Add(0x10);

            UInt16 cmd = (UInt16)Command.DeviceRead;
            UInt16 sub = GetSubcommand(DeviceExt.GetDeviceType(device));

            raw_data.Add((byte)(cmd & 0xff)); raw_data.Add((byte)(cmd >> 0x8));
            raw_data.Add((byte)(sub & 0xff)); raw_data.Add((byte)(sub >> 0x8));
            raw_data.Add((byte)(adr & 0xff)); raw_data.Add((byte)(adr >> 0x8));
            raw_data.Add(0x00);
            raw_data.Add((byte)device);
            raw_data.Add((byte)(cnt & 0xff)); raw_data.Add((byte)(cnt >> 0x8));

            stream.Write(raw_data.ToArray());
        }

        private List<byte> RecvResponse()
        {
            if (stream == null)
                throw new Exception("connection isn't established");

            // read a single byte to determine 
            // if a serial no. is included or not
            var value = stream.ReadByte();
            byte[] hdr_buf;
            switch (value)
            {
                // if value is 0xd0, there's no serial no. included
                // in the response
                case 0xd0:
                    hdr_buf = RecvBytes(8);
                    break;
                // if value is 0xd4, there's a serial no. included
                // in the response
                case 0xd4:
                    hdr_buf = RecvBytes(12);
                    break;
                // in the case where we receive some other data, we mark it
                // as invalid and throw an `Exception`
                default:
                    throw new Exception($"invalid start byte received: {value}");
            }

            int dataSize = hdr_buf[^1] << 8 | hdr_buf[^2];
            List<byte> response_buffer = new();
            response_buffer.AddRange(RecvBytes(dataSize));
            int endCode = response_buffer[1] << 8 | response_buffer[0];

            if (endCode != 0)
                throw new Exception($"non-zero end code: {endCode:X}H");

            response_buffer.RemoveRange(0, 2);
            return response_buffer;
        }
    }
}