﻿using System.Net.Sockets;

namespace SLMP {
    /// <summary>
    /// This class exposes functionality to connect and manage
    /// SLMP-compatible devices.
    /// </summary>
    public partial class SlmpClient {
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

        private SlmpConfig _config;
        private TcpClient _client;
        private NetworkStream? _stream;

        /// <summary>Initializes a new instance of the <see cref="SlmpClient" /> class.</summary>
        /// <param name="cfg">The config.</param>
        public SlmpClient(SlmpConfig cfg) {
            _config = cfg;
            _client = new TcpClient();
        }

        // TODO: reveise `Connect` and `Disconnect` functions
        /// <summary>Connects to the address specified in the config.</summary>
        /// <exception cref="System.TimeoutException">connection timed out</exception>
        public void Connect() {
            _client = new TcpClient();

            if (!_client.ConnectAsync(_config.Address, _config.Port).Wait(_config.ConnTimeout))
                throw new TimeoutException("connection timed out");

            // connection is successful
            _client.SendTimeout = _config.SendTimeout;
            _client.ReceiveTimeout = _config.RecvTimeout;

            _stream = _client.GetStream();
        }

        /// <summary>
        /// Attempt to close the socket connection.
        /// </summary>
        public void Disconnect() {
            if (_stream != null) {
                _stream.Close();
                _stream = null;
            }
            if (_client.Connected)
                _client.Client.Shutdown(SocketShutdown.Both);
            _client.Close();
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
            // TODO: integrate self test into this method
            return _stream != null && _client.Connected;
        }

        private void CheckConnection() {
            if (!Connected())
                throw new NotConnectedException();
        }

        /// <summary>This function exists because `NetworkStream` doesn't have a `recv_exact` method.</summary>
        /// <param name="count">Number of bytes to receive.</param>
        private byte[] ReceiveBytes(int count) {
            CheckConnection();

            int offset = 0, toRead = count;
            int read;
            byte[] buffer = new byte[count];

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            while (toRead > 0 && (read = _stream.Read(buffer, offset, toRead)) > 0) {
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
            int value = _stream.ReadByte();
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
            ushort sub = DeviceMethods.GetSubcommand(device);

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
            _stream.Write(rawRequest.ToArray());
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
            ushort sub = DeviceMethods.GetSubcommand(device);
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
            _stream.Write(rawRequest.ToArray());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        /// <summary>
        /// Sends the `SelfTest` command.
        /// </summary>
        private void SendSelfTestCommand() {
            // We don't check the connection on purpose since
            // this function is meant to be internal and a part of `CheckConnection`.
            // If we do call it, it will result in a stack overflow.
            // CheckConnection();

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
            _stream.Write(rawRequest.ToArray());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}