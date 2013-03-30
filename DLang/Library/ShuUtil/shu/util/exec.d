module shu.util.exec;

private import std.c.windows.windows;
private import std.string;
//import std.string;
//private import std.windows.charset;
//private import std.stdio;
//private import std.bind;
/++++++++++++++++++++++++
+ Samples:
+ -----------------------
+ import wdd.os.exec;
+
+ void main()
+
{
    WinExec("ww.exe","hide"); 
    +
}
+ --------------------------
+/
/***********************************
* WinExec Windows Exec Command
* Params:
* strcmd = strcmd
* isshow = window
*/

version(Windows)
{
	extern (Windows)
	{
		private UINT WinExec(LPCSTR lpCmdLine,UINT uCmdShow);
	}

	public void Exec(string strcmd, string isshow)
	{
		if(isshow == "show")
		{
			WinExec(toStringz(strcmd), SW_SHOW);
		}
		else if(isshow == "hide")
		{
			WinExec(toStringz(strcmd), SW_HIDE);
		}
	}
}