module shu.command;

import std.stdio, std.conv, std.string, core.thread;
import shu.consoled;

alias void function(string cmd) CmdFunctionCallback;
alias void delegate(string cmd) CmdDelegateCallback;
enum CallBackType
{
	Function,
	Delegate
}

class CommandModle
{
	string Name;
	string Directions;
	CallBackType CallBack;
	CmdFunctionCallback	FunctionCallBack;
	CmdDelegateCallback DelegateCallBack;

	this()
	{

	}
}

class Command
{

private:
	Thread m_Thread = null;
	string m_PromptName = null;
	string m_Developers = null;
	static bool m_Closing = false;
	string m_StrCmd = null;
	CommandModle[string] m_Cmmands;

	void runStart()
	{
		while(!m_Closing)
		{
			while(true)
			{
				// 再次检测程序已经运行
				if(m_Closing == true) break;
				
				m_StrCmd = strip(readln().toLower());
				
				// 如果输入的键值不为空
				if (m_StrCmd !is null && m_StrCmd != "")
				{
					//CmdExecute(m_StrCmd);
					if(m_StrCmd in m_Cmmands)
					{
						auto cmd = m_Cmmands[m_StrCmd];
						if(cmd.CallBack == CallBackType.Function)
							cmd.FunctionCallBack(m_StrCmd);
						else
							cmd.DelegateCallBack(m_StrCmd);
					}
					else
					{
						CmdError(m_StrCmd);
					}
					break;
				}
				else
				{
					///显示命令
					CmdDisplay(null);
					//continue;
					break;
				}
				
				///执行命令
				write(m_PromptName);
			}
		}
	}

public:
	this()
	{
		AddRegister("admin", "GM命令", &CmdDisplay);
		AddRegister("help", "现实所有命令", &CmdDisplay);
		AddRegister("cls", "清屏", &ClearScreen);
		//AddRegister("exit", "退出程序", &AppExit);
		m_Thread = new Thread(&runStart);
	}

	void AddRegister(string name, string desc, CmdFunctionCallback fn)
	{
		if(name in m_Cmmands)
		{
			writefln("Cmd %s Register Error, %s is existing!", name, name);
		}
		else
		{
			auto cmd = new CommandModle();
			cmd.Name = name;
			cmd.Directions = desc;
			cmd.CallBack = CallBackType.Function;
			cmd.FunctionCallBack = fn;
			m_Cmmands[cmd.Name] = cmd;
		}
	}

	void AddRegister(string name, string desc, CmdDelegateCallback dg)
	{
		if(name in m_Cmmands)
		{
			writefln("Cmd %s Register Error, %s is existing!", name, name);
		}
		else
		{
			auto cmd = new CommandModle();
			cmd.Name = name;
			cmd.Directions = desc;
			cmd.CallBack = CallBackType.Delegate;
			cmd.DelegateCallBack = dg;
			m_Cmmands[cmd.Name] = cmd;
		}
	}

	void Start(string promptName = "Server=>", string developers = "☆☆☆蜀山技术研究团队 ☆ 服务器端组☆☆☆")
	{
		m_PromptName = promptName;
		m_Developers = developers;

		writefln(m_Developers);
		
		if(m_Thread !is null) m_Thread.start();
	}

	void Stop()
	{
		m_Closing = true;
	}
	

private:
	/// <summary>
	/// 显示 命令
	/// </summary>
	/// <param name="cmd">命令</param>
	void CmdDisplay(string cmd)
	{
		if(m_StrCmd is null || m_StrCmd == "")
		{
			cmd = "Enter";
		}

		writefln("Command("~ cmd ~") : Help - 命令帮助 命令不区分大小写");
		writefln("=======================================================");
		writefln("-                    Master command                   -");
		writefln("=======================================================");
		writefln("-                                                      ");
		foreach(CommandModle cmd; m_Cmmands)
		{
			writefln("-        命令 : %s [%s]", cmd.Name, cmd.Directions);
			writefln("-");
		}
		writefln("-------------------------------------------------------");
		writefln("=======================================================");
	}

	void ClearScreen(string cmd)
	{
		clrscr();
		writefln(m_Developers);
	}

	void AppExit(string cmd)
	{
		m_Closing = true;
	}

	void CmdError(string cmd)
	{
		writefln("命令("~ cmd ~") : 未知的无效命令");
	}
}

