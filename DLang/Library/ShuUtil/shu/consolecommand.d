module shu.consolecommand;

import std.stdio, std.string, shu.consoled;

alias void function(string cmd) CmdDisplayCallback;
alias bool function(string cmd) CmdExecuteCallback;

class ConsoleCommand
{

private:
	CmdDisplayCallback m_OnCmdDisplay;
	CmdExecuteCallback m_OnCmdExecute;
	string m_Developers = null;
	bool m_Closing = false;
	string m_StrCmd = null;

public:
	this(CmdDisplayCallback cmdDisplay, CmdExecuteCallback cmdExecute)
	{
		m_OnCmdDisplay = cmdDisplay;
		m_OnCmdExecute = cmdExecute;
	}

	void Start(string promptName = "Server=>", string developers = "☆☆☆蜀山技术研究团队 ☆ 服务器端组☆☆☆")
	{
		m_Developers = developers;
		
		while(!m_Closing)
		{
			while (true)
			{				
				// 再次检测程序已经运行
				if(m_Closing == true) break;

				m_StrCmd = strip(readln());

				// 如果输入的键值不为空
				if (m_StrCmd !is null && m_StrCmd != "")
				{
					CmdExecute(m_StrCmd);
					break;
				}
				else
				{
					///显示命令
					CmdDisplay(null);
					continue;
				}
				
				///执行命令
				write(promptName);
			}
		}
	}
	
	/// <summary>
	/// 显示 命令
	/// </summary>
	/// <param name="cmd">命令</param>
	private void CmdDisplay(string cmd)
	{
		if(m_StrCmd is null || m_StrCmd == "")
		{
			cmd = "回车";
		}

		writefln("命令("~ cmd ~") : Help - 命令帮助 命令不区分大小写");
		writefln("=======================================================");
		writefln("=                       主命令                        =");
		writefln("=======================================================");
		writefln("=                                                     =");
		writefln("=        命令 : ? -Admin   => 显示所有命令            =");
		writefln("=                                                     =");
		writefln("=        命令 : Cls -Cls   => 清屏                    =");
		writefln("=                                                     =");
		writefln("=        命令 : Exit -Exit => 退出程序                =");
		writefln("=                                                     =");
		writefln("=======================================================");
		m_OnCmdDisplay(cmd);
	}
	
	/// <summary>
	/// 执行命令
	/// </summary>
	/// <param name="cmd">命令</param>
	private void CmdExecute(string cmd)
	{
		switch (cmd)
		{
			case "?":
			case "-admin":
				CmdDisplay(cmd);
				break;
			case "cls":
				clrscr();
				break;
			case "exit":
			case "-exit":
				writefln("命令("~ cmd ~") : Exit - 退出程序");
				m_Closing = true;
				break;
			default:	
				//if(m_StrCmd != "" && m_OnCmdExecute(cmd) == false)
				if(m_StrCmd != "")
				{
					writefln("命令("~ cmd ~") : 未知的无效命令");
				}
				break;
		}
	}
}

