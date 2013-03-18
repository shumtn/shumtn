using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Shu.Util
{
    public class Sha
    {
        public static class Sha256
        {
            private static byte[] result = null;

            public static string GetHex(string stringData)
            {
                SHA256 shaM = new SHA256Managed();
                result = shaM.ComputeHash(Encoding.UTF8.GetBytes(stringData));
                return BitConverter.ToString(result, 0).Replace("-", null);
            }

            public static string GetHex(byte[] socketData)
            {
                SHA256 shaM = new SHA256Managed();
                result = shaM.ComputeHash(socketData);
                return BitConverter.ToString(result, 0).Replace("-", null);
            }

            public static string GetB64(string stringData)
            {
                SHA256 shaM = new SHA256Managed();
                result = shaM.ComputeHash(Encoding.UTF8.GetBytes(stringData));
                return Convert.ToBase64String(result);
            }

            public static string GetB64(byte[] socketData)
            {
                SHA256 shaM = new SHA256Managed();
                result = shaM.ComputeHash(socketData);
                return Convert.ToBase64String(result);
            }
        }
    }
}
