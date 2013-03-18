//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Net;
//using System.IO;

//namespace Shu.Util
//{
//    /// <summary>
//    /// 获取Html代码
//    /// </summary>
//    public class Html
//    {
//        #region 生成Html
//        /// <summary>
//        /// 生成静态文件
//        /// </summary>
//        /// <param name="StrUrl">原文件</param>
//        /// <param name="StrEnc">编码</param>
//        /// <returns></returns>
//        public static string Get(string StrUrl, string StrEnc)
//        {
//            try
//            {
//                //System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(NewUrl));
//                //System.Web.HttpContext.Current.Response.Write("http://"+ System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + StrUrl);
//                string lcHtml = null;
//                HttpWebRequest Hwrq;
//                Hwrq = (HttpWebRequest)WebRequest.Create(StrUrl);
//                Hwrq.Timeout = 50000;
//                HttpWebResponse Hwrs = (HttpWebResponse)Hwrq.GetResponse();
//                Encoding Enc = Encoding.GetEncoding(StrEnc);
//                StreamReader Sr = new StreamReader(Hwrs.GetResponseStream(), Enc);
//                lcHtml = Sr.ReadToEnd();
//                Hwrs.Close();
//                Sr.Close();
//                //StreamWriter Sw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath(NewUrl), true, System.Text.Encoding.GetEncoding(StrEnc));
//                //Sw.WriteLine(lcHtml);
//                //Sw.Close();
//                //Sw.Dispose();
//                return lcHtml;
//            }
//            catch
//            {
//                return null;
//            }
//        }

//        /// <summary>
//        /// 生成静态文件
//        /// </summary>
//        /// <param name="StrUrl">原文件</param>
//        /// <param name="StrEnc">编码</param>
//        /// <param name="NewUrl">新文件</param>
//        /// <returns></returns>
//        public static bool Get(string StrUrl, string StrEnc, string NewUrl)
//        {
//            try
//            {
//                System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(NewUrl));
//                //System.Web.HttpContext.Current.Response.Write("http://"+ System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + StrUrl);
//                string lcHtml = "<!--#include virtual=" + "\"" + "hehe" + "\"" + "-->";
//                HttpWebRequest Hwrq;
//                Hwrq = (HttpWebRequest)WebRequest.Create("http://" + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + StrUrl);
//                Hwrq.Timeout = 50000;
//                HttpWebResponse Hwrs = (HttpWebResponse)Hwrq.GetResponse();
//                Encoding Enc = Encoding.GetEncoding(StrEnc);
//                StreamReader Sr = new StreamReader(Hwrs.GetResponseStream(), Enc);
//                lcHtml = Sr.ReadToEnd();
//                Hwrs.Close();
//                Sr.Close();
//                StreamWriter Sw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath(NewUrl), true, System.Text.Encoding.GetEncoding(StrEnc));
//                Sw.WriteLine(lcHtml);
//                Sw.Close();
//                Sw.Dispose();
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }
//        #endregion
//    }
//}

