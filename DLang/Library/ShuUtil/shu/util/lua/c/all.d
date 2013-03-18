/++
Convenience module to import the Lua 5.1 C API.
This module also exposes luaL_tolstring which works like the function with the same name in Lua 5.2.
See_Also:
	Documentation for this API can be found $(LINK2 http://www.lua.org/manual/5.1/manual.html,here).
+/
module shu.util.lua.c.all;

public import shu.util.lua.c.lua, shu.util.lua.c.lauxlib, shu.util.lua.c.lualib, shu.util.lua.c.tostring;