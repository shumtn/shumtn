using System;
using System.Runtime.InteropServices;

namespace Shu.Util
{
    public static class ConsoleWindow
    {
        #region 引入DLL接口
        /// <summary>
        /// 获取控制台窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public extern static IntPtr GetConsoleWindow();

        /// <summary>
        /// 获取系统菜单
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="bRevert"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        /// <summary>
        /// 移除控制台菜单某功能
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="iPos"></param>
        /// <param name="iFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static int RemoveMenu(IntPtr hMenu, int iPos, int iFlags);

        /// <summary>
        /// 重绘控制台菜单栏
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static bool DrawMenuBar(IntPtr hWnd);


        /// <summary>
        /// 查找窗体
        /// </summary>
        /// <param name="lpClassName">类名称</param>
        /// <param name="lpWindowName">窗体名称(Title)</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]   //找子窗体   
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]   //用于发送信息给窗体   
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        [DllImport("MessageDLL.dll", EntryPoint = "StartSendMessage")]
        public extern static void StartSendMessage(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(HandleRef hWnd);

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="hWnd">窗体</param>
        /// <param name="type">显示窗体参数</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "ShowWindow")]   //
        public static extern bool ShowWindow(IntPtr hWnd, int type);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        #endregion

        #region 方法构造

        /// <summary>
        /// 去除关闭按扭
        /// </summary>
        public static void RemoveCloseMenu()
        {
            const int SC_CLOSE = 0xF060;
            IntPtr closeMenu = GetSystemMenu(GetConsoleWindow(), IntPtr.Zero);
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
            DrawMenuBar(ConsoleWindow.GetConsoleWindow());
        }

        /// <summary>
        /// 去除关闭按扭
        /// </summary>
        public static void RemoveCloseMenu(IntPtr hwnd)
        {
            const int SC_CLOSE = 0xF060;
            IntPtr closeMenu = GetSystemMenu(hwnd, IntPtr.Zero);
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
            DrawMenuBar(hwnd);
        }


        /// <summary>
        /// 显示或隐藏窗体
        /// </summary>
        /// <param name="title"></param>
        public static void ShowOrHideWindow(string title)
        {
            Console.Title = title;
            IntPtr ParenthWnd = new IntPtr(0);
            ParenthWnd = FindWindow(null, title);
            ConsoleKeyInfo info;
            while (true)
            {
                info = Console.ReadKey(true);

                if (info.Key == ConsoleKey.S)
                {
                    ShowWindow(ParenthWnd, 8);//显示本dos窗体
                }
                else if (info.Key == ConsoleKey.H)
                {
                    ShowWindow(ParenthWnd, 0);//隐藏本dos窗体                    
                }
                else
                {
                    break;
                }

                //SetFocus(ParenthWnd);
                bool p = SetForegroundWindow(ParenthWnd);
            }
        }
        #endregion
    }



    class ConsoleWindow11
    {
        /// <summary>
        /// 控制台句柄
        /// </summary>
        //public static IntPtr ConsoleHwnd = GetConsoleWindow();

        #region 引入DLL接口
        /// <summary>
        /// 获取控制台窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public extern static IntPtr GetConsoleWindow();

        /// <summary>
        /// 获取系统菜单
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="bRevert"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        /// <summary>
        /// 移除控制台菜单某功能
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="iPos"></param>
        /// <param name="iFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static int RemoveMenu(IntPtr hMenu, int iPos, int iFlags);

        /// <summary>
        /// 重绘控制台菜单栏
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static bool DrawMenuBar(IntPtr hWnd);
        #endregion

        //#region 方法构造

        //#region 私有静态成员变量
        ///// <summary>
        ///// 只锁定添加操作
        ///// </summary>
        //private static SpinLock s_OnlyLockAddRemove = new SpinLock();
        //#endregion
        //#endregion

        /// <summary>
        /// 去除关闭按扭
        /// </summary>
        public static void RemoveSystemCloseMenu()
        {
            const int SC_CLOSE = 0xF060;
            IntPtr closeMenu = GetSystemMenu(GetConsoleWindow(), IntPtr.Zero);
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
            DrawMenuBar(ConsoleWindow.GetConsoleWindow());
        }

        /// <summary>
        /// 去除关闭按扭
        /// </summary>
        public static void RemoveSystemCloseMenu(IntPtr hwnd)
        {
            const int SC_CLOSE = 0xF060;
            IntPtr closeMenu = GetSystemMenu(hwnd, IntPtr.Zero);
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
            DrawMenuBar(hwnd);
        }
    }
}