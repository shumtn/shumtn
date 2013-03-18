using System;
using System.IO;
using System.Text;
using System.Net;

namespace Shu.Util
{
    public static class Utility
    {
        public static void FormatBuffer(TextWriter output, Stream input, long length)
        {
            output.WriteLine("        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F");
            output.WriteLine("       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --");

            long byteIndex = 0;

            long whole = length >> 4;
            long rem = length & 0xF;

            for (int i = 0; i < whole; ++i, byteIndex += 16)
            {
                StringBuilder bytes = new StringBuilder(49);
                StringBuilder chars = new StringBuilder(16);

                for (int j = 0; j < 16; ++j)
                {
                    int c = input.ReadByte();

                    bytes.Append(c.ToString("X2"));

                    if (j != 7)
                    {
                        bytes.Append(' ');
                    }
                    else
                    {
                        bytes.Append("  ");
                    }

                    if (c >= 0x20 && c < 0x80)
                    {
                        chars.Append((char)c);
                    }
                    else
                    {
                        chars.Append('.');
                    }
                }

                output.Write(byteIndex.ToString("X4"));
                output.Write("   ");
                output.Write(bytes.ToString());
                output.Write("  ");
                output.WriteLine(chars.ToString());
            }

            if (rem != 0)
            {
                StringBuilder bytes = new StringBuilder(49);
                StringBuilder chars = new StringBuilder((int)rem);

                for (int j = 0; j < 16; ++j)
                {
                    if (j < rem)
                    {
                        int c = input.ReadByte();

                        bytes.Append(c.ToString("X2"));

                        if (j != 7)
                        {
                            bytes.Append(' ');
                        }
                        else
                        {
                            bytes.Append("  ");
                        }

                        if (c >= 0x20 && c < 0x80)
                        {
                            chars.Append((char)c);
                        }
                        else
                        {
                            chars.Append('.');
                        }
                    }
                    else
                    {
                        bytes.Append("   ");
                    }
                }

                output.Write(byteIndex.ToString("X4"));
                output.Write("   ");
                output.Write(bytes.ToString());
                output.Write("  ");
                output.WriteLine(chars.ToString());
            }
        }

        public static long FormatEndPoint(ref string ip, ref string port, byte[] address)
        {
            long rlong = 0;
            if (address.Length == 6)
            {
                byte[] bip = new byte[4];
                Buffer.BlockCopy(address, 0, bip, 0, 4);
                IPAddress ipaddress = new IPAddress(bip);
                ip = ipaddress.ToString();
                port = BitConverter.ToUInt16(address, 4).ToString();
                rlong = (long)address[0] << 40 | (long)address[1] << 32 | (long)address[2] << 24 | (long)address[3] << 16 | (long)address[4] << 8 | (long)address[5];
            }
            else
            {
                ip = "";
                port = "";
            }

            return rlong;
        }

        public static IPEndPoint GetIPAddress(string ip, string port)
        {
            IPAddress iIPAddress;
            if (ip == "0" && ip == string.Empty && ip == "")
            {
                iIPAddress = IPAddress.Parse("127.0.0.1");
            }
            else
            {
                if (!IPAddress.TryParse(ip, out iIPAddress))
                {
                    //LOGs.WriteLine(LogMessageType.MSG_ERROR, "连接IP地址无效,已默认为连接127.0.0.1!");
                    iIPAddress = IPAddress.Parse("127.0.0.1");
                }
            }
            int iPort;
            if (!int.TryParse(port, out iPort))
            {
                //LOGs.WriteLine(LogMessageType.MSG_ERROR, "连接端口无效,已默认为连接8001端口!");
                iPort = 6000;
            }
            return new IPEndPoint(iIPAddress, iPort);
        }
    }
}