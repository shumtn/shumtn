module main;

import std.stdio, shu.text.string;
import mainserver, testcrc;

void main(string[] args)
{
	// 设置语言
	setLocale();

	testcrc crc = new testcrc();
	crc.listFile();

	MainServer.InitServer(args);
	MainServer.StarServer(args);
}