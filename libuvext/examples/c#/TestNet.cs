using System;
using Shu.Net;
using System.Runtime.InteropServices;

namespace ShuNetClient
{
	public class TestNet
	{
		private static ShuLoad.ConnectCallBack m_OnConnect;
		private static ShuLoad.WriteCallBack m_OnWrite;
		private static ShuLoad.ReadCallBack m_OnRead;
		private static ShuLoad.CloseCallBack m_OnClose;
		private static ShuLoad.ErrorCallBack m_OnError;

		public TestNet ()
		{
			m_OnConnect = new ShuLoad.ConnectCallBack(OnConnect);
			m_OnWrite = new ShuLoad.WriteCallBack(OnWrite);
			m_OnRead = new ShuLoad.ReadCallBack(OnRead);
			m_OnClose = new ShuLoad.CloseCallBack(OnClose);
			m_OnError = new ShuLoad.ErrorCallBack(OnError);

			ShuLoad.net_server_start("0.0.0.0", 6000, 200, 100, 4096, m_OnConnect, m_OnWrite, m_OnRead, m_OnClose, m_OnError);
			//ShuLoad.net_server_create("0.0.0.0", 6000, 20, 10, 4096, m_OnConnect, m_OnWrite, m_OnRead, m_OnClose, m_OnError);
		}

		private void OnConnect(IntPtr ptr)
		{
			channel c;			
			try   
			{   
				c = (channel)Marshal.PtrToStructure(ptr, typeof(channel));
			}   
			finally   
			{   
				//Marshal.FreeHGlobal(ptr);   
			} 
			
			Console.WriteLine ("连接完成 Id=>" + c.id.ToString() + " remoteIp=>" + c.remoteIp + ":" + c.remotePort );
		}
		
		private void OnWrite(IntPtr ptr)
		{
		}
		
		private void OnRead (IntPtr ptr)
		{
			channel c = (channel)Marshal.PtrToStructure(ptr, typeof(channel));
			Console.WriteLine ("连接完成 Id=>" + c.id + " remoteIp=>" + c.remoteIp + ":" + c.remotePort + " size=>" + c.size);
		}

		private void OnClose(IntPtr ptr)
		{
			channel c = (channel)Marshal.PtrToStructure(ptr, typeof(channel));
			Console.WriteLine ("连接完成 Id=>" + c.id + " remoteIp=>" + c.remoteIp + ":" + c.remotePort );
		}
		
		private void OnError(IntPtr ptr)
		{
		}
	}
}

