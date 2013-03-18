using System;
using System.IO;

namespace Shu.Util
{
	public class LuaFunctionApi
	{
		[LuaFunction("lua1")]  
        public void a1()  
        {  
			Console.WriteLine("a1 called");  
        }  
  
        [LuaFunction("lua2")]  
        public int a2()  
        {  
			Console.WriteLine("a2 called");  
            return 0;  
        }  
  
        [LuaFunction("lua3")]  
        public void a3(string s)  
        {  
			Console.WriteLine("a3 called " + s);  
        } 
		
		[LuaFunction("test4")]
		public void test4()
		{
			Console.WriteLine("测试程序");
		}
		
		[LuaFunction("DoFile")]
		public int DoFile(string fileName)
		{
			string name = Path.Combine("Scripts/Lua" , fileName);
			//string name = Path.Combine(ServerCore.BaseDirectory, "Scripts/Lua" , fileName);
			if(File.Exists(name))
			{
				return 0;
			}
			else
			{
				return 1;
			}
		}
		
		[LuaFunction("rfalse")]
		public void rfalse(string name)
		{
			Shu.Util.Message.WriteLine(Shu.Util.MessageType.Init, name);
		}
	}
}

