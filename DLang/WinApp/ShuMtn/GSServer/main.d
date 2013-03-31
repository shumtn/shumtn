module main;

import std.stdio;
import scripts.ScriptManager, shu.text.string, shu.consolecommand, network.Clistener;

void OnCmdDisplay(string cmd)
{

}

bool OnCmdExecute(string cmd)
{
	return true;
}

void main(string[] args)
{
	// 设置语言
	setLocale();

	writefln("服务器开始运行咯！");

	ConsoleCommand cc = new ConsoleCommand(&OnCmdDisplay, &OnCmdExecute);
	cc.Start();

	/*// 初始化脚本
	ScriptManager.Instance();

	// 初始化监听
	CListener cl = new CListener();
	if(cl !is null)
		cl.start();
	else
	{
		writefln("Server Start Error");
		stdin.readln();
	}*/
}