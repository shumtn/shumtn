using System;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace Shu.Util
{
    public class ConsoleCommand
    {
        #region Callback
        /// <summary>
        /// 菜单显示回调
        /// </summary>
        public delegate void CmdDisplayCallback(string cmd);
        private void OnDataSend(string cmd, CmdDisplayCallback callback)
        {
            callback(cmd);
        }

        /// <summary>
        /// 命令执行回调
        /// </summary>
        public delegate bool CmdExecuteCallback(string cmd);
        private void OnCmdExecute(string cmd, CmdExecuteCallback callback)
        {
            callback(cmd);
        }
        #endregion

        private string m_StrCmd = null;
        public string StrCmd
        {
            get { return m_StrCmd; }
            set { m_StrCmd = value; }
        }

        /// <summary>
        /// 主服务程序是否正在程序关闭中...
        /// </summary>
        private bool s_Closing = false;
        /// <summary>
        /// 主服务程序是否正在程序关闭中...
        /// </summary>
        public bool Closing
        {
            get { return s_Closing; }
            set { return; }
        }

        private string m_Developers = null;

        private CmdDisplayCallback MyOnCmdDisplay;

        private CmdExecuteCallback MyOnCmdExecute;

        /// <summary>
        /// 控制台命令
        /// </summary>
        /// <param name="cmdDisplay">显示回调</param>
        /// <param name="cmdExecute">执行回调</param>
        public ConsoleCommand(CmdDisplayCallback cmdDisplay, CmdExecuteCallback cmdExecute)
        {
            MyOnCmdDisplay = cmdDisplay;
            MyOnCmdExecute = cmdExecute;
        }

        /// <summary>
        /// 初始化 服务器 命令
        /// </summary>
        /// <param name="promptName">提示符名称</param>
        /// <param name="developersInfo">开发者信息 "☆☆☆蜀山技术研究团队 ☆ 服务器端组☆☆☆";//"网关服务器:戴君洁 联系QQ:21995346 联系电话:15984763535";</param>
        public void Start(string promptName = "Server", string developers = "☆☆☆蜀山技术研究团队 ☆ 服务器端组☆☆☆")
        {
            m_Developers = developers;

            while (!s_Closing)
            {
                StringBuilder sb = new StringBuilder();

                while (true)
                {
                    Message.WriteLine(MessageType.DosPrompt, promptName);

                    // 再次检测程序已经运行
                    if (s_Closing == true)
                    {
                        break;
                    }

                    m_StrCmd = Console.ReadLine();

                    if (string.IsNullOrEmpty(m_StrCmd))
                    {
                        // 如果输入的键值不为空
                        if (m_StrCmd != string.Empty)
                        {
                            Message.WriteLine(MessageType.Input, string.Empty);
                            break;
                        }
                        else
                        {
                            ///显示命令
                            CmdDisplay(null);
                            continue;
                        }
                    }

                    ///执行命令
                    CmdExecute(m_StrCmd);
                }
            }
        }

        /// <summary>
        /// 显示 命令
        /// </summary>
        /// <param name="cmd">命令</param>
        private void CmdDisplay(string cmd)
        {
            if (string.IsNullOrEmpty(cmd))
            {
                cmd = "回车";
            }

            Message.WriteLine(MessageType.Notice, "命令({0}) : Help - 命令帮助 命令不区分大小写", cmd);
            {
                Message.WriteLine(MessageType.Notice, "=======================================================");
                Message.WriteLine(MessageType.Notice, "=                       主命令                        =");
                Message.WriteLine(MessageType.Notice, "=======================================================");
                Message.WriteLine(MessageType.Notice, "=                                                     =");
                Message.WriteLine(MessageType.Notice, "=        命令 : ? -Admin   => 显示所有命令            =");
                Message.WriteLine(MessageType.Notice, "=                                                     =");
                Message.WriteLine(MessageType.Notice, "=        命令 : Cls -Cls   => 清屏                    =");
                Message.WriteLine(MessageType.Notice, "=                                                     =");
                Message.WriteLine(MessageType.Notice, "=        命令 : Exit -Exit => 退出程序                =");
                Message.WriteLine(MessageType.Notice, "=                                                     =");
                Message.WriteLine(MessageType.Notice, "=======================================================");
                MyOnCmdDisplay(cmd);
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmd">命令</param>
        private void CmdExecute(string cmd)
        {
            switch (cmd)
            {
                case "?":
                case "-admin":
                    CmdDisplay(cmd);
                    break;
                case "cls":
                    Console.Clear();
                    ServerMainInfo();
                    break;
                case "exit":
                case "-exit":
                    Message.WriteLine(MessageType.Notice, "命令({0}) : Exit - 退出程序", cmd);
                    {
                        s_Closing = true;
                    }
                    break;
                default:
                    {
                        bool bIsDisposal = false;

                        bIsDisposal = MyOnCmdExecute(cmd);

                        if (m_StrCmd != string.Empty && bIsDisposal == false)
                        {
                            Message.WriteLine(MessageType.Warning, "命令({0}) : 未知的无效命令", cmd);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 显示服务器信息
        /// </summary>
        /// <param name="developersInfo"></param>
        private void ServerMainInfo()
        {
            //显示 管理者 信息
            Message.WriteLine(MessageType.Info, m_Developers);

            AppInfo.InitServerMainInfo();
            //AppInfo.InitServerIsRun();
            AppInfo.DisplayServerMainInfo();
        }
    }
}
