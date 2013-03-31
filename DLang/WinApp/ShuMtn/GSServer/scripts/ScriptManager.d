module scripts.ScriptManager;

import std.stdio, shu.script, scripts.LuaRegFunction;

class ScriptManager
{

private:
	static ScriptManager m_instance = null;

public:
	static ScriptManager Instance()
	{
		if(m_instance is null) m_instance = new ScriptManager();

		return m_instance;
	}

	this()
	{
		writefln("==============Lua============");

		InitFunction();

		writefln("");

		InitLuaFile();
	}

	void RegFunction(T, U)(T key, U value)
	{
		Script.Instance().RegFunction(key, value);
	}

	void GetFunction(U...)(U args)
	{
		Script.Instance().GetFunction(args);
	}

private:
	void AddRegister(T, U)(T key, U value)
	{
		RegFunction(key, value);
		
		writefln(".." ~ key);
	}

	void InitFunction()
	{
		writefln("------Start RegFunction------");

		AddRegister("MyAdd", &MyAdd);
		AddRegister("MyPrintf1", &MyPrintf1);
		AddRegister("MyPrintf2", &MyPrintf2);

		AddRegister("DoFile", &DoFile);
		AddRegister("rfalse", &rfalse);

		
		//ScriptManager.GetInit().DoString();
		//ScriptManager.GetInit().DoString("print('显示中文看看')");
		//auto p = ScriptManager.GetInit().Lua.GetFunction("print");
		//p("haha");
		
		
		//ScriptManager.GetInit().Lua.RegFunction("MyPrintf", &MyPrintf5);
		//ScriptManager.GetInit().Lua.doFile("testlua.lua");
		//ScriptManager.GetInit().Lua.doString("MyPrintf('test test')");
		//ScriptManager.GetInit().Lua.RegFunction("MyAdd", &MyAdd);
		//ScriptManager.GetInit().Lua.doString("print(MyAdd(5, 4))");

		writefln("------RegFunction   End------");
	}

	void InitLuaFile()
	{
		writefln("------Start LoadLuaFile------");
		Script.Instance().doFile("Script/init.lua");
		writefln("------LoadLuaFile   End------");
	}
}

