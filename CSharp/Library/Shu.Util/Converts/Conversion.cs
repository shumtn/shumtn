using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Data;
using System.Reflection;
using System.Data.Common;
using System.Dynamic;

namespace Shu.Util
{
    /// <summary>
    /// 转换函数
    /// </summary>
    public class Conversion
    {
        public static dynamic DataFillDynamic(IDataReader reader)
        {
            dynamic d = new ExpandoObject();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                try
                {
                    ((IDictionary<string, object>)d).Add(reader.GetName(i), reader.GetValue(i));
                }
                catch
                {
                    ((IDictionary<string, object>)d).Add(reader.GetName(i), null);
                }
            }
            return d;
        } 


        public static void ReaderToModel(IDataReader reader, object Model)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                System.Reflection.PropertyInfo propertyInfo = Model.GetType().GetProperty(reader.GetName(i));
                if (propertyInfo != null)
                {
                    if (reader.GetValue(i) != DBNull.Value)
                    {
                        if (propertyInfo.PropertyType.IsEnum)
                        {
                            propertyInfo.SetValue(Model, Enum.ToObject(propertyInfo.PropertyType, reader.GetValue(i)), null);
                        }
                        else
                        {
                            propertyInfo.SetValue(Model, reader.GetValue(i), null);
                        }
                    }
                }
            }
            
        }

        public static T ReaderToModel<T>(IDataReader dr)
        {
            try
            {
                using (dr)
                {
                    if (dr.Read())
                    {
                        List<string> list = new List<string>(dr.FieldCount);
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            list.Add(dr.GetName(i).ToLower());
                        }
                        T model = Activator.CreateInstance<T>();
                        foreach (PropertyInfo pi in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                        {
                            if (list.Contains(pi.Name.ToLower()))
                            {
                                if (!IsNullOrDBNull(dr[pi.Name]))
                                {
                                    pi.SetValue(model, HackType(dr[pi.Name], pi.PropertyType), null);
                                }
                            }
                        }
                        return model;
                    }
                }
                return default(T);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<T> ReaderToList<T>(IDataReader dr)
        {
            using (dr)
            {
                List<string> field = new List<string>(dr.FieldCount);
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    field.Add(dr.GetName(i).ToLower());
                }
                List<T> list = new List<T>();
                while (dr.Read())
                {
                    T model = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (field.Contains(property.Name.ToLower()))
                        {
                            if (!IsNullOrDBNull(dr[property.Name]))
                            {
                                property.SetValue(model, HackType(dr[property.Name], property.PropertyType), null);
                            }
                        }
                    }
                    list.Add(model);
                }
                return list;
            }
        }
        //这个类对可空类型进行判断转换，要不然会报错
        private static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;

                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }

        private static bool IsNullOrDBNull(object obj)
        {
            return ((obj is DBNull) || string.IsNullOrEmpty(obj.ToString())) ? true : false;
        }



        #region 日期转换
        /// <summary>
        /// 日期转换
        /// </summary>
        public static class DataTime
        {
            /// <summary>
            /// 检查是否是年份 否则 只取年份
            /// </summary>
            /// <param name="datetime">被检查时间</param>
            /// <param name="isnullvalue">检查失败</param>
            /// <returns></returns>
            public static string IsYear(string datetime, string isnullvalue)
            {
                try
                {
                    return Convert.ToDateTime(datetime).ToString("yyyy").ToString();
                }
                catch
                {
                    return isnullvalue;
                }
            }

            /// <summary>
            /// 获取年份
            /// </summary>
            /// <param name="datetime">被检查时间</param>
            /// <returns></returns>
            public static string GetYear(string datetime)
            {
                try
                {
                    return datetime.Remove(5);
                }
                catch
                {
                    return datetime;
                }
            }
        }
        #endregion

        #region Struct

        #region 转换 Float Int Uint
        /// <summary>
        /// 对象的各个成员在非托管内存中的精确位置被显式控制。
        /// 每个成员使用 FieldOffsetAttribute 指示该字段在类型中的位置
        /// 转换 Float Int Uint
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 4)]
        public struct Convert_Float_Int_Uint
        {
            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(0)]
            public uint uiUint;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(0)]
            public int iInt;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(0)]
            public float fFloat;
        }
        #endregion

        #region 转换 Double Long Ulong
        /// <summary>
        /// 对象的各个成员在非托管内存中的精确位置被显式控制。
        /// 每个成员使用 FieldOffsetAttribute 指示该字段在类型中的位置
        /// 转换 Double Long Ulong
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 8)]
        public struct Convert_Double_Long_Ulong
        {
            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(0)]
            public ulong ulUlong;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(0)]
            public long lLong;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(0)]
            public double dDouble;
        }
        #endregion

        #endregion

        #region ConvertString
        /// <summary>
        /// 字符串转换处理
        /// </summary>
        public static class ConvertString
        {
            #region 汉字拼音
            #region 转换函数
            public static string ChinaLetters(string Str)
            {
                byte[] ZW = new byte[2];
                long HZ_INT;
                ZW = Encoding.Default.GetBytes(Str);
                int i1 = ((short)((ZW[0])));
                int i2 = ((short)((ZW[1])));
                HZ_INT = i1 * 256 + i2;
                if ((HZ_INT >= 45217) && (HZ_INT <= 45252))
                {
                    return "A";
                }
                if ((HZ_INT >= 45253) && (HZ_INT <= 45760))
                {
                    return "B";
                }
                if ((HZ_INT >= 45761) && (HZ_INT <= 46317))
                {
                    return "C";
                }
                if ((HZ_INT >= 46318) && (HZ_INT <= 46825))
                {
                    return "D";
                }
                if ((HZ_INT >= 46826) && (HZ_INT <= 47009))
                {
                    return "E";
                }
                if ((HZ_INT >= 47010) && (HZ_INT <= 47296))
                {
                    return "F";
                }
                if ((HZ_INT >= 47297) && (HZ_INT <= 47613))
                {
                    return "G";
                }
                if ((HZ_INT >= 47614) && (HZ_INT <= 48118))
                {
                    return "H";
                }
                if ((HZ_INT >= 48119) && (HZ_INT <= 49061))
                {
                    return "J";
                }
                if ((HZ_INT >= 49062) && (HZ_INT <= 49323))
                {
                    return "K";
                }
                if ((HZ_INT >= 49324) && (HZ_INT <= 49895))
                {
                    return "L";
                }
                if ((HZ_INT >= 49896) && (HZ_INT <= 50370))
                {
                    return "M";
                }
                if ((HZ_INT >= 50371) && (HZ_INT <= 50613))
                {
                    return "N";
                }
                if ((HZ_INT >= 50614) && (HZ_INT <= 50621))
                {
                    return "O";
                }
                if ((HZ_INT >= 50622) && (HZ_INT <= 50905))
                {
                    return "P";
                }
                if ((HZ_INT >= 50906) && (HZ_INT <= 51386))
                {
                    return "Q";
                }
                if ((HZ_INT >= 51387) && (HZ_INT <= 51445))
                {
                    return "R";
                }
                if ((HZ_INT >= 51446) && (HZ_INT <= 52217))
                {
                    return "S";
                }
                if ((HZ_INT >= 52218) && (HZ_INT <= 52697))
                {
                    return "T";
                }
                if ((HZ_INT >= 52698) && (HZ_INT <= 52979))
                {
                    return "W";
                }
                if ((HZ_INT >= 52980) && (HZ_INT <= 53640))
                {
                    return "X";
                }
                if ((HZ_INT >= 53689) && (HZ_INT <= 54480))
                {
                    return "Y";
                }
                if ((HZ_INT >= 54481) && (HZ_INT <= 55289))
                {
                    return "Z";
                }
                return Convert.ToString(Str.Substring(0, 1)).ToUpper();
            }
            #endregion
            #endregion

            #region 半角全角
            /// <summary>
            /// 转半角的函数(DBC case)
            /// </summary>
            /// <param name="input">任意字符串</param>
            /// <returns>半角字符串</returns>
            ///<remarks>
            ///全角空格为12288，半角空格为32
            ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
            ///</remarks>
            public static string ToDbc(string input)
            {
                char[] c = input.ToCharArray();
                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i] == 12288)
                    {
                        c[i] = (char)32;
                        continue;
                    }
                    if (c[i] > 65280 && c[i] < 65375)
                    {
                        c[i] = (char)(c[i] - 65248);
                    }
                }
                return new string(c);
            }

            /// <summary>
            /// 转全角的函数(SBC case)
            /// </summary>
            /// <param name="input">任意字符串</param>
            /// <returns>全角字符串</returns>
            ///<remarks>
            ///全角空格为12288，半角空格为32
            ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
            ///</remarks>        
            public static string ToSbc(string input)
            {
                //半角转全角：
                char[] c = input.ToCharArray();
                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i] == 32)
                    {
                        c[i] = (char)12288;
                        continue;
                    }
                    if (c[i] < 127)
                    {
                        c[i] = (char)(c[i] + 65248);
                    }
                }
                return new string(c);
            }
            #endregion 半角全角

            #region 中文货币
            /// <summary>
            /// 人民币转换中文
            /// </summary>
            /// <param name="money">输入人民币</param>
            /// <returns></returns>
            public static string ChineseMoneys(decimal money)
            {
                string Capstr = null;

                string Cap = "零壹贰叁肆伍陆柒捌玖";
                string Numstr = "0123456789";
                string MoneyNumstr = money.ToString();
                int Pint = MoneyNumstr.IndexOf(".");
                int Numint;

                string Moneyint = null;
                string Moneydec = null;
                string Intstr = null;
                string MoneyCap = null;
                string Moneyintstr = null;
                string Moneydecstr = null;
                //Capstr=Pint.ToString(); 

                if (Pint != -1)
                {
                    string strArr = ".";
                    char[] charArr = strArr.ToCharArray();
                    string[] MoneyNumArr = MoneyNumstr.Split(charArr);
                    Moneyint = MoneyNumArr[0].ToString();
                    Moneydec = MoneyNumArr[1].ToString();

                }
                else
                {
                    Moneyint = MoneyNumstr;
                    Moneydec = "00";
                }

                if (Moneyint.Length > 16)
                {
                    //throw("数值超界");
                }
                else
                {
                    //--- 处理整数部分-------- 

                    for (int j = 1; j <= Moneyint.Length; j++)
                    {
                        Moneyintstr = Moneyint.Substring(j - 1, 1);
                        for (int i = 0; i <= 9; i++)
                        {

                            Intstr = Numstr.Substring(i, 1);
                            MoneyCap = Cap.Substring(i, 1);

                            if (Moneyintstr == Intstr)
                            {

                                switch (Intstr)
                                {
                                    case "0":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "1":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "2":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "3":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "4":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "5":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "6":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "7":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "8":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "9":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                }
                            }
                        }
                        Numint = Moneyint.Length - j + 1;
                        switch (Numint)
                        {
                            case 16:
                                Capstr = Capstr + "仟万";
                                break;
                            case 15:
                                Capstr = Capstr + "佰万";
                                break;
                            case 14:
                                Capstr = Capstr + "拾万";
                                break;
                            case 13:
                                Capstr = Capstr + "万";
                                break;

                            case 12:
                                Capstr = Capstr + "仟";
                                break;

                            case 11:
                                Capstr = Capstr + "佰";
                                break;

                            case 10:
                                Capstr = Capstr + "拾";
                                break;

                            case 9:
                                Capstr = Capstr + "亿";
                                break;

                            case 8:
                                Capstr = Capstr + "仟";
                                break;

                            case 7:
                                Capstr = Capstr + "佰";
                                break;

                            case 6:
                                Capstr = Capstr + "拾";
                                break;


                            case 5:
                                Capstr = Capstr + "万";
                                break;

                            case 4:
                                Capstr = Capstr + "仟";
                                break;
                            case 3:
                                Capstr = Capstr + "佰";
                                break;
                            case 2:
                                Capstr = Capstr + "拾";
                                break;
                            case 1:
                                Capstr = Capstr + "元";
                                break;

                        }

                    }

                    //------处理小数部分－－－－－ 
                    for (int j = 1; j <= 2; j++)
                    {
                        Moneydecstr = Moneydec.Substring(j - 1, 1);

                        for (int i = 0; i <= 9; i++)
                        {
                            Intstr = Numstr.Substring(i, 1);
                            MoneyCap = Cap.Substring(i, 1);
                            if (Moneydecstr == Intstr)
                            {

                                switch (Intstr)
                                {
                                    case "0":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "1":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "2":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "3":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "4":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "5":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "6":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "7":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "8":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                    case "9":
                                        Capstr = Capstr + MoneyCap;
                                        break;
                                }
                            }
                        }

                        switch (j)
                        {
                            case 1:
                                Capstr = Capstr + "角";
                                break;
                            case 2:
                                Capstr = Capstr + "分";
                                break;
                        }
                    }
                }
                return Capstr;
            }
            #endregion 人民币转换中文

            #region 内部静态属性
            /// <summary>
            /// UTF8编码
            /// </summary>
            private static Encoding s_UTF8 = new UTF8Encoding(false, false);
            /// <summary>
            /// 安全 UTF8
            /// </summary>
            public static Encoding UTF8
            {
                get { return s_UTF8; }
            }
            #endregion

            #region To...
            /// <summary>
            /// 转换 string 类型 成 bool 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static bool ToBoolean(string strValue)
            {
                bool bReturn = false;

                bool.TryParse(strValue, out bReturn);

                return bReturn;
            }

            /// <summary>
            /// 转换 string 类型 成 bool 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static bool ConvertToBoolean(string strValue)
            {
                return ToBoolean(strValue);
            }

            /// <summary>
            /// 转换 string 类型 成 float 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static float ToSingle(string strValue)
            {
                float fReturn = 0.0F;

                float.TryParse(strValue, out fReturn);

                return fReturn;
            }

            /// <summary>
            /// 转换 string 类型 成 float 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static float ConvertToSingle(string strValue)
            {
                return ToSingle(strValue);
            }

            /// <summary>
            /// 转换 string 类型 成 double 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static double ToDouble(string strValue)
            {
                double dReturn = 0.0;

                double.TryParse(strValue, out dReturn);

                return dReturn;
            }

            /// <summary>
            /// 转换 string 类型 成 double 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static double ConvertToDouble(string strValue)
            {
                return ToDouble(strValue);
            }

            /// <summary>
            /// 转换 string 类型 成 TimeSpan 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static TimeSpan ToTimeSpan(string strValue)
            {
                TimeSpan timeSpan;

                TimeSpan.TryParse(strValue, out timeSpan);

                return timeSpan;
            }


            /// <summary>
            /// 转换 string 类型 成 TimeSpan 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static TimeSpan ConvertToTimeSpan(string strValue)
            {
                return ToTimeSpan(strValue);
            }

            /// <summary>
            /// 转换 string 类型 成 DateTime 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static DateTime ToDateTime(string strValue)
            {
                DateTime dateTime;

                DateTime.TryParse(strValue, out dateTime);

                return dateTime;
            }

            /// <summary>
            /// 转换 string 类型 成 DateTime 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static DateTime ConvertToDateTime(string strValue)
            {
                return ToDateTime(strValue);
            }

            /// <summary>
            /// 转换 string 类型 成 short 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static short ToInt16(string strValue)
            {
                short iReturn = 0;

                if (strValue.StartsWith("0x"))
                    short.TryParse(strValue.Substring(2), NumberStyles.HexNumber, null, out iReturn);
                else
                    short.TryParse(strValue, out iReturn);

                return iReturn;
            }

            /// <summary>
            /// 转换 string 类型 成 short 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static short ConvertToInt16(string strValue)
            {
                return ToInt16(strValue);
            }

            /// <summary>
            /// 转换 string 类型 成 ushort 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static ushort ToUInt16(string strValue)
            {
                ushort iReturn = 0;

                if (strValue.StartsWith("0x"))
                    ushort.TryParse(strValue.Substring(2), NumberStyles.HexNumber, null, out iReturn);
                else
                    ushort.TryParse(strValue, out iReturn);

                return iReturn;
            }

            /// <summary>
            /// 转换 string 类型 成 ushort 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static ushort ConvertToUInt16(string strValue)
            {
                return ToUInt16(strValue);
            }

            /// <summary>
            /// 转换 string 类型 成 int 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static int ToInt32(string strValue)
            {
                int iReturn = 0;

                if (strValue.StartsWith("0x"))
                    int.TryParse(strValue.Substring(2), NumberStyles.HexNumber, null, out iReturn);
                else
                    int.TryParse(strValue, out iReturn);

                return iReturn;
            }

            /// <summary>
            /// 转换 string 类型 成 int 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static int ConvertToInt32(string strValue)
            {
                return ToInt32(strValue);
            }

            /// <summary>
            /// 转换 string 类型 成 uint 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static uint ToUInt32(string strValue)
            {
                uint iReturn = 0;

                if (strValue.StartsWith("0x"))
                    uint.TryParse(strValue.Substring(2), NumberStyles.HexNumber, null, out iReturn);
                else
                    uint.TryParse(strValue, out iReturn);

                return iReturn;
            }

            /// <summary>
            /// 转换 string 类型 成 uint 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static uint ConvertToUInt32(string strValue)
            {
                return ToUInt32(strValue);
            }

            /// <summary>
            /// 转换 string 类型 成 long 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static long ToLong64(string strValue)
            {
                long lReturn = 0;

                if (strValue.StartsWith("0x"))
                    long.TryParse(strValue.Substring(2), NumberStyles.HexNumber, null, out lReturn);
                else
                    long.TryParse(strValue, out lReturn);

                return lReturn;
            }

            /// <summary>
            /// 转换 string 类型 成 long 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static long ConvertToLong64(string strValue)
            {
                return ToLong64(strValue);
            }

            /// <summary>
            /// 转换 string 类型 成 ulong 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static ulong ToULong64(string strValue)
            {
                ulong lReturn = 0;

                if (strValue.StartsWith("0x"))
                {
                    ulong.TryParse(strValue.Substring(2), NumberStyles.HexNumber, null, out lReturn);
                }
                else
                {
                    ulong.TryParse(strValue, out lReturn);
                }

                return lReturn;
            }

            /// <summary>
            /// 转换 string 类型 成 ulong 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static ulong ConvertToULong64(string strValue)
            {
                return ToULong64(strValue);
            }

            /// <summary>
            /// 转换 string 类型 成 IPAddress 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static IPAddress ToIPAddress(string strValue)
            {
                IPAddress ipAddress = IPAddress.None;

                IPAddress.TryParse(strValue, out ipAddress);

                return ipAddress;
            }

            /// <summary>
            /// 转换 string 类型 成 IPAddress 类型
            /// </summary>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static IPAddress ConvertToIPAddress(string strValue)
            {
                return ToIPAddress(strValue);
            }

            /// <summary>
            /// 字符串转换成字节数组
            /// </summary>
            /// <param name="byteString"></param>
            /// <returns></returns>
            public static byte[] ToByteArray(string byteString)
            {
                return s_UTF8.GetBytes(byteString);
            }

            /// <summary>
            /// 字符串转换成字节数组
            /// </summary>
            /// <param name="byteString">要转换的字符串</param>
            /// <param name="encoding">转换的编码方式</param>
            /// <returns></returns>
            public static byte[] ToByteArray(string byteString, Encoding encoding)
            {
                return encoding.GetBytes(byteString);
            }

            #endregion

            #region Concat...
            /// <summary>
            /// 合并字符串
            /// </summary>
            /// <param name="stringBuilder">返回的字符串</param>
            /// <param name="strStringList">合并的字符串列表</param>
            public static void Concat(ref StringBuilder stringBuilder, params string[] strStringList)
            {
                for (int iIndex = 0; iIndex < strStringList.Length; iIndex++)
                    stringBuilder.Append(strStringList[iIndex]);
            }

            /// <summary>
            /// 合并字符串
            /// </summary>
            /// <param name="strString">限定的对象类型</param>
            /// <param name="stringBuilder">返回的字符串</param>
            /// <param name="strString2">要合并的字符串</param>
            public static void Coalition(string strString, ref StringBuilder stringBuilder, string strString2)
            {
                Concat(ref stringBuilder, strString, strString2);
            }

            /// <summary>
            /// 合并字符串
            /// </summary>
            /// <param name="strString">限定的对象类型</param>
            /// <param name="stringBuilder">返回的字符串</param>
            /// <param name="strString2">要合并的字符串</param>
            /// <param name="strString3">要合并的字符串</param>
            public static void Coalition(string strString, ref StringBuilder stringBuilder, string strString2, string strString3)
            {
                Concat(ref stringBuilder, strString, strString2, strString3);
            }

            /// <summary>
            /// 合并字符串
            /// </summary>
            /// <param name="strString">限定的对象类型</param>
            /// <param name="stringBuilder">返回的字符串</param>
            /// <param name="strString2">要合并的字符串</param>
            /// <param name="strString3">要合并的字符串</param>
            /// <param name="strString4">要合并的字符串</param>
            public static void Coalition(string strString, ref StringBuilder stringBuilder, string strString2, string strString3, string strString4)
            {
                Concat(ref stringBuilder, strString, strString2, strString3, strString4);
            }
            #endregion
        }

        #endregion
    }
}
