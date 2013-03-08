using System;
using System.Runtime.InteropServices;

namespace Shu.Net
{
	public struct channel
	{
		public int		id;				// 编号
		[MarshalAs(UnmanagedType.LPStr)]
		public string	localIp;		// 本地地址
		public int 		localPort;		// 本地端口

		[MarshalAs(UnmanagedType.LPStr)]
		public string 	remoteIp;		// 远程地址
		public int 		remotePort;		// 远程端口
		
		public int		size;			// 数据长度
		//[MarshalAs(UnmanagedType.LPArray, SizeConst=256)
		//[MarshalAs(UnmanagedType.LPStr)]
		public IntPtr	data;			// 数据
		public IntPtr	handle;			// 缓冲指针
	}

	public static class ShuLoad
	{
		/// <summary>
		/// 连接
		/// </summary>
		/// <param name="channel">事件对象</param>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ConnectCallBack(IntPtr channel);
		
		/// <summary>
		/// 写入
		/// </summary>
		/// <param name="channel">事件对象</param>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void WriteCallBack(IntPtr channel);		
		
		/// <summary>
		/// 读取
		/// </summary>
		/// <param name="channel">事件对象</param>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ReadCallBack(IntPtr channel);
		
		/// <summary>
		/// 关闭
		/// </summary>
		/// <param name="channel">事件对象</param>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void CloseCallBack(IntPtr channel);
		
		/// <summary>
		/// 错位
		/// </summary>
		/// <param name="channel">事件对象</param>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ErrorCallBack(IntPtr channel);

		[DllImport("ShuNet", CallingConvention = CallingConvention.Cdecl)]
		public static extern void net_server_start(string ip, ushort port, ushort backlog, int maxConnects, int bufferSize=4096, ConnectCallBack connect_cb=null, WriteCallBack write_cb=null, ReadCallBack read_cb=null, CloseCallBack close_cb=null, ErrorCallBack error_cb=null);

		[DllImport("ShuNet", CallingConvention = CallingConvention.Cdecl)]
		public static extern void net_server_send(IntPtr bev, IntPtr data, int length);

		[DllImport("ShuNet", CallingConvention = CallingConvention.Cdecl)]
		public static extern void net_client_start(string ip, int port, int bufferSize=4096, ConnectCallBack connect_cb=null, WriteCallBack write_cb=null, ReadCallBack read_cb=null, CloseCallBack close_cb=null, ErrorCallBack error_cb=null);

		[DllImport("ShuNet", CallingConvention = CallingConvention.Cdecl)]
		public static extern void net_client_send(IntPtr bev, IntPtr data, int length);
	


		[DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
		public static extern void net_server_create(string ip, int port, int backlog, int maxConnects, int bufferSize=4096, ConnectCallBack connect_cb=null, WriteCallBack write_cb=null, ReadCallBack read_cb=null, CloseCallBack close_cb=null, ErrorCallBack error_cb=null);
		
		[DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
		public static extern void net_server_write(IntPtr bev, IntPtr data, int length);
	}



	/*public class Shu
	{
		//函数指针结构在cs中的声明
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private class Dreamdu_Struct
		{
			public Dreamdu_PGetIP GetIP;
			public Monkey_PGetName GetName;
		}
		
		private static IntPtr hModule = IntPtr.Zero;
		private static GetDreamduFuns dreamduStruct = null;
		private static Dreamdu_Struct func = new Dreamdu_Struct();
		private static IntPtr pcon = IntPtr.Zero;
		
		//加载dll
		[DllImport("kernel32.dll")]
		private static extern IntPtr LoadLibrary(string lpFileName); 
	
		//获得函数指针的地址
		[DllImport("kernel32.dll")]
		//private static extern GetDreamduFuns GetDreamduAddress(IntPtr hModule, string lpProcName);
		private static extern GetDreamduFuns GetProcAddress(IntPtr hModule, string lpProcName);
	
		//释放dll
		[DllImport("kernel32", EntryPoint = "FreeLibrary", SetLastError = true)]
		private static extern bool FreeLibrary(IntPtr hModule);
	
		private delegate void GetDreamduFuns(IntPtr funs);
		private delegate bool Dreamdu_PGetIP(string name);
		private delegate void Monkey_PGetName(string name);
	
		
	
		//装载 Dll
		public static void LoadDll(string lpFileName)
		{
			hModule = LoadLibrary(lpFileName);
			if (hModule == IntPtr.Zero)
			throw (new Exception());
		}
	
		//获得函数指针
		public static void LoadFun(string lpProcName)
		{
			if (hModule == IntPtr.Zero)
			throw (new Exception(""));
			dreamduStruct = GetProcAddress(hModule, lpProcName);
			//if (farProc == null)
			//throw (new Exception(""));
	
			try
			{
				pcon = Marshal.AllocHGlobal(Marshal.SizeOf(func));
				Marshal.StructureToPtr(func, pcon, true);
				dreamduStruct(pcon);
				Marshal.PtrToStructure(pcon, func);
			}
			finally
			{
				Marshal.FreeHGlobal(pcon);
			}
		}
	
		//卸载 Dll
		public static void UnLoadDll()
		{
			bool ret = FreeLibrary(hModule);
			hModule = IntPtr.Zero;
			dreamduStruct = null;
		}
	
		public static bool GetIP(string name)
		{
			return func.GetIP(name);
		}
	
		public static void GetName(string name)
		{
			func.GetName(name);
			//return func.GetName(name);
		}
	}*/
}

