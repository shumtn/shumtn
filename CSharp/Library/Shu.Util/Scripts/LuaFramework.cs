using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using LuaInterface;

namespace Shu.Util
{
	/// <summary>  
    /// Lua函数描述特性类  
    /// </summary>  
    public class LuaFunction : Attribute  
    {  
        private string m_FunctionName;  
  
		/// <summary>
		/// Lua属性构造
		/// </summary>
		/// <param name='functionName'>函数名称</param>
        public LuaFunction(string functionName)  
        {  
            m_FunctionName = functionName;  
        }
  
		/// <summary>
		/// 函数名称
		/// </summary>
        public string FunctionName()  
        {  
            return m_FunctionName;  
        }  
    } 
	
	/// <summary>  
    /// Lua引擎  
    /// </summary>  
    public class LuaFramework  
    {  
		/// <summary>
		/// lua虚拟机
		/// </summary>
        private Lua m_Lua = null;

        public LuaFramework()
        {
            if(m_Lua == null) m_Lua = new Lua();
        }
		
		/// <summary>  
        /// 注册lua函数  
        /// </summary>  
        /// <param name="luaFunctionClass">lua函数类</param>  
        public void RegisterFunction( object luaFunctionClass )  
        {  
            foreach (MethodInfo mInfo in luaFunctionClass.GetType().GetMethods())  
            {  
                foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))  
                {
					if(attr is LuaFunction)
					{
						string luaFunctionName = (attr as LuaFunction).FunctionName();
						m_Lua.RegisterFunction(luaFunctionName, luaFunctionClass, mInfo); 
					}
                }  
            }  
        }
		
		/// <summary>  
        /// 注册lua函数  
        /// </summary>
        /// <param name="luaFunctionName">函数名称</param>
        /// <param name="luaFunctionClass">lua函数类</param>
		public void RegisterFunction(string luaFunctionName, object luaFunctionClass )  
        {
			foreach (MethodInfo mInfo in luaFunctionClass.GetType().GetMethods())  
            {
				m_Lua.RegisterFunction(luaFunctionName, luaFunctionClass, mInfo);
			}
		}
  
        /// <summary>  
        /// 执行lua脚本文件  
        /// </summary>  
        /// <param name="luaFileName">脚本文件名</param>  
        public void ExecuteFile(string luaFileName)  
        {  
            try  
            {  
                m_Lua.DoFile(luaFileName);  
            }  
            catch (Exception e)  
            {  
				Console.WriteLine(e.ToString()); 
            }  
        }  
  
        /// <summary>  
        /// 执行lua脚本  
        /// </summary>  
        /// <param name="luaCommand">lua指令</param>  
        public void ExecuteString(string luaCommand)  
        {  
            try  
            {  
                m_Lua.DoString(luaCommand);
            }  
            catch (Exception e)  
            {  
				Console.WriteLine(e.ToString()); 
            }  
        }
		
		public string[] CacheFiles()
		{
			List<string> list = new List<string>();

			GetScripts( list, Path.Combine( Shu.Util.Core.BaseDirectory, "Scripts/Lua" ), "*.lua" );
			
			for(int i=0;i<list.Count;i++)
			{
				Console.WriteLine("加载脚本=>" + list[i]);
			}			
			return list.ToArray();
		}

		public static void GetScripts( List<string> list, string path, string filter )
		{
			string oldPath = Path.Combine( Shu.Util.Core.BaseDirectory, "Scripts/Lua" );
			//Debug.Show(path);
			
			foreach( string dir in Directory.GetDirectories( path ) )
				GetScripts( list, dir, filter );

			//list.AddRange( Directory.GetFiles( path, filter ) );
			
			string[] files = Directory.GetFiles( path, filter );
			
			for(int i=0;i<files.Length;i++)
			{
				string file = files[i];
				//Debug.Show(file);
				file = file.Replace(oldPath,"").Remove(0,1);
				list.Add(file);
			}
		}
    }
}

