using System;

namespace Shu.Util
{
    /// <summary>
    /// TryParse辅助类
    /// </summary>
    public static class TryParse
    {
        /// <summary>
        /// 枚举的TryParse
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strType"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool EnumTryParse<T>(string strType, out T result)
        {
            if (Enum.IsDefined(typeof(T), strType))
            {
                result = (T)Enum.Parse(typeof(T), strType, true);
                return true;
            }
            else
            {
                foreach (string value in Enum.GetNames(typeof(T)))
                {
                    if (value.Equals(strType, StringComparison.OrdinalIgnoreCase))
                    {
                        result = (T)Enum.Parse(typeof(T), value);
                        return true;
                    }
                }
                result = default(T);
                return false;
            }
        }
    }
}

