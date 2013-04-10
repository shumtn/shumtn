module testcrc;

import std.stdio,std.digest.crc, std.file, std.string, std.conv;

class testcrc
{
	this()
	{
		// Constructor code
	}

	public void listFile()
	{
		auto tempStr = "";
		auto crc = new CRC32Digest();
		//auto dFiles = dirEntries(".","*.{d,di,dll,swf,crc}",SpanMode.depth);
		auto dFiles = dirEntries(".","*.{d,di,dll,swf,crc}",SpanMode.depth);
		foreach(d; dFiles)
		{
			auto bytes = cast(ubyte[])read(d.name);
			ubyte[] bHash = crc.digest(bytes);
			auto sHash = crcHexString(bHash);
			//string crc = toLower(sHash);
			//string url = chompPrefix(d.name, ".\\");
			//writefln("crc=>%s file=>%s", crc, url);
			string str = toLower(sHash) ~ " " ~ chompPrefix(d.name, ".\\") ~ "\r\n";
			tempStr ~= str;
		}
		writefln(tempStr);

		std.file.write("version.txt", tempStr.dup);

		readln();
	}
}

