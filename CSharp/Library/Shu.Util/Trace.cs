using System;
using System.Diagnostics;
using System.Reflection;

namespace Shu.Util
{
    public class Trace
    {
        public static void WriteLine(string str, bool isShow=false)
        {
            StackTrace st = new StackTrace(true);
			MethodBase method = st.GetFrame(1).GetMethod();
            string methodName = method.Name;
			string minClassName = method.ReflectedType.Name;
			string maxClassName = method.DeclaringType.ToString();
            string fullName = maxClassName + "." + methodName;
#if DEBUG
			Console.WriteLine(fullName + ":\n\t" + str);
#endif
			Debug.WriteLine(minClassName + "." + methodName + ":" + str);
        }
		
		/// <summary>
		/// 打印异常
		/// </summary>
        public static void WriteLine(Exception ex)
        {
			WriteLine(ex.Message);
        }
    }
}
