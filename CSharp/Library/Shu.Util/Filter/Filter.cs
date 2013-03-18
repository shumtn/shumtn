using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Net; // 提供webClient
using System.Configuration;
using System.Text.RegularExpressions;

namespace Shu.Util
{
    public class Filter
    {
        /// <summary>
        /// 过滤字符串
        /// </summary>
        /// <param name="Str">要过滤字符</param>
        /// <param name="Res">正则表达式</param>
        /// <returns></returns>
        /// </summary>
        public static MatchCollection StrReg(string strcode, string strres)
        {
            try
            {
                MatchCollection mc = Regex.Matches(strcode, strres, RegexOptions.IgnoreCase);
                return mc;
            }
            catch
            {
                return null;
            }
        }






        #region 字符切断
        /// <summary>
        /// 切断函数
        /// </summary>
        /// <param name="inputString">要处理的文本</param>
        /// <param name="len">长度</param>
        /// <param name="have">是否有省略号</param>
        /// <returns></returns>
        public static string CuttingString(string inputString, int len, bool have = true)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号
            byte[] mybyte = Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
            {
                if (have == true)
                {
                    tempString += "…";
                }
                else
                {
                    tempString += "";
                }
            }
            return tempString;
        }
        #endregion 字符切断

        ///// <summary>
        ///// 过滤字符串
        ///// </summary>
        ///// <param name="Str">字符串</param>
        ///// <param name="Res">表达式</param>
        ///// <returns></returns>
        ///// </summary>
        //public static string FiltrString(String Str, String Res)
        //{
        //    string jcstr = null;
        //    if (!String.IsNullOrEmpty(Str))
        //    {
        //        if (Regex.IsMatch(Str, Res))
        //        {
        //            jcstr = Str;
        //        }
        //        else
        //        {
        //            jcstr = null;
        //        }
        //        return jcstr;
        //    }
        //    else
        //    {
        //        jcstr = null;
        //    }
        //    return jcstr;
        //}

        /// <summary>
        /// 查询字符串是否存在 并返回星号
        /// </summary>        
        /// <param name="Str">要查询的字符串</param>
        /// <param name="Src">源查询的字符源</param>
        /// <returns></returns>
        public static string SelectString(string Str, string Src)
        {
            string[] strWords = Src.Split(new char[] { '|' });
            string fhstr = null;
            foreach (string strWord in strWords)
            {
                if (Str == strWord)
                {
                    fhstr = Str;
                    break;
                }
                else
                {
                    fhstr = null;
                }
            }
            return fhstr;
        }

        /// <summary>
        /// 过滤脏话
        /// </summary>
        /// <param name="Str">要过滤的文本</param>
        /// <returns>返回过滤后的文本</returns>
        public static string String(string str, string url = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = ConfigurationManager.AppSettings["ZangHuaGuoLv"].ToString();
            }

            string sLine = null;
            using (StreamReader sr = new StreamReader(url))
            {
                sLine = sr.ReadToEnd();
            }

            ArrayList arrText = new ArrayList();
            if (sLine != null) arrText.Add(sLine);
            foreach (string sOutput in arrText)
            {
                string[] strArr = sOutput.Split('@');
                for (int i = 0; i < strArr.Length; i++)
                {
                    string temp = "";
                    for (int j = 0; j < strArr[i].Length; j++) { temp += "*"; }
                    str = str.Replace(strArr[i], temp);
                }
            }
            return str;
        }

        /// <summary>
        /// 过滤数字
        /// </summary>
        /// <param name="Int">数 字</param>
        /// <param name="Res">表达式</param>
        /// <returns></returns>
        public static Int32 Numeric(Int32 Int, String Res)
        {
            Int32 jcint = 0;
            if (Int != 0)
            {
                if (Regex.IsMatch(Convert.ToString(Int), Res))
                {
                    jcint = Int;
                }
                else
                {
                    jcint = 0;
                }
                return jcint;
            }
            else
            {
                jcint = 0;
            }
            return jcint;
        }

        /// <summary>
        /// 过滤ＨＴＭＬ代码
        /// </summary>
        /// <param name="source">过滤的源</param>
        /// <returns></returns>
        public static string Html(string source)
        {
            try
            {
                string result;
                result = source.Replace("\r", " ");
                result = result.Replace("\n", " ");
                result = result.Replace("'", " ");
                result = result.Replace("\t", string.Empty);
                result = Regex.Replace(result, @"( )+", " ");
                result = Regex.Replace(result, @"<( )*head([^>])*>", "<head>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*head( )*>)", "</head>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(<head>).*(</head>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*script([^>])*>", "<script>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*script( )*>)", "</script>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*style([^>])*>", "<style>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*style( )*>)", "</style>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(<style>).*(</style>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*td([^>])*>", "\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*br( )*>", "\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*li( )*>", "\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*div([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*p([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @" ", " ", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"•", " * ", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"‹", "<", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"›", ">", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"™", "(tm)", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"⁄", "/", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<", "<", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @">", ">", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"©", "(c)", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"®", "(r)", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&(.{2,6});", string.Empty, RegexOptions.IgnoreCase);
                result = result.Replace("\n", "\r");
                result = Regex.Replace(result, "(\r)( )+(\r)", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\t)( )+(\t)", "\t\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\t)( )+(\r)", "\t\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\r)( )+(\t)", "\r\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\r)(\t)+(\r)", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\r)(\t)+", "\r\t", RegexOptions.IgnoreCase);
                string breaks = "\r\r\r";
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }
                return result.Trim();
            }
            catch
            {
                return source.Trim();
            }
        }

        /// <summary>
        /// 防SQL语句注入
        /// </summary>
        /// <param name="ParaName">参数名称</param>
        /// <param name="ParaType">参数类型 1代表数字 0为字符串</param>
        /// <returns></returns>
        public static string Sql(string txtStr, int txtType)
        {
            //如果是1为数字，0为字符串
            string Paravalue = null;
            if (txtStr == null)
            {
                Paravalue = "0";
            }
            else if (txtStr.ToString() == "")
            {
                Paravalue = "0";
            }
            else
            {
                Paravalue = txtStr.ToString().ToLower();

                if (txtType == 1)
                {
                    if (!(IsNumeric(Paravalue)))
                    {
                        Paravalue = "0";
                    }
                }
                else
                {
                    Paravalue = Paravalue.Replace("%20", "").Replace("%", "").Replace("&", "&").Replace("<", "<").Replace(">", ">").Replace("§", "§§").Replace("*", "").Replace("\n", "").Replace("\r\n", "").Replace("create", "").Replace("select", "").Replace("insert", "").Replace("update", "").Replace("delete", "").Replace("create", "").Replace("drop", "").Replace("from", "").Replace("delcare", "").Replace("exec", "").Replace("char", "").Replace("where", "").Replace("or", "").Replace("and", "").Replace("begin", "").Replace(" ", " ");
                }
            }
            return Paravalue;
        }

        /// <summary>
        /// 检查时候是数字
        /// </summary>
        /// <param name="strData">要检查的数据</param>
        /// <returns></returns>
        private static bool IsNumeric(string strData)
        {
            float fData;
            bool bValid = true;
            try
            {
                fData = float.Parse(strData);
            }
            catch (FormatException)
            {
                bValid = false;
            }
            return bValid;
        }
    }
}

