using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Shu.Util
{
    public class Aes
    {
        #region Aes属性
        private static string _getRandom = null;
        public static string GetRandom
        {
            set { _getRandom = value; }
            get { return _getRandom; }
        }

        public static string Key(string strKey)
        {
            byte[] key = new byte[32];

            key = Encoding.UTF8.GetBytes(strKey);

            return Encoding.UTF8.GetString(key);
        }
        #endregion

        #region Byte加密
        /// <summary>
        /// Byte 加密
        /// </summary>
        /// <param name="toEncrypt">被加密的 明文 Byte</param>
        /// <param name="strKey">钥匙</param>
        /// <returns>返回 byte[]</returns>
        public static byte[] Encrypt(byte[] toEncrypt, string strKey)
        {
            Random r = new Random();
            byte[] b = new byte[4];
            r.NextBytes(b);
            if (string.IsNullOrEmpty(_getRandom))
            {
                _getRandom = BitConverter.ToString(b).Replace("-", null);
            }

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Key(strKey + _getRandom));
            byte[] toEncryptArray = toEncrypt;

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return resultArray;
        }

        /// <summary>
        /// Byte 解密
        /// </summary>
        /// <param name="toDecrypt">被解密的 密文</param>
        /// <param name="strKey">钥匙</param>
        /// <returns>返回 byte[]</returns>
        public static byte[] Decrypt(byte[] toDecrypt, string strKey)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Key(strKey + _getRandom));
            byte[] toEncryptArray = toDecrypt; //Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return resultArray;
        }
        #endregion

        #region String加密
        /// <summary>
        /// String 加密
        /// </summary>
        /// <param name="toEncrypt">被加密的 明文 String</param>
        /// <param name="strKey">钥匙</param>
        /// <returns>返回 String</returns>
        public static string Encrypt(string toEncrypt, string strKey)
        {
            Random r = new Random();
            byte[] b = new byte[4];
            r.NextBytes(b);
            if (string.IsNullOrEmpty(_getRandom))
            {
                _getRandom = BitConverter.ToString(b).Replace("-", null);
            }

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Key(strKey + _getRandom));
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            
            string encrypt = Convert.ToBase64String(resultArray, 0, resultArray.Length);

            if (string.IsNullOrEmpty(encrypt))
            {
                return null;
            }
            else
            {
                return encrypt;
            }
        }

        /// <summary>
        /// String 解密
        /// </summary>
        /// <param name="toDecrypt">被解密的 密文</param>
        /// <param name="strKey">钥匙</param>
        /// <returns>返回 String</returns>
        public static string Decrypt(string toDecrypt, string strKey)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Key(strKey + _getRandom));
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            string decrypt = UTF8Encoding.UTF8.GetString(resultArray);

            if (string.IsNullOrEmpty(decrypt))
            {
                return null;
            }
            else
            {
                return decrypt;
            }
        }
        #endregion

        public static string Decode(string JiePwd)
        {
            string passPhrase = "戴力你永远是最棒的哦！";
            string saltValue = "www.wdd.cn";
            string hashAlgorithm = "SHA512";
            int passwordIterations = 2;
            string initVector = "dlnyyszbdo!@#$%^";
            int keySize = 0x100;
            return Decode(JiePwd, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        }

        public static string Decode(string JieTxt, string passPhrase)
        {
            string saltValue = "www.wdd.cn";
            string hashAlgorithm = "SHA512";
            int passwordIterations = 2;
            string initVector = "dlnyyszbdo!@#$%^";
            int keySize = 0x100;
            return Decode(JieTxt, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        }

        public static string Decode(string JieTxt, string passPhrase, string saltValue)
        {
            string hashAlgorithm = "SHA512";
            int passwordIterations = 2;
            string initVector = "dlnyyszbdo!@#$%^";
            int keySize = 0x100;
            return Decode(JieTxt, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        }

        private static string Decode(string cipherText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(initVector);
                byte[] rgbSalt = Encoding.ASCII.GetBytes(saltValue);
                byte[] buffer = Convert.FromBase64String(cipherText);
                byte[] rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
                RijndaelManaged managed = new RijndaelManaged();
                managed.Mode = CipherMode.CBC;
                ICryptoTransform transform = managed.CreateDecryptor(rgbKey, bytes);
                MemoryStream stream = new MemoryStream(buffer);
                CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
                byte[] buffer5 = new byte[buffer.Length];
                int count = stream2.Read(buffer5, 0, buffer5.Length);
                stream.Close();
                stream2.Close();
                return Encoding.UTF8.GetString(buffer5, 0, count);
            }
            catch
            {
                return null;
            }
        }

        public static string Encode(string JiaTxt)
        {
            string passPhrase = "戴力你永远是最棒的哦！";
            string saltValue = "www.wdd.cn";
            string hashAlgorithm = "SHA512";
            int passwordIterations = 2;
            string initVector = "dlnyyszbdo!@#$%^";
            int keySize = 0x100;
            return Encode(JiaTxt, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        }

        public static string Encode(string JiaTxt, string passPhrase)
        {
            string saltValue = "www.wdd.cn";
            string hashAlgorithm = "SHA512";
            int passwordIterations = 2;
            string initVector = "dlnyyszbdo!@#$%^";
            int keySize = 0x100;
            return Encode(JiaTxt, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        }

        public static string Encode(string JiaTxt, string passPhrase, string saltValue)
        {
            string hashAlgorithm = "SHA512";
            int passwordIterations = 2;
            string initVector = "dlnyyszbdo!@#$%^";
            int keySize = 0x100;
            return Encode(JiaTxt, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        }

        private static string Encode(string plainText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(initVector);
                byte[] rgbSalt = Encoding.ASCII.GetBytes(saltValue);
                byte[] buffer = Encoding.UTF8.GetBytes(plainText);
                byte[] rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
                RijndaelManaged managed = new RijndaelManaged();
                managed.Mode = CipherMode.CBC;
                ICryptoTransform transform = managed.CreateEncryptor(rgbKey, bytes);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                byte[] inArray = stream.ToArray();
                stream.Close();
                stream2.Close();
                return Convert.ToBase64String(inArray);
            }
            catch
            {
                return null;
            }
        }
    }
}