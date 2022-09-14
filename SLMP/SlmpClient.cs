﻿using System.Net.Sockets;

namespace SLMP {
    /// <summary>
    /// This class exposes functionality to connect and manage
    /// SLMP-compatible devices.
    /// </summary>
    public class SlmpClient {
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

        private SlmpConfig config;
        private TcpClient client;
        private NetworkStream? stream;

        /// <summary>Initializes a new instance of the <see cref="SlmpClient" /> class.</summary>
        /// <param name="cfg">The config.</param>
        public SlmpClient(SlmpConfig cfg) {
            config = cfg;
            client = new TcpClient();
        }

        /// <summary>Connects to the address specified in the config.</summary>
        /// <exception cref="System.TimeoutException">connection timed out</exception>
        public void Connect() {
            client = new TcpClient();

            if (!client.ConnectAsync(config.Address, config.Port).Wait(config.ConnTimeout))
                throw new TimeoutException("connection timed out");

            // connection is successful
            client.SendTimeout = config.SendTimeout;
            client.ReceiveTimeout = config.RecvTimeout;

            stream = client.GetStream();
        }

        public void Disconnect() {
            if (stream != null) {
                stream.Close();
                stream = null;
            }
            if (client.Connected)
                client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        /// <summary>
        /// Reads a single Bit from a given `BitDevice` and returns a `bool`.
        /// </summary>
        /// <param name="device">The word device.</param>
        /// <param name="addr">Bit address.</param>
        public bool ReadDevice(BitDevice device, ushort addr) {
            return ReadDevice(device, addr, 1)[0];
        }

        /// <summary>
        /// Reads from a given `BitDevice` and returns an array of `bool`s.
        /// Note that there's a limit on how many registers can be read at a time.
        /// </summary>
        /// <param name="device">The bit device.</param>
        /// <param name="addr">Start address.</param>
        /// <param name="count">Number of registers to read.</param>
        public bool[] ReadDevice(BitDevice device, ushort addr, ushort count) {
            SendReadDeviceCommand(device, addr, count);
            List<byte> response = ReceiveResponse();
            List<bool> result = new();

            response.ForEach(delegate (byte a) {
                result.Add((a & 0x10) != 0);
                result.Add((a & 0x01) != 0);
            });

            return result.GetRange(0, count).ToArray();
        }

        /// <summary>
        /// Reads a single Word from a the given `WordDevice` and returns an `ushort`.
        /// </summary>
        /// <param name="device">The word device.</param>
        /// <param name="addr">Word address.</param>
        public ushort ReadDevice(WordDevice device, ushort addr) {
            return ReadDevice(device, addr, 1)[0];
        }

        /// <summary>
        /// Reads from a given `WordDevice` and returns an array of `ushort`s.
        /// Note that there's a limit on how many registers can be read at a time.
        /// </summary>
        /// <param name="device">The word device.</param>
        /// <param name="addr">Start address.</param>
        /// <param name="count">Number of registers to read.</param>
        public ushort[] ReadDevice(WordDevice device, ushort addr, ushort count) {
            SendReadDeviceCommand(device, addr, count);
            List<byte> response = ReceiveResponse();
            List<ushort> result = new();

            // if the length of the response isn't even
            // then the response is invalid and we can't
            // construct an array of `ushort`s from it
            if (response.Count % 2 != 0)
                throw new InvalidDataException("While reading words: data section of the response is uneven");

            // word data is received in little endian format
            // which means the lower byte of a word comes first
            // and upper byte second
            response
                .Chunk(2)
                .ToList()
                .ForEach(n => result.Add((ushort)(n[1] << 8 | n[0])));

            return result.ToArray();
        }

        /// <summary>
        /// Writes a single `Bit` to a given `BitDevice`.
        /// </summary>
        /// <param name="device">The WordDevice to write.</param>
        /// <param name="addr">Address.</param>
        /// <param name="data">Data to be written into the remote device.</param>
        public void WriteDevice(BitDevice device, ushort addr, bool data) {
            WriteDevice(device, addr, new bool[] { data });
        }

        /// <summary>
        /// Writes an array of `bool`s to a given `BitDevice`.
        /// Note that there's a limit on how many registers can be written at a time.
        /// </summary>
        /// <param name="device">The BitDevice to write.</param>
        /// <param name="addr">Starting address.</param>
        /// <param name="data">Data to be written into the remote device.</param>
        public void WriteDevice(BitDevice device, ushort addr, bool[] data) {
            ushort count = (ushort)data.Length;
            List<bool> listData = data.ToList();
            List<byte> encodedData = new();

            // If the length of `data` isn't even, add a dummy
            // `false` to make the encoding easier. It gets ignored on the station side.
            if (count % 2 != 0)
                listData.Add(false);

            listData
                .Chunk(2)
                .ToList()
                .ForEach(a => encodedData.Add(
                    (byte)(Convert.ToByte(a[0]) << 4 | Convert.ToByte(a[1]))));

            SendWriteDeviceCommand(device, addr, count, encodedData.ToArray());
            ReceiveResponse();
        }

        /// <summary>
        /// Writes a single `ushort` to a given `WordDevice`.
        /// </summary>
        /// <param name="device">The WordDevice to write.</param>
        /// <param name="addr">Address.</param>
        /// <param name="data">Data to be written into the remote device.</param>
        public void WriteDevice(WordDevice device, ushort addr, ushort data) {
            WriteDevice(device, addr, new ushort[] { data });
        }

        /// <summary>
        /// Writes an array of `ushort`s to a given `WordDevice`.
        /// Note that there's a limit on how many registers can be written at a time.
        /// </summary>
        /// <param name="device">The WordDevice to write.</param>
        /// <param name="addr">Starting address.</param>
        /// <param name="data">Data to be written into the remote device.</param>
        public void WriteDevice(WordDevice device, ushort addr, ushort[] data) {
            ushort count = (ushort)data.Length;
            List<byte> encodedData = new();

            foreach (ushort word in data) {
                encodedData.Add((byte)(word & 0xff));
                encodedData.Add((byte)(word >> 0x8));
            }

            SendWriteDeviceCommand(device, addr, count, encodedData.ToArray());
            ReceiveResponse();
        }

        /// <summary>
        /// Writes the given string to the specified device as a null terminated string.
        /// Note that there's a limit on how many registers can be written at a time.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="addr">Starting address.</param>
        /// <param name="text">The string to write.</param>
        public void WriteString(WordDevice device, ushort addr, string text) {
            // add proper padding to the string
            text += new string('\0', 2 - (text.Length % 2));
            List<ushort> result = new();

            System.Text.Encoding.ASCII.GetBytes(text.ToCharArray())
                .Chunk(2)
                .ToList()
                .ForEach(a => result.Add((ushort)(a[1] << 8 | a[0])));

            WriteDevice(device, addr, result.ToArray());
        }

        /// <summary>
        /// Reads a string with the length `len` from the specified `WordDevice`. Note that
        /// this function reads the string at best two chars, ~500 times in a second.
        /// Meaning it can only read ~1000 chars per second.
        /// Note that there's a limit on how many registers can be read at a time.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="addr">Starting address of the null terminated string.</param>
        /// <param name="len">Length of the string.</param>
        public string ReadString(WordDevice device, ushort addr, ushort len) {
            ushort wordCount = (ushort)((len % 2 == 0 ? len : len + 1) / 2);
            List<char> buffer = new();

            foreach (ushort word in ReadDevice(device, addr, wordCount)) {
                buffer.Add((char)(word & 0xff));
                buffer.Add((char)(word >> 0x8));
            }

            return string.Join("", buffer.GetRange(0, len));
        }

        /// <summary>
        /// Read from a `WordDevice` to create a C# structure.
        /// The target structure can only contain very primitive data types.
        /// Supported data types:
        ///    bool: 2 bytes, 0 for `False` anything else for `True`
        ///  ushort: 2 bytes (UInt16)
        ///   short: 2 bytes (Int16)
        ///    uint: 4 bytes (UInt32)
        ///     int: 4 bytes (Int32)
        ///  string: arbitrary long, must have an `SLMPStringAttribute`
        /// </summary>
        /// <typeparam name="T">The `Struct` to read.</typeparam>
        /// <param name="device">The device to read from..</param>
        /// <param name="addr">Starting address of the structure data.</param>
        public T? ReadStruct<T>(WordDevice device, ushort addr) where T : struct {
            Type structType = typeof(T);
            ushort[] words = ReadDevice(
                device, addr, (ushort)Struct.GetStructSize(structType));

            return Struct.FromBytes(structType, words) as T?;
        }

        public bool SelfTest() {
            try {
                SendSelfTestCommand();
                List<byte> response = ReceiveResponse();

                return response.Count == 6 &&
                       response.SequenceEqual(new byte[] { 0x04, 0x00, 0xde, 0xad, 0xbe, 0xef });
            } catch {
                return false;
            }
        }

        /// <summary>
        /// Query the connection status.
        /// </summary>
        public bool Connected() {
            return stream != null && client.Connected;
        }

        private void CheckConnection() {
            if (!Connected())
                throw new NotConnectedException();
        }

        /// <summary>
        /// Gets the subcommand for a given `(Bit/Word)Device`.
        /// </summary>
        /// <exception cref="System.ArgumentException">invalid device type provided</exception>
        private static ushort GetSubcommand(dynamic type) {
            return type switch {
                BitDevice => 0x0001,
                WordDevice => 0x0000,
                _ => throw new ArgumentException("invalid device type provided"),
            };
        }

        /// <summary>This function exists because `NetworkStream` doesn't have a `recv_exact` method.</summary>
        /// <param name="count">Number of bytes to receive.</param>
        private byte[] ReceiveBytes(int count) {
            CheckConnection();

            int offset = 0, toRead = count;
            int read;
            byte[] buffer = new byte[count];

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            while (toRead > 0 && (read = stream.Read(buffer, offset, toRead)) > 0) {
                toRead -= read;
                offset += read;
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (toRead > 0) throw new EndOfStreamException();

            return buffer;
        }

        /// <summary>Receives the response and returns the raw response data.</summary>
        /// <returns>Raw response data</returns>
        private List<byte> ReceiveResponse() {
            CheckConnection();

            // read a single byte to determine
            // if a serial no. is included or not
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            int value = stream.ReadByte();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            byte[] hdrBuf;
            switch (value) {
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
                    throw new InvalidDataException($"while reading respoonse header: invalid start byte received: {value}");
            }

            // calculate the response data length
            int dataSize = hdrBuf[^1] << 8 | hdrBuf[^2];
            List<byte> responseBuffer = ReceiveBytes(dataSize).ToList();

            // if the encode isn't `0` then we know that we hit an error.
            int endCode = responseBuffer[1] << 8 | responseBuffer[0];
            if (endCode != 0)
                throw new SLMPException(endCode);

            responseBuffer.RemoveRange(0, 2);
            return responseBuffer;
        }

        /// <summary>Sends the read device command.</summary>
        /// <param name="device">The target device.</param>
        /// <param name="adr">The address</param>
        /// <param name="cnt">The count.</param>
        private void SendReadDeviceCommand(dynamic device, ushort adr, ushort cnt) {
            CheckConnection();

            List<byte> rawRequest = HEADER.ToList();

            ushort cmd = (ushort)Command.DeviceRead;
            ushort sub = GetSubcommand(device);

            rawRequest.AddRange(new byte[]{
                // request data length (in terms of bytes): fixed size (12) for the read command
                0x0c, 0x00,
                // monitoring timer. TODO: make this something configurable instead of hard-coding it.
                0x00, 0x00,
                (byte)(cmd & 0xff), (byte)(cmd >> 0x8),
                (byte)(sub & 0xff), (byte)(sub >> 0x8),
                (byte)(adr & 0xff), (byte)(adr >> 0x8),
                0x00,
                (byte)device,
                (byte)(cnt & 0xff), (byte)(cnt >> 0x8),
            });

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            stream.Write(rawRequest.ToArray());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        /// <summary>
        /// Sends the write device command.
        /// </summary>
        /// <param name="device">The target device</param>
        /// <param name="adr">The address.</param>
        /// <param name="cnt">Number of data points.</param>
        /// <param name="data">Data itself.</param>
        private void SendWriteDeviceCommand(dynamic device, ushort adr, ushort cnt, byte[] data) {
            CheckConnection();

            List<byte> rawRequest = HEADER.ToList();

            ushort cmd = (ushort)Command.DeviceWrite;
            ushort sub = GetSubcommand(device);
            ushort len = (ushort)(data.Length + 0x000c);

            rawRequest.AddRange(new byte[]{
                // request data length (in terms of bytes): (12 + data.Length)
                (byte)(len & 0xff), (byte)(len >> 0x8),
                // monitoring timer. TODO: make this something configurable instead of hard-coding it.
                0x00, 0x00,
                (byte)(cmd & 0xff), (byte)(cmd >> 0x8),
                (byte)(sub & 0xff), (byte)(sub >> 0x8),
                (byte)(adr & 0xff), (byte)(adr >> 0x8),
                0x00,
                (byte)device,
                (byte)(cnt & 0xff), (byte)(cnt >> 0x8),
            });
            rawRequest.AddRange(data);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            stream.Write(rawRequest.ToArray());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        private void SendSelfTestCommand() {
            List<byte> rawRequest = HEADER.ToList();
            ushort cmd = (ushort)Command.SelfTest;
            ushort sub = 0x0000;

            rawRequest.AddRange(new byte[]{
                // request data length (in terms of bytes): fixed size (12) for the read command
                0x0c, 0x00,
                // monitoring timer. TODO: make this something configurable instead of hard-coding it.
                0x00, 0x00,
                (byte)(cmd & 0xff), (byte)(cmd >> 0x8),
                (byte)(sub & 0xff), (byte)(sub >> 0x8),
                0x04, 0x00,
                0xde, 0xad, 0xbe, 0xef
            });

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            stream.Write(rawRequest.ToArray());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}