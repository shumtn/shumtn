using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;
using System.Data;

namespace Shu.Util
{
    /// <summary>
    /// 插入拼音
    /// </summary>
    public class GetString
    {
        /// <summary>
        /// 远程要获取代码
        /// </summary>
        /// <param name="urls">要获取的代码的路径</param>
        /// <returns>返回获取后的代码</returns>
        public static string 远程获取(string urls)
        {
            WebClient wc = new WebClient();
            Stream s = wc.OpenRead(urls);
            Encoding enc = Encoding.GetEncoding("gb2312");//utf-8 gb2312
            StreamReader sr = new StreamReader(s,enc);
            string sLine = sr.ReadToEnd();
            sr.Close();
            return sLine.Trim();
        }

        /// <summary>
        /// 远程要获取代码
        /// </summary>
        /// <param name="urls">要获取的代码的路径</param>
        /// <returns>返回获取后的代码</returns>
        public static string GetCode(string urls)
        {
            WebClient wc = new WebClient();
            Stream s = wc.OpenRead(urls);
            Encoding enc = Encoding.GetEncoding("utf-8");//utf-8 gb2312
            StreamReader sr = new StreamReader(s, enc);
            string sLine = sr.ReadToEnd();
            sr.Close();
            return sLine.Trim();
        }

        /// <summary> 
        /// 替换通过正则获取字符串所带的正则首尾匹配字符串 
        /// </summary> 
        /// <param name="RegValue">要替换的值</param> 
        /// <param name="regStart">正则匹配的首字符串</param> 
        /// <param name="regEnd">正则匹配的尾字符串</param> 
        /// <returns></returns> 
        public static string RegReplace(string RegValue, string regStart, string regEnd)
        {
            string s = RegValue;
            if (RegValue != "" && RegValue != null)
            {
                if (regStart != "" && regStart != null)
                {
                    s = s.Replace(regStart, "");
                }
                if (regEnd != "" && regEnd != null)
                {
                    s = s.Replace(regEnd, "");
                }
            }
            return s;
        }

        /// <summary> 
        /// 替换通过正则获取字符串所带的正则首尾匹配字符串 
        /// </summary> 
        /// <param name="RegValue">要替换的值</param> 
        /// <param name="regStart">正则匹配的首字符串</param> 
        /// <param name="regEnd">正则匹配的尾字符串</param> 
        /// <returns></returns> 
        public static string GetValue(string value, string start, string end)
        {
            string s = value, ww = null, xx = null;
            int w = 0, x = 0;
            w = s.IndexOf(start) + start.Length;
            ww = s.Remove(0, w);
            x = ww.LastIndexOf(end);
            xx = ww.Remove(x);
            return xx.Trim();
        }

        /// <summary> 
        /// 替换通过正则获取字符串所带的正则首尾匹配字符串 
        /// </summary> 
        /// <param name="RegValue">要替换的值</param> 
        /// <param name="regStart">正则匹配的首字符串</param> 
        /// <param name="regEnd">正则匹配的尾字符串</param> 
        /// <returns></returns> 
        public static string DelValue(string value, string start, string end)
        {
            string s = value, xx = null; // ww = null, 
            int w = 0, x = 0;
            //w = s.IndexOf(start) + start.Length;
            //ww = s.Remove(0, w);
            //x = ww.LastIndexOf(end);
            //xx = ww.Remove(x);

            w = s.IndexOf(start);
            x = s.LastIndexOf(end);// +end.Length;
            xx = s.Remove(w, x - w);
            return xx.Trim();
        } 
    }
}
