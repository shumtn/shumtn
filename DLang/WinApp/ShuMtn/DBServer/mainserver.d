module mainserver;

import std.stdio;
import command.cmdmanager, network.netmanager;

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

	static bool StarServer()
	{
		// 初始化网络
		NetManager.Instance().start();

		// 初始化命令
		CmdManager.Instance().start();

		return true;
	}

	static ExitServer()
	{
		NetManager.Instance().stop();
		CmdManager.Instance().stop();
	}
}

