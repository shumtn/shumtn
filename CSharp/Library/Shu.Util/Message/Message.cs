using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Configuration;

namespace Shu.Util
{
    #region 枚举
    /// <summary>
    /// 日志消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 初始化 黄色
        /// </summary>
        Init,
        /// <summary>
        /// 默认,灰色
        /// </summary>
        None,
        /// <summary>
        /// 状态,绿色
        /// </summary>
        Status,
        /// <summary>
        /// 数据,紫红色
        /// </summary>
        Sql,
        /// <summary>
        /// 信息,绿色
        /// </summary>
        Info,
        /// <summary>
        /// 提示,黄色
        /// </summary>
        Notice,
        /// <summary>
        /// 警告,青色
        /// </summary>
        Warning,
        /// <summary>
        /// 调试信息,蓝色
        /// </summary>
        Debug,
        /// <summary>
        /// 错误,红色
        /// </summary>
        Error,
        /// <summary>
        /// 致命错误
        /// </summary>
        FatalError,
        /// <summary>
        /// 修改，插件,红色
        /// </summary>
        Hack,
        /// <summary>
        /// 加载信息,红色
        /// </summary>
        Load,
        /// <summary>
        /// 普通输出,绿色或白色
        /// </summary>
        Input,
        /// <summary>
        /// 在控制台需要输出的,绿色
        /// </summary>
        DosPrompt
    }
    #endregion

    /// <summary>
    /// 线程安全的日志
    /// </summary>
    public class Message
    {
        #region 声明
        /// <summary>
        /// MessageFile 对象
        /// </summary>
        private static MessageFile m_MessageFile = null;

        /// <summary>
        /// 消息
        /// </summary>
        private static string m_Message = null;

        /// <summary>
        /// 当前调用 名称空间 + 类名称 + 方法名
        /// </summary>
        private static string m_ClassFullName = null;

        /// <summary>
        /// 输入的信息
        /// </summary>
        private static string s_strInput = string.Empty;
        /// <summary>
        /// 控制台输出的信息
        /// </summary>
        private static string s_strDosPrompt = string.Empty;

        /// <summary>
        /// 当前需要处理的集合
        /// </summary>
        private static Queue<LogInfo> s_LogInfoQueue = new Queue<LogInfo>();
        /// <summary>
        /// 线程锁
        /// </summary>
        private static ReaderWriterLockSlim s_LockLogInfoQueue = new ReaderWriterLockSlim();
        /// <summary>
        /// 锁标志
        /// </summary>
        private static volatile bool s_IsLock = false; //volatile 指示一个字段可以由多个同时执行的线程修改

        #endregion

        #region 结构
        /// <summary>
        /// 日志消息结构
        /// </summary>
        private struct LogInfo
        {
            #region 构造和初始化和清理
            /// <summary>
            /// 构造
            /// </summary>
            /// <param name="messageFlag">日志消息的类型标志</param>
            /// <param name="strFormat">要格式的字符串</param>
            /// <param name="parameter">格式参数</param>
            public LogInfo(MessageType messageFlag, string strFormat, object[] parameter)
            {
                m_MessageFlag = messageFlag;
                m_strFormat = strFormat;
                m_Parameter = parameter;
            }
            #endregion

            #region 属性
            #region 私有成员变量
            /// <summary>
            /// 日志消息类型
            /// </summary>
            private MessageType m_MessageFlag;
            #endregion
            /// <summary>
            /// 日志消息的类型标志
            /// </summary>
            public MessageType MessageFlag
            {
                get { return m_MessageFlag; }
            }

            #region 私有成员变量
            /// <summary>
            /// 要格式的字符串
            /// </summary>
            private string m_strFormat;
            #endregion
            /// <summary>
            /// 要格式的字符串
            /// </summary>
            public string Format
            {
                get { return m_strFormat; }
            }

            #region 私有成员变量
            /// <summary>
            /// 格式参数
            /// </summary>
            private object[] m_Parameter;
            #endregion
            /// <summary>
            /// 格式参数
            /// </summary>
            public object[] Parameter
            {
                get { return m_Parameter; }
            }
            #endregion
        }
        #endregion

        #region 方法

        #region 公共
        public static void Write(MessageType messageType, string strFormat)
        {
            InternalWrite(messageType, strFormat);
        }

        public static void WriteLine(ConsoleColor messageColor, string strFormat)
        {
            ColorWriteLine(messageColor, strFormat);
        }

        ///// <summary>
        ///// 防止多线程的问题
        ///// </summary>
        ///// <param name="messageFlag">日志的消息类型</param>
        ///// <param name="strFormat">要格式的字符串</param>
        ///// <param name="arg">格式参数</param>
        //public static void Write(MessageType messageFlag, string strFormat)
        //{
        //    bool bIsLock = false;

        //    object[] arg = null;

        //    s_LockLogInfoQueue.EnterWriteLock();
        //    {
        //        s_LogInfoQueue.Enqueue(new LogInfo(messageFlag, strFormat, arg));

        //        // 检测是否有其它的线程已在处理中，如在使用就退出,否则开始锁定
        //        if (s_IsLock == false)
        //        {
        //            bIsLock = s_IsLock = true;
        //        }
        //    }
        //    s_LockLogInfoQueue.ExitWriteLock();

        //    // 如果有其它的线程在处理就退出
        //    if (bIsLock == false)
        //    {
        //        return;
        //    }

        //    LogInfo[] logInfoArray = null;
        //    do
        //    {
        //        logInfoArray = null;

        //        s_LockLogInfoQueue.EnterWriteLock();
        //        {
        //            if (s_LogInfoQueue.Count > 0)
        //            {
        //                logInfoArray = s_LogInfoQueue.ToArray();
        //                s_LogInfoQueue.Clear();
        //            }
        //            else
        //            {
        //                s_IsLock = false; // 没有数据需要处理,释放锁定让其它的程序来继续处理
        //            }
        //        }
        //        s_LockLogInfoQueue.ExitWriteLock();

        //        if (logInfoArray == null)
        //        {
        //            break;
        //        }

        //        for (int iIndex = 0; iIndex < logInfoArray.Length; iIndex++)
        //        {
        //            LogInfo logInfo = logInfoArray[iIndex];

        //            if (logInfo.Parameter == null)
        //            {                        
        //                InternalWrite(logInfo.MessageFlag, logInfo.Format);
        //            }
        //        }
        //    } while (logInfoArray != null);
        //}

        /// <summary>
        /// 输出日志,防止多线程的问题
        /// </summary>
        /// <param name="messageFlag">日志消息的类型</param>
        /// <param name="strFormat">要写入的字符串</param>
        public static void WriteLine(MessageType messageFlag, string strFormat)
        {
            WriteLine(messageFlag, strFormat, null);

            StackTrace st = new StackTrace(true);
            StackFrame sf = st.GetFrame(1);
            Type t = sf.GetMethod().DeclaringType;
            m_ClassFullName = t.ToString() + " " + sf.GetMethod().Name + ":" + sf.GetFileLineNumber() + " ";

            string logWriteFile = null;
            try
            {
                logWriteFile = ConfigurationManager.AppSettings["LogWriteFile"].ToString();
            }
            catch
            {
                logWriteFile = null;
            }

            if (!string.IsNullOrEmpty(logWriteFile))
            {
                if (logWriteFile.ToLower() == "all")
                {
                    m_MessageFile = new MessageFile();

                    m_MessageFile.WriteLine(m_Message + strFormat);
                }
                else if (logWriteFile.ToLower() == MessageType.Debug.ToString().ToLower())
                {
                    m_MessageFile = new MessageFile();

                    m_MessageFile.WriteLine(m_Message + strFormat);
                }
                else if (logWriteFile.ToLower() == "error")
                {
                    if ((messageFlag == MessageType.Error) || (messageFlag == MessageType.FatalError) || (messageFlag == MessageType.Hack))
                    {
                        m_MessageFile = new MessageFile();

                        m_MessageFile.WriteLine(m_Message + m_ClassFullName + " " + strFormat);
                    }
                }
            }
        }

        /// <summary>
        /// 防止多线程的问题
        /// </summary>
        /// <param name="messageFlag">日志的消息类型</param>
        /// <param name="strFormat">要格式的字符串</param>
        /// <param name="arg">格式参数</param>
        public static void WriteLine(MessageType messageFlag, string strFormat, params object[] arg)
        {
            bool bIsLock = false;

            s_LockLogInfoQueue.EnterWriteLock();
            {
                s_LogInfoQueue.Enqueue(new LogInfo(messageFlag, strFormat, arg));

                // 检测是否有其它的线程已在处理中，如在使用就退出,否则开始锁定
                if (s_IsLock == false)
                {
                    bIsLock = s_IsLock = true;
                }
            }
            s_LockLogInfoQueue.ExitWriteLock();

            // 如果有其它的线程在处理就退出
            if (bIsLock == false)
            {
                return;
            }

            LogInfo[] logInfoArray = null;
            do
            {
                logInfoArray = null;

                s_LockLogInfoQueue.EnterWriteLock();
                {
                    if (s_LogInfoQueue.Count > 0)
                    {
                        logInfoArray = s_LogInfoQueue.ToArray();
                        s_LogInfoQueue.Clear();
                    }
                    else
                    {
                        s_IsLock = false; // 没有数据需要处理,释放锁定让其它的程序来继续处理
                    }
                }
                s_LockLogInfoQueue.ExitWriteLock();

                if (logInfoArray == null)
                {
                    break;
                }

                for (int iIndex = 0; iIndex < logInfoArray.Length; iIndex++)
                {
                    LogInfo logInfo = logInfoArray[iIndex];

                    if (logInfo.Parameter == null)
                    {
                        InternalWriteLine(logInfo.MessageFlag, logInfo.Format);
                    }
                    else
                    {
                        InternalWriteLine(logInfo.MessageFlag, logInfo.Format, logInfo.Parameter);
                    }
                }
            } while (logInfoArray != null);
        }
  
        /// <summary>
        /// 输出到日志文件
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <param name="strFormat">要输出的字符串信息</param>
        public static void WriteLineLogFile(string strFileName, string strFormat)
        {
            string strFilter = StringFilter(strFormat);
            if (strFilter != string.Empty)
            {
                using (StreamWriter writer = File.AppendText(strFileName))
                {
                    writer.WriteLine(strFilter);
                }
            }
        }
       
        /// <summary>
        /// 输出到日志文件
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <param name="strFormat">要输出的字符串信息</param>
        /// <param name="msgType">需要要的信息类型</param>
        public static void WriteLineLogFile(string strFileName, string strFormat, params MessageType[] msgType)
        {
            string strFilter = StringFilter(strFormat, msgType);
            if (strFilter != string.Empty)
            {
                using (StreamWriter writer = File.AppendText(strFileName))
                {
                    writer.WriteLine(strFilter);
                }
            }
        }
        /// <summary>
        /// 输出到日志文件
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <param name="strFormat">要输出的字符串</param>
        /// <param name="arg">格式参数</param>
        public static void WriteLineLogFile(string strFileName, string strFormat, params object[] arg)
        {
            WriteLineLogFile(strFileName, string.Format(strFormat, arg));
        }
        #endregion
        
        #region 内部
        /// <summary>
        /// Write 控制台输出,根据消息类型输出不同颜色的提示
        /// </summary>
        /// <param name="messageFlag">日志消息类型</param>
        /// <param name="strFormat">要格式的字符串</param>
        internal static void ColorWriteLine(ConsoleColor messageColor, string strFormat)
        {
            Console.ForegroundColor = messageColor;
            Console.WriteLine(strFormat);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Write 控制台输出,根据消息类型输出不同颜色的提示
        /// </summary>
        /// <param name="messageFlag">日志消息类型</param>
        /// <param name="strFormat">要格式的字符串</param>
        internal static void InternalWrite(MessageType messageFlag, string strFormat)
        {
            switch (messageFlag)
            {
                case MessageType.Init:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    m_Message = "[初始化]：";
                    //Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(m_Message);
                    break;

                case MessageType.None: // 置换打印信息
                    Console.ForegroundColor = ConsoleColor.Gray;
                    m_Message = "";
                    Console.Write(m_Message);
                    break;

                case MessageType.Status:
                    Console.ForegroundColor = ConsoleColor.Green;
                    m_Message = "[状态]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Sql:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    m_Message = "[SQL]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    m_Message = "[信息]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Notice:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    m_Message = "[提示]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    m_Message = "[警告]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Debug:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    m_Message = "[调试]: ";
                    //Console.Write(m_Message);
                    ConsoleWriteLineDebug(m_Message);
                    break;

                case MessageType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    m_Message = "[错误]: ";

                    Console.Write(m_Message);
                    break;

                case MessageType.FatalError:
                    Console.ForegroundColor = ConsoleColor.Red;
                    m_Message = "[致命错误]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Hack:
                    Console.ForegroundColor = ConsoleColor.Red;
                    m_Message = "[黑客]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Load:
                    Console.ForegroundColor = ConsoleColor.Red;
                    m_Message = "[加载]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.DosPrompt:
                    Console.ForegroundColor = ConsoleColor.Green;
                    m_Message = strFormat;
                    s_strDosPrompt = m_Message;
                    break;

                case MessageType.Input:
                    if (s_strDosPrompt != string.Empty)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(s_strDosPrompt);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    s_strInput = strFormat;
                    break;

                default:
                    break;
            }

            Console.ForegroundColor = ConsoleColor.Gray;

            StringBuilder strStringBuilder = new StringBuilder("");

            if (messageFlag != MessageType.DosPrompt)
            {
                int iBlankLength = (s_strDosPrompt.Length + s_strInput.Length) - strFormat.Length; //留空白
                for (int iIndex = 0; iIndex < iBlankLength; iIndex++)
                {
                    strStringBuilder.Append(" ");
                }

                for (int iIndex = 0; iIndex < iBlankLength; iIndex++)
                {
                    strStringBuilder.Append("\b");
                }
            }

            Console.Write(strFormat + strStringBuilder); //输出信息到控制台

            if (messageFlag == MessageType.Load)
            {
                // none
            }
            else if (messageFlag == MessageType.Input)
            {
                // none
            }
            else if (messageFlag == MessageType.DosPrompt)
            {
                if (s_strInput != string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(s_strInput);
                }
            }
            else
            {
                //Console.WriteLine(" ");

                if (s_strDosPrompt != string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(s_strDosPrompt);
                }

                if (s_strInput != string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(s_strInput);
                }
            }
        }

        /// <summary>
        /// 控制台输出,根据消息类型输出不同颜色的提示
        /// </summary>
        /// <param name="messageFlag">日志消息类型</param>
        /// <param name="strFormat">要格式的字符串</param>
        internal static void InternalWriteLine(MessageType messageFlag, string strFormat)
        {
            Console.Write("\r");  

            switch (messageFlag)
            {
                case MessageType.Init:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    m_Message = "[初始化]：";
                    //Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(m_Message);
                    break;

                case MessageType.None: // 置换打印信息
                    Console.ForegroundColor = ConsoleColor.Gray;
                    m_Message = "";
                    Console.Write(m_Message);
                    break;

                case MessageType.Status:
                    Console.ForegroundColor = ConsoleColor.Green;
                    m_Message = "[状态]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Sql:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    m_Message = "[SQL]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    m_Message = "[信息]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Notice:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    m_Message = "[提示]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    m_Message = "[警告]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Debug:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    m_Message = "[调试]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    m_Message = "[错误]: ";

                    Console.Write(m_Message);
                    break;

                case MessageType.FatalError:
                    Console.ForegroundColor = ConsoleColor.Red;
                    m_Message = "[致命错误]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Hack:
                    Console.ForegroundColor = ConsoleColor.Red;
                    m_Message = "[黑客]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.Load:
                    Console.ForegroundColor = ConsoleColor.Red;
                    m_Message = "[加载]: ";
                    Console.Write(m_Message);
                    break;

                case MessageType.DosPrompt:
                    Console.ForegroundColor = ConsoleColor.Green;
                    m_Message = strFormat;
                    s_strDosPrompt = m_Message;
                    break;

                case MessageType.Input:
                    if (s_strDosPrompt != string.Empty)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(s_strDosPrompt);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    s_strInput = strFormat;
                    break;

                default:
                    break;
            }

            if (messageFlag == MessageType.DosPrompt)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (messageFlag == MessageType.Input)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            StringBuilder strStringBuilder = new StringBuilder("");

            if (messageFlag != MessageType.DosPrompt)
            {
                int iBlankLength = (s_strDosPrompt.Length + s_strInput.Length) - strFormat.Length; //留空白
                for (int iIndex = 0; iIndex < iBlankLength; iIndex++)
                {
                    strStringBuilder.Append(" ");
                }

                for (int iIndex = 0; iIndex < iBlankLength; iIndex++)
                {
                    strStringBuilder.Append("\b");
                }
            }

            Console.Write(strFormat + strStringBuilder); //输出信息到控制台

            if (messageFlag == MessageType.Load)
            {
                // none
            }
            else if (messageFlag == MessageType.Input)
            {
                // none
            }
            else if (messageFlag == MessageType.DosPrompt)
            {
                if (s_strInput != string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(s_strInput);
                }
            }
            else
            {
                Console.WriteLine(" ");

                if (s_strDosPrompt != string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(s_strDosPrompt);
                }

                if (s_strInput != string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(s_strInput);
                }
            }
        }

        /// <summary>
        /// 控制台输出,根据消息类型输出不同颜色的提示
        /// </summary>
        /// <param name="messageFlag">日志消息类型</param>
        /// <param name="strFormat">要格式的字符串</param>
        /// <param name="arg">格式参数</param>
        internal static void InternalWriteLine(MessageType messageFlag, string strFormat, params object[] arg)
        {
            InternalWriteLine(messageFlag, string.Format(strFormat, arg));
        }

        /// <summary>
        /// 字符过滤,过滤 LogMessageType.MSG_LOAD 载入信息
        /// </summary>
        /// <param name="strFormat">要过滤得字符串</param>
        /// <returns></returns>
        internal static string StringFilter(string strFormat)
        {
            if (strFormat == null || strFormat == string.Empty)
            {
                return strFormat;
            }

            string strReturn = strFormat;
            int iIndexOf = strFormat.IndexOf("[L"); //寻找 [LOAD]
            if (iIndexOf > 0)
            {
                return string.Empty;
            }
            return strReturn;
        }
        /// <summary>
        /// 字符过滤,过滤制定的LogMessageType 类型
        /// </summary>
        /// <param name="strFormat">要过滤得字符串</param>
        /// <param name="msgType">需要要类型</param>
        /// <returns></returns>
        internal static string StringFilter(string strFormat, params MessageType[] msgType)
        {
            if (strFormat == null || strFormat == string.Empty)
            {
                return strFormat;
            }

            string strReturn = strFormat;
            int iIndexOf = 0; //寻找 相关类型标记
            foreach (MessageType messageFlag in msgType)
            {                
                switch (messageFlag)
                {                        
                    case MessageType.None: //空
                        iIndexOf = strFormat.IndexOf("[NONE]");
                        if (iIndexOf >= 0)
                            return strReturn;
                        break;
                    case MessageType.Status:
                        iIndexOf = strFormat.IndexOf("[ST");
                        if (iIndexOf >= 0)
                            return strReturn;
                        break;
                    case MessageType.Sql:
                        iIndexOf = strFormat.IndexOf("[SQ");
                        if (iIndexOf >= 0)
                            return strReturn;
                        break;
                    case MessageType.Info:
                        iIndexOf = strFormat.IndexOf("[IN");
                        if (iIndexOf >= 0)
                            return strReturn;
                        break;
                    case MessageType.Notice:
                        iIndexOf = strFormat.IndexOf("[NO");
                        if (iIndexOf >= 0)
                            return strReturn;
                        break;
                    case MessageType.Warning:
                        iIndexOf = strFormat.IndexOf("[WA");
                        if (iIndexOf >= 0)
                            return strReturn;
                        break;
                    case MessageType.Debug:
                        iIndexOf = strFormat.IndexOf("[D");
                        if (iIndexOf >= 0)
                            return strReturn;
                        break;
                    case MessageType.Error:
                        iIndexOf = strFormat.IndexOf("[ER");
                        if (iIndexOf >= 0)
                            return strReturn;
                        break;
                    case MessageType.FatalError:
                        iIndexOf = strFormat.IndexOf("[FA");
                        if (iIndexOf >= 0)
                            return strReturn;
                        break;
                    case MessageType.Hack:
                        iIndexOf = strFormat.IndexOf("[HA");
                        if (iIndexOf >= 0)
                            return strReturn;
                        break;
                    case MessageType.Load:
                        iIndexOf = strFormat.IndexOf("[L");
                        if (iIndexOf >= 0)
                            return strReturn;
                        break;
                    case MessageType.DosPrompt:
                        return strReturn;
                        //break;
                    case MessageType.Input:
                        return strReturn;
                        //break;

                    default:
                        return string.Empty;
                        //break;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 字符过滤,过滤 '\r' 回车  '\b' 退格
        /// </summary>
        /// <param name="charFormat"></param>
        /// <returns></returns>
        internal static string StringFilter(char charFormat)
        {
            if (charFormat == '\r' || charFormat == '\b')
            {
                return string.Empty;
            }
            return charFormat.ToString();
        }
        #endregion

        #endregion

        internal static void ConsoleWriteLineDebug(string str)
        {
            StackTrace st = new StackTrace(true);
            string methodName = st.GetFrame(1).GetMethod().Name;
            string className = st.GetFrame(1).GetMethod().DeclaringType.ToString();
            string fullName = className + "." + methodName;
#if DEBUG
            Console.WriteLine(fullName + ":" + str);
#endif
        }
    }
}

