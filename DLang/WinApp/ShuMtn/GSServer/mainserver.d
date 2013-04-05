module mainserver;

import std.stdio;
import command.CmdManager, scripts.ScriptManager, network.NetManager;

class MainServer
{

public:
	this()
	{

	}

	static bool InitServer(string[] args)
	{
		return true;
	}

	static bool StarServer(string[] args)
	{
		// 初始化命令
		CmdManager.Instance().start();
		
		// 初始化脚本
		//ScriptManager.Instance();
		
		// 初始化网络
		NetManager.Instance().start();

		return true;
	}

	static ExitServer()
	{
		NetManager.Instance().stop();
		CmdManager.Instance().stop();
	}
}

