module main;

import std.stdio, shu.text.string;
import mainserver;

void main(string[] args)
{
	// 设置语言
	setLocale();

	MainServer.InitServer(args);
	MainServer.StarServer(args);
}