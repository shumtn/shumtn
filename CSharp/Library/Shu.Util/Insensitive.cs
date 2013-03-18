using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Shu.Util
{
    /// <summary>
    /// 不敏感的字符串大小写比较的类
    /// </summary>
    public static class Insensitive
    {
        #region 静态属性
        /// <summary>
        /// 初始化不敏感的字符串大小写比较的接口
        /// </summary>
        private static IComparer s_Comparer = CaseInsensitiveComparer.Default;
        /// <summary>
        /// 返回不敏感的字符串大小写比较的接口
        /// </summary>
        public static IComparer Comparer
        {
            get { return s_Comparer; }
        }
        #endregion

        #region 静态方法
        /// <summary>
        /// 不敏感的字符串大小写比较
        /// </summary>
        public static int Compare(string strStringA, string strStringB)
        {
            return s_Comparer.Compare(strStringA, strStringB);
        }

        /// <summary>
        /// 不敏感的字符串比较是否相同
        /// </summary>
        public static bool Equals(string strStringA, string strStringB)
        {
            if (strStringA == null && strStringB == null)
                return true;
            else if (strStringA == null || strStringB == null || strStringA.Length != strStringB.Length)
                return false;

            return (s_Comparer.Compare(strStringA, strStringB) == 0);
        }

        /// <summary>
        /// 不敏感的字符串比较第一个字符串前几个字符是否和第二个字符串的完全相同
        /// </summary>
        public static bool StartsWith(string strStringA, string strStringB)
        {
            if (strStringA == null || strStringB == null || strStringA.Length < strStringB.Length)
                return false;

            return (s_Comparer.Compare(strStringA.Substring(0, strStringB.Length), strStringB) == 0);
        }

        /// <summary>
        /// 不敏感的字符串比较第一个字符串后几个字符是否和第二个字符串的完全相同
        /// </summary>
        public static bool EndsWith(string strStringA, string strStringB)
        {
            if (strStringA == null || strStringB == null || strStringA.Length < strStringB.Length)
                return false;

            return (s_Comparer.Compare(strStringA.Substring(strStringA.Length - strStringB.Length), strStringB) == 0);
        }

        /// <summary>
        /// 不敏感的字符串比较第一个字符串是否完全包容第二个字符串
        /// </summary>
        public static bool Contains(string strStringA, string strStringB)
        {
            if (strStringA == null || strStringB == null || strStringA.Length < strStringB.Length)
                return false;

            strStringA = strStringA.ToLower();
            strStringB = strStringB.ToLower();

            return (strStringA.IndexOf(strStringB) >= 0);
        }
        #endregion
    }
}
