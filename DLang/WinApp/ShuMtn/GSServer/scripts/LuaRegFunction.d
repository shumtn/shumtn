module scripts.LuaRegFunction;

import std.stdio, std.conv, shu.util.lua.all, shu.util.lua.c.all, shu.script;

extern(C):

int MyAdd(lua_State* L)
{
	double x = lua_tonumber(L, 1);
	double y = lua_tonumber(L, 2);
	//lua_pushnumber(L, x);
	//lua_pushnumber(L, y);
	lua_pushnumber(L, x + y);
	
	return 3;     //结果返回3个
}

void MyPrintf1(string str)
{
	writefln("lua=>%s", str);
}

void MyPrintf2(string str)
{
	writefln("lua=>%s", str);
}

int DoFile(lua_State* L)
{
	string fileName = to!string(lua_tostring(L, 1));
	if(fileName is null) return 0;

	//luaL_loadbuffer
	//Script.Instance().doFile("Script/" ~ fileName);
	///rfalse(fileName);

	Script.Instance().loadFile("Script/" ~ fileName);

	int nRet = 0;//g_Script.DoFile(str);
	lua_pushnumber(L, nRet);
	
	return 1;
}

void rfalse(string str)
{
	//writefln(str);
	auto p = Script.Instance().GetFunction("print");
	p(str);
}
