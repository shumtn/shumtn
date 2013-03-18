using System;
using System.IO;
using System.Text;
using System.Configuration;

namespace Shu.Util
{
    /// <summary>
    /// 控制台调试输出信息的文件数据输出流的类
    /// </summary>
    internal class MessageFile : TextWriter, IDisposable
    {
        #region 常量
        private static string m_LogPath = null;
        /// <summary>
        /// 日志路径
        /// </summary>
        public static string Path
        {
            get { return m_LogPath; }
        }

        private static string m_FileName = null;
        /// <summary>
        /// 数据输出的文件的文件名
        /// </summary>
        public static string FileName
        {
            get { return m_FileName; }
        }


        private static string m_FullFileName = null;
        /// <summary>
        /// 数据输出的完整路径和文件的文件名
        /// </summary>
        public static string FullFileName
        {
            get { return m_FullFileName; }
        }

        /// <summary>
        /// 日志要记录的类型
        /// </summary>
        public MessageType LogType
        {
            get { return m_LogType; }
            set { m_LogType = value; }
        }

        
        private bool m_bIsLogFile = false;
        /// <summary>
        /// 是否记录文件
        /// </summary>
        public bool IsLogFile
        {
            get { return m_bIsLogFile; }
            set { m_bIsLogFile = value; }
        }

        private string m_LogName = string.Empty;
        /// <summary>
        /// 日志要记录的类型
        /// </summary>
        private MessageType m_LogType = MessageType.None;
        

        /// <summary>
        /// 返回系统的缺省代码页的编码
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        /// <summary>
        /// 日志日期的格式
        /// </summary>
        private readonly static string DateFormat = "[yy年 M月 d日 HH:mm:ss.f tt] ";//"[MMMM dd hh:mm:ss.f tt]: ";

        /// <summary>
        /// 文件数据输出流
        /// </summary>
        private StringBuilder m_StringBuilder = new StringBuilder();
        #endregion

        #region 构造
        /// <summary>
        /// 控制台调试输出信息的文件数据输出流的构造函数
        /// </summary>
        public MessageFile()
        {
            string logFilePath = null, logFileName = null, logDateTime = null;

            //读取 LogFilePath 节点
            try
            {
                logFilePath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
            }
            catch
            {
                logFilePath = null;
            }

            //读取 LogFileName 节点
            try
            {
                logFileName = ConfigurationManager.AppSettings["LogFileName"].ToString();
            }
            catch
            {
                logFileName = null;
            }

            //读取 LogDateTime 节点
            try
            {
                logDateTime = ConfigurationManager.AppSettings["LogDateTime"].ToString();                
            }
            catch
            {
                logDateTime = null;
            }

            //判断 是否 指定 日志 路径
            if (!string.IsNullOrEmpty(logFilePath))
            {
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }
            }
            else
            {
                logFilePath = "./Logs";

                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }
            }

            m_LogPath = logFilePath;

            //判断 是否 指定 日志 文件 名称
            if (!string.IsNullOrEmpty(logFileName))
            {
                if (!string.IsNullOrEmpty(logDateTime))
                {
                    logFileName = DateTime.Now.ToString(logDateTime) + "_" + logFileName;
                }
                else
                {
                    logFileName = DateTime.Now.ToString("yyyy_MM_dd") + "_" + logFileName;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(logDateTime))
                {
                    logFileName = DateTime.Now.ToString(logDateTime) + "_Log.txt";
                }
                else
                {
                    logFileName = DateTime.Now.ToString("yyyy_MM_dd") + "_Log.txt";
                }
            }

            m_FileName = logFileName;

            m_bIsLogFile = true;

            m_FullFileName = logFilePath + "/" + logFileName;
        }
        #endregion       

        #region 方法
        /// <summary>
        /// 在文件数据输出流中写入一个字符信息
        /// </summary>
        /// <param name="charWrite">字符</param>
        public override void Write(char charWrite)
        {
            m_StringBuilder.Append(Message.StringFilter(charWrite));
        }

        /// <summary>
        /// 在文件数据输出流中写入一行文本信息
        /// </summary>
        /// <param name="strWrite">字符串</param>
        public override void Write(string strWrite)
        {
            m_StringBuilder.Append(strWrite);
        }

        /// <summary>
        /// 在文件数据输出流中写入一行具有参数的文本信息
        /// </summary>
        /// <param name="strWriteLine">字符串</param>
        public override void WriteLine(string strWriteLine)
        {
            m_StringBuilder.Append(strWriteLine);

            m_StringBuilder.Insert(0, DateTime.Now.ToString(DateFormat));

            if (m_bIsLogFile == true)
            {
                if (m_LogType == MessageType.None)
                {
                    Message.WriteLineLogFile(m_FullFileName, m_StringBuilder.ToString());
                }
                else
                {
                    Message.WriteLineLogFile(m_FullFileName, m_StringBuilder.ToString(), m_LogType);
                }
            }

            m_StringBuilder.Length = 0;
        }
        #endregion
    }
}

