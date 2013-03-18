using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Shu.Util
{
    public class Hardware
    {
        /// <summary>
        /// Cpu信息
        /// </summary>
        public class Cpu
        {

        }

        /// <summary>
        /// 硬盘信息
        /// </summary>
        public class Hard
        {

        }

        /// <summary>
        /// 网卡信息
        /// </summary>
        public class Nic
        {
            #region Ip
            /// <summary>
            /// Ip 信息
            /// </summary>
            public class Ip
            {
                #region 变量
                private static string _dataPath = null;
                private static string _ip = "0.0.0.0";
                private static string country;
                private static string local;
                private static long firstStartIp = 0;
                private static long lastStartIp = 0;
                private static FileStream objfs = null;
                private static long startIp = 0;
                private static long endIp = 0;
                private static int countryFlag = 0;
                private static long endIpOff = 0;
                private static string errMsg = null;
                #endregion

                public Ip(string ipdata)
                {
                    _dataPath = ipdata;
                }

                #region 属性
                //public static string DataPath
                //{
                //    set { _dataPath = value; }
                //}

                //public string IP
                //{
                //    set { _ip = value; }
                //}

                /// <summary>
                /// IP地址位置
                /// </summary>
                public static string Country
                {
                    get { return country; }
                }

                /// <summary>
                /// 上网方式
                /// </summary>
                public static string Local
                {
                    get { return local; }
                }

                public static string ErrMsg
                {
                    get { return errMsg; }
                }

                //public static FileStream IpFileStream
                //{
                //    set { objfs = value; }
                //}

                /// <summary>
                /// 获取IP地址
                /// </summary>
                public static string IpAdds
                {
                    get
                    {
                        return System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[1].ToString();
                    }
                }
                #endregion 

                #region 搜索匹配数据
                /// <summary>
                /// 操作 QQWry 搜索匹配数据
                /// </summary>
                /// <returns>int</returns>
                private static int QQwry()
                {
                    string pattern = @"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))";
                    Regex objRe = new Regex(pattern);
                    Match objMa = objRe.Match(_ip);
                    if (!objMa.Success)
                    {
                        errMsg = "IP格式错误";
                        return 4;
                    }

                    long ip_Int = IpToInt(_ip);
                    int nRet = 0;
                    if (ip_Int >= IpToInt("127.0.0.0") && ip_Int <= IpToInt("127.255.255.255"))
                    {
                        country = "本机内部环回地址";
                        local = "";
                        nRet = 1;
                    }
                    else if ((ip_Int >= IpToInt("0.0.0.0") && ip_Int <= IpToInt("2.255.255.255")) || (ip_Int >= IpToInt("64.0.0.0") && ip_Int <= IpToInt("126.255.255.255")) || (ip_Int >= IpToInt("58.0.0.0") && ip_Int <= IpToInt("60.255.255.255")))
                    {
                        country = "网络保留地址";
                        local = "";
                        nRet = 1;
                    }

                    try
                    {
                        objfs = new FileStream(_dataPath, FileMode.Open, FileAccess.Read);
                    }
                    catch
                    {
                    }

                    try
                    {
                        //objfs.Seek(0,SeekOrigin.Begin);
                        objfs.Position = 0;
                        byte[] buff = new Byte[8];
                        objfs.Read(buff, 0, 8);
                        firstStartIp = buff[0] + buff[1] * 256 + buff[2] * 256 * 256 + buff[3] * 256 * 256 * 256;
                        lastStartIp = buff[4] * 1 + buff[5] * 256 + buff[6] * 256 * 256 + buff[7] * 256 * 256 * 256;
                        long recordCount = Convert.ToInt64((lastStartIp - firstStartIp) / 7.0);
                        if (recordCount <= 1)
                        {
                            country = "FileDataError";
                            objfs.Close();
                            return 2;
                        }
                        long rangE = recordCount;
                        long rangB = 0;
                        long recNO = 0;
                        while (rangB < rangE - 1)
                        {
                            recNO = (rangE + rangB) / 2;
                            GetStartIp(recNO);
                            if (ip_Int == startIp)
                            {
                                rangB = recNO;
                                break;
                            }
                            if (ip_Int > startIp)
                                rangB = recNO;
                            else
                                rangE = recNO;
                        }
                        GetStartIp(rangB);
                        GetEndIp();
                        if (startIp <= ip_Int && endIp >= ip_Int)
                        {
                            GetCountry();
                            local = local.Replace("（我们一定要解放台湾！！！）", "");
                        }
                        else
                        {
                            nRet = 3;
                            country = "未知";
                            local = "";
                        }
                        objfs.Close();
                        return nRet;
                    }
                    catch
                    {
                        return 1;
                    }
                }
                #endregion

                #region IP地址转换成Int数据
                /// <summary>
                /// IP地址转换成Int数据
                /// </summary>
                /// <param name="ip">IP地址</param>
                /// <returns>long</returns>
                public static long IpToInt(string ip)
                {
                    char[] dot = new char[] { '.' };
                    string[] ipArr = ip.Split(dot);
                    if (ipArr.Length == 3)
                    {
                        ip = ip + ".0";
                    }
                    ipArr = ip.Split(dot);

                    long ip_Int = 0;
                    long p1 = long.Parse(ipArr[0]) * 256 * 256 * 256;
                    long p2 = long.Parse(ipArr[1]) * 256 * 256;
                    long p3 = long.Parse(ipArr[2]) * 256;
                    long p4 = long.Parse(ipArr[3]);
                    ip_Int = p1 + p2 + p3 + p4;
                    return ip_Int;
                }

                #endregion

                #region int转换成IP
                /// <summary>
                /// int转换成IP
                /// </summary>
                /// <param name="ip_Int">IP的long数据</param>
                /// <returns>string</returns>
                public static string IntToIp(long ip_Int)
                {
                    long seg1 = (ip_Int & 0xff000000) >> 24;
                    if (seg1 < 0)
                    {
                        seg1 += 0x100;
                    }

                    long seg2 = (ip_Int & 0x00ff0000) >> 16;
                    if (seg2 < 0)
                    {
                        seg2 += 0x100;
                    }

                    long seg3 = (ip_Int & 0x0000ff00) >> 8;
                    if (seg3 < 0)
                    {
                        seg3 += 0x100;
                    }

                    long seg4 = (ip_Int & 0x000000ff);
                    if (seg4 < 0)
                    {
                        seg4 += 0x100;
                    }

                    string ip = seg1.ToString() + "." + seg2.ToString() + "." + seg3.ToString() + "." + seg4.ToString();

                    return ip;
                }
                #endregion

                #region 获取起始IP范围
                /// <summary>
                /// 获取起始IP范围
                /// </summary>
                /// <param name="recNO"></param>
                /// <returns></returns>
                private static long GetStartIp(long recNO)
                {
                    long offSet = firstStartIp + recNO * 7;
                    //objfs.Seek(offSet,SeekOrigin.Begin);
                    objfs.Position = offSet;
                    byte[] buff = new Byte[7];
                    objfs.Read(buff, 0, 7);

                    endIpOff = Convert.ToInt64(buff[4].ToString()) + Convert.ToInt64(buff[5].ToString()) * 256 + Convert.ToInt64(buff[6].ToString()) * 256 * 256;
                    startIp = Convert.ToInt64(buff[0].ToString()) + Convert.ToInt64(buff[1].ToString()) * 256 + Convert.ToInt64(buff[2].ToString()) * 256 * 256 + Convert.ToInt64(buff[3].ToString()) * 256 * 256 * 256;
                    return startIp;
                }
                #endregion

                #region 获取结束IP
                /// <summary>
                /// 获取结束IP
                /// </summary>
                /// <returns>long</returns>
                private static long GetEndIp()
                {
                    //objfs.Seek(endIpOff,SeekOrigin.Begin);
                    objfs.Position = endIpOff;
                    byte[] buff = new Byte[5];
                    objfs.Read(buff, 0, 5);
                    endIp = Convert.ToInt64(buff[0].ToString()) + Convert.ToInt64(buff[1].ToString()) * 256 + Convert.ToInt64(buff[2].ToString()) * 256 * 256 + Convert.ToInt64(buff[3].ToString()) * 256 * 256 * 256;
                    countryFlag = buff[4];
                    return endIp;
                }
                #endregion

                #region 获取国家/区域偏移量
                /// <summary>
                /// 获取国家/区域偏移量
                /// </summary>
                /// <returns>string</returns>
                private static string GetCountry()
                {
                    switch (countryFlag)
                    {
                        case 1:
                        case 2:
                            country = GetFlagStr(endIpOff + 4);
                            local = (1 == countryFlag) ? " " : GetFlagStr(endIpOff + 8);
                            break;
                        default:
                            country = GetFlagStr(endIpOff + 4);
                            local = GetFlagStr(objfs.Position);
                            break;
                    }
                    return " ";
                }
                #endregion

                #region 获取国家/区域字符串
                /// <summary>
                /// 获取国家/区域字符串
                /// </summary>
                /// <param name="offSet"></param>
                /// <returns></returns>
                private static string GetFlagStr(long offSet)
                {
                    int flag = 0;
                    byte[] buff = new Byte[3];
                    while (1 == 1)
                    {
                        //objfs.Seek(offSet,SeekOrigin.Begin);
                        objfs.Position = offSet;
                        flag = objfs.ReadByte();
                        if (flag == 1 || flag == 2)
                        {
                            objfs.Read(buff, 0, 3);
                            if (flag == 2)
                            {
                                countryFlag = 2;
                                endIpOff = offSet - 4;
                            }
                            offSet = Convert.ToInt64(buff[0].ToString()) + Convert.ToInt64(buff[1].ToString()) * 256 + Convert.ToInt64(buff[2].ToString()) * 256 * 256;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (offSet < 12)
                    {
                        return " ";
                    }
                    objfs.Position = offSet;
                    return GetStr();
                }
                #endregion

                #region GetStr
                /// <summary>
                /// 获取字符串
                /// </summary>
                /// <returns>string</returns>
                private static string GetStr()
                {
                    byte lowC = 0;
                    byte upC = 0;
                    string str = "";
                    byte[] buff = new byte[2];
                    while (1 == 1)
                    {
                        lowC = (Byte)objfs.ReadByte();
                        if (lowC == 0)
                        {
                            break;
                        }
                        if (lowC > 127)
                        {
                            upC = (byte)objfs.ReadByte();
                            buff[0] = lowC;
                            buff[1] = upC;
                            Encoding enc = Encoding.GetEncoding("GB2312");
                            if (upC == 0)
                            {
                                break;
                            }
                            str += enc.GetString(buff);
                        }
                        else
                        {
                            str += (char)lowC;
                        }
                    }
                    return str;
                }
                #endregion

                #region 获取IP地址
                /// <summary>
                /// 返回真实地理位置
                /// </summary>
                /// <returns></returns>
                public static string IpLoca()
                {
                    QQwry();
                    string localtion = country + local;
                    localtion = localtion.Replace("CZ88.NET", "");
                    return localtion;
                }

                /// <summary>
                /// 根据IP地址返回真实位置
                /// </summary>
                /// <param name="ip">要查询的IP地址</param>
                /// <returns>返回IP真实地理位置</returns>
                public static string IpLoca(string ip)
                {
                    _ip = ip;
                    QQwry();
                    string localtion = country + local;
                    localtion = localtion.Replace("CZ88.NET", "");
                    return localtion;
                }
                /// <summary>
                /// 根据IP地址返回真实位置
                /// </summary>
                /// <param name="dataPath">数据库位置</param>
                /// <param name="ip">IP地址</param>
                /// <returns></returns>
                public static string IpLoca(string dataPath, string ip)
                {
                    _dataPath = dataPath;
                    _ip = ip;
                    QQwry();
                    return country + local;
                }
                #endregion
            }
            #endregion

            #region Mac
            [DllImport("Iphlpapi.dll")]
            private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
            [DllImport("Ws2_32.dll")]
            private static extern Int32 inet_addr(string ip);

            /// <summary>
            /// 获取网卡物理地址
            /// </summary>
            /// <param name="Ip">目标IP地址</param>
            /// <returns></returns>
            public static string Mac(string Ip)
            {
                try
                {
                    Int32 ldest = inet_addr(Ip);
                    Int32 lhost = inet_addr("");
                    Int64 macinfo = new Int64();
                    Int32 len = 6;

                    int res = SendARP(ldest, 0, ref macinfo, ref len);

                    string mac_src = macinfo.ToString("X");

                    string mac_dest = "";

                    while (mac_src.Length < 12)
                    {
                        mac_src = mac_src.Insert(0, "0");
                    }

                    for (int i = 0; i < 11; i++)
                    {
                        if (0 == (i % 2))
                        {
                            if (i == 10)
                            {
                                mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2));
                            }
                            else
                            {
                                mac_dest = "-" + mac_dest.Insert(0, mac_src.Substring(i, 2));
                            }
                        }
                    }
                    return mac_dest;
                }
                catch //(Exception err)
                {
                    return null;
                }
            }
            #endregion
        }
    }
}