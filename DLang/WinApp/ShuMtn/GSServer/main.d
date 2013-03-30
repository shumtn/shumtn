module main;

import std.stdio;
import network.Clistener;

void main(string[] args)
{
	CListener cl = new CListener();
	if(cl !is null)
		cl.start();
	else
	{
		writefln("Server Start Error");
		stdin.readln();
	}
}