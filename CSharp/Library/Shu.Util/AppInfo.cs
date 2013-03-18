using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Shu.Util
{
    public class AppInfo
    {
        #region 属性
        /// <summary>
        /// 是否是64位系统
        /// </summary>
        public static readonly bool Is64Bit = (IntPtr.Size == 8);

        private static Version s_Version = new Version();
        /// <summary>
        /// 服务程序的版本号
        /// </summary>
        public static Version Version
        {
            get { return s_Version; }
        }

        private static bool s_MultiProcessor = false;
        /// <summary>
        /// 硬件是否是多处理器
        /// </summary>
        public static bool MultiProcessor
        {
            get { return s_MultiProcessor; }
        }

        private static int s_ProcessorCount = 1;
        /// <summary>
        /// 硬件处理器的数量
        /// </summary>
        public static int ProcessorCount
        {
            get { return s_ProcessorCount; }
        }

        private static string s_ExePath = string.Empty;
        /// <summary>
        /// 主服务程序的全部路径
        /// </summary>
        public static string ExePath
        {
            get { return s_ExePath; }
        }

        private static string s_BaseDirectory = string.Empty;
        /// <summary>
        /// 主服务程序的路径
        /// </summary>
        public static string BaseDirectory
        {
            get { return s_BaseDirectory; }
        }

        private static Assembly s_Assembly = null;
        /// <summary>
        /// 主服务程序集
        /// </summary>
        public static Assembly Assembly
        {
            get { return s_Assembly; }
        }

        private static Process s_Process = Process.GetCurrentProcess();
        /// <summary>
        /// 主服务程序的本地系统进程
        /// </summary>
        public static Process Process
        {
            get { return s_Process; }
        }

        private static Thread s_Thread = Thread.CurrentThread;
        /// <summary>
        /// 主服务程序的主核心线程
        /// </summary>
        public static Thread Thread
        {
            get { return s_Thread; }
        }
        #endregion


        /// <summary>
        /// 1) 初始化程序的主要信息(核心(主)线程,进程,程序域,程序目录)
        /// </summary>
        public static void InitServerMainInfo()
        {
            s_Process = Process.GetCurrentProcess(); //当前进程
            s_Assembly = Assembly.GetEntryAssembly();//当前程序集
            s_Version = s_Assembly.GetName().Version;//当前服务器版本

            s_ExePath = s_Assembly.Location;        //当前服务器exe路径
            s_BaseDirectory = s_ExePath;            //设置exe路径为基本目录
            if (s_BaseDirectory.Length > 0)
            {
                s_BaseDirectory = Path.GetDirectoryName(s_BaseDirectory);    //获取目录名
                Directory.SetCurrentDirectory(BaseDirectory);                //设置当前应用程序的工作目录为BaseDirectory
            }
            //s_Thread = Thread.CurrentThread;                                 //当前主线程
            //if (s_Thread != null)
            //{
            //    s_Thread.Name = "当前登陆服务器主线程";
            //}
            s_ProcessorCount = Environment.ProcessorCount;                   //当前计算机的处理器数
            if (s_ProcessorCount > 1)
            {
                s_MultiProcessor = true;                                     //是否多处理器
            }
        }

        /// <summary>
        /// 2) 检测程序实例是否在运行
        /// </summary>
        public static void InitServerIsRun()
        {
            //通过基本目录名,获取和操作与同一可执行文件关联的所有进程
            Process[] processArray = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(s_ExePath));
            while (processArray.Length > 1)
            {
                Message.WriteLine(MessageType.Warning, "当前程序已经运行");

                if (processArray[0].Id == Process.GetCurrentProcess().Id)  //与当前进程id不一样的就退出
                {
                    processArray[1].WaitForExit();
                }
                else
                {
                    processArray[0].WaitForExit();
                }

                processArray = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(s_ExePath));
                if (processArray.Length == 1)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 5) 程序详细内容的信息显示
        /// </summary>
        public static void DisplayServerMainInfo()
        {
            ////显示 管理者 信息
            //Message.WriteLine(MessageType.Init, GateWay.GateWayDevelopers);

            //显示 服务器 信息
            string serverInfo = string.Format("版本: 主版本 {0}.{1}, 编译版本 {2}.{3}", Version.Major, Version.Minor, Version.Build, Version.Revision);
            Message.WriteLine(MessageType.Info, serverInfo);

            string version = null;

            OperatingSystem os = Environment.OSVersion;
            string text = string.Empty;
            switch (os.Platform)
            {
                case PlatformID.Win32Windows:
                    switch (os.Version.Minor)
                    {
                        case 0:
                            text = "Microsoft Windows 95";
                            break;
                        case 10:
                            text = "Microsoft Windows 98";
                            break;
                        case 90:
                            text = "Microsoft Windows Millennium Edition";
                            break;
                        default:
                            text = "Microsoft Windows 95 or later";
                            break;
                    }
                    break;
                case PlatformID.Win32NT:
                    switch (os.Version.Major)
                    {
                        case 3:
                            text = "Microsoft Windows NT 3.51";
                            break;
                        case 4:
                            text = "Microsoft Windows NT 4.0";
                            break;
                        case 5:
                            switch (os.Version.Minor)
                            {
                                case 0:
                                    text = "Microsoft Windows 2000";
                                    break;
                                case 1:
                                    text = "Microsoft Windows XP";
                                    break;
                                case 2:
                                    text = "Microsoft Windows 2003";
                                    break;
                                default:
                                    text = "Microsoft NT 5.x";
                                    break;
                            }
                            break;
                        case 6:
                            text = "Microsoft Windows Vista or 2008 Server";
                            break;
                    }
                    break;
                default:
                    if ((int)os.Platform > 3)
                    {
                        string name = "/proc/version";
                        if (File.Exists(name))
                        {
                            using (StreamReader reader = new StreamReader(name))
                            {
                                text = reader.ReadToEnd().Trim();
                            }
                        }
                    }
                    break;
            }

            ////显示 服务器 运行 平台
            //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            //{
            //    Message.WriteLine(MessageType.Info, "核心: 运行系统 " + Environment.OSVersion.Platform);
            //}
            //else if (Environment.OSVersion.Platform == PlatformID.Unix)
            //{
            //    Message.WriteLine(MessageType.Info, "核心: 运行系统 " + Environment.OSVersion.Platform);
            //}

            ////显示 服务器 运行 操作系统
            //text = string.Format("{0} -- {1}", text, os.ToString());
            //Message.WriteLine(MessageType.Info, "核心: 运行系统 " + text);


            //显示 .NET 框架 版本
#if !Framework_3_5
                version = string.Format("核心: 运行.NET框架版本 {0}", Environment.Version.ToString());
#else
                version = string.Format("核心: 运行的Mono框架版本 {0}.{1}.{2}  {3}", Environment.Version.Major, Environment.Version.Minor, Environment.Version.Build, Environment.OSVersion);
#endif

#if MONO
                version = string.Format("核心: 运行的Mono框架版本 {0}.{1}.{2}", Environment.Version.Major, Environment.Version.Minor, Environment.Version.Build);
#endif
            Message.WriteLine(MessageType.Info, version);


            //显示 服务器CPU 情况
            if (s_MultiProcessor || Is64Bit)
            {
                string multiProcessor = string.Format("核心: 处理器" + s_ProcessorCount + "个 -优化在<{0}> " + Environment.OSVersion.Platform, Is64Bit ? "64-系统" : "32-系统");
                Message.WriteLine(MessageType.Info, multiProcessor);
            }
        }
    }
}
