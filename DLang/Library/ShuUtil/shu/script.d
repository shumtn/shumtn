module shu.script;

import shu.util.lua.all;
import shu.util.lua.c.all;
import std.conv : toStringz;

/** Lua 脚本
 * 1.获取lua脚本中指定全局对象的值
 * 2.设置lua脚本中指定全局对象的值
 * 3.在lua脚本中可以使用C++程序中的函数
 * 4.在C++程序中可以使用lua脚本中的函数
 * */
class Script
{

private:
	static Script m_instance = null;
	LuaState m_lua = null;
	
public:
	static Script Instance()
	{
		if(m_instance is null) m_instance = new Script();

		return m_instance;
	}

	this()
	{
		open();
	}
	
	~this()
	{
		close();
	}
	
	/** 打开脚本 */
	void open()
	{
		if(m_lua is null) m_lua = new LuaState();
		if(m_lua !is null) m_lua.openLibs();
	}
	
	/** 关闭脚本 */
	void close()
	{
		if(m_lua !is null)
		{
			lua_close(cast(lua_State*)&m_lua);
			m_lua = null;
		}
	}
	
	/** 重置脚本 */
	void reset()
	{
		close();
		open();
	}
	
	LuaFunction loadString(string code)
	{
		return m_lua.loadString(cast(char[])code);
	}

	LuaObject[] doString(string code="print('hello, world! doString')")
	{
		return m_lua.doString(cast(char[])code);
	}
	
	LuaFunction loadFile(string path)
	{
		return m_lua.loadFile(cast(char[])path);
	}
	
	LuaObject[] doFile(string path)
	{
		return m_lua.doFile(cast(char[])path);
	}
	
	// 注册脚本函数 ================
	/** 注册函数到lua脚本 */
	LuaObject RegisterType(T)()
	{
		return m_lua.registerType!T;
	}	

	/** 注册函数到lua脚本 */
	void RegFunction(T, U)(T key, U value)
	{
		m_lua.set(key, value);
		//lua_register(cast(lua_State*)m_lua, toStringz(key), cast(lua_CFunction)value ); // 这个不能不能这么写 
	}
	
	// 获取脚本函数 ================
	/** 使用lua脚本中的函数 */
	LuaFunction GetFunction(U...)(U args)
	{
		return m_lua.get!LuaFunction( args );
	}

	/** 获取状态 */
	LuaState GetState()
	{
		return m_lua;
	}
}

