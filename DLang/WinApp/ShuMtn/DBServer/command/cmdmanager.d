module command.cmdmanager;

import std.stdio, shu.command, mainserver;

class CmdManager
{

private:
	static CmdManager m_instance = null;
	Command m_Command = null;

public:
	static CmdManager Instance()
	{
		if(m_instance is null) m_instance = new CmdManager();
		
		return m_instance;
	}

	this()
	{
		m_Command = new Command();
		m_Command.AddRegister("exit", "退出程序", &OnAppExit);
	}

	void start()
	{
		if(m_Command !is null)
		{
			m_Command.Start();
		}
	}

	void stop()
	{
		if(m_Command !is null)
		{
			m_Command.Stop();
			m_Command = null;
		}
	}

private:
	void OnMyXX(string cmd)
	{
		writefln("%s 这家伙是我老婆", cmd);
	}

	void OnAppExit(string cmd)
	{
		MainServer.ExitServer();
	}
}

