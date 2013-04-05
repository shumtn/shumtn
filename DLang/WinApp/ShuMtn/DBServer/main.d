module main;

import std.stdio, shu.text.string, mainserver;

void main(string[] args)
{
	// 设置语言
	setLocale();

	MainServer.InitServer(args);
	MainServer.StarServer();
}

