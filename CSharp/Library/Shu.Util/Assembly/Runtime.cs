using System;

namespace Shu.Util
{
	public class Runtime
	{
		public Runtime ()
		{
			
		}
		
		/// <summary>
		/// 判断运行框架是否是Mono
		/// </summary>
		public static bool MonoRuntime()
		{			
			var monoRuntimeType = Type.GetType("Mono.Runtime", false);
			if(monoRuntimeType != null)
				return true;
			else
				return false;
		}
		
		/// <summary>
		/// 显示运行框架版本
		/// </summary>
		public static string RuntimeVersion()
		{
			#if MONO
			#else
			#endif
			
			string version = null;
			if(MonoRuntime())
				version = string.Format("核心: 运行的Mono框架版本 {0}.{1}.{2}", Environment.Version.Major, Environment.Version.Minor, Environment.Version.Build);
			else
				version = string.Format("核心: 运行.NET框架版本 {0}", Environment.Version.ToString());
			
			return version;
		}
	}
}

