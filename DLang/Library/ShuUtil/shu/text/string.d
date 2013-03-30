module shu.text.string;

import std.string,std.ascii, std.array;  
import std.utf;  
import std.c.windows.windows;


import std.stdio, std.cstream;
import core.stdc.wchar_,core.stdc.locale;

extern(C) private int setlocale(int, char*);  
static setLocale()  
{  
	//char*loc = setlocale( LC_CTYPE, cast(char*)"" );
	//setlocale( LC_ALL, loc );
	
	fwide(core.stdc.stdio.stdout, 1);  
	//setlocale( LC_ALL, cast(char*)"" );  
	//setlocale( LC_ALL, cast(char*)"utf8" );  
	//setlocale( 0, cast(char*)"utf8" );
    setlocale(0, cast(char*)"china");
	
	
	
	//fwide(core.stdc.stdio.stdout, 1);  
	//setlocale(0, cast(char*)"china");     
	//setlocale(0, cast(char*)"china");

	//char *loc = setlocale( LC_CTYPE, "" );   
	//setlocale( LC_ALL, loc );

	//setlocale(0, cast(char*)"china");
	//setlocale(0, cast(char*)"korea");
	//setlocale(0, cast(char*)"japan");
	//setlocale(LC_ALL, cast(char*)"utf-8");
}

private wstring a2w( string a )
{     
	return toUTF16(a);      
}

private string wcs2mbz(wstring ws)
{
	uint codepage =  1; //2000/+
	char[] rz;
	
	rz.length = WideCharToMultiByte(codepage, 0, ws.ptr, ws.length, null, 0, null, null) + 1;
	WideCharToMultiByte(codepage, 0, ws.ptr, ws.length, rz.ptr, rz.length, null, null);
	rz[rz.length-1] = 0;
	rz.length = rz.length-1;
	return cast(string)rz.idup;
}

public string ToAnsi(string a)
{
	return wcs2mbz(a2w(a));
}

string hexToString(in ubyte[] bytes)
{
	auto result = new char[bytes.length * 2];
	size_t i;
	string temp;
	foreach (u; bytes)
	{
		result[i] = std.ascii.hexDigits[u >> 4];
		result[i + 1] = std.ascii.hexDigits[u & 0x0F];	
		/*if( i != bytes.length )
			temp ~= "0x" ~ result[i] ~ result[i + 1] ~ " ";
		else
			temp ~= "0x" ~ result[i] ~ result[i + 1];
		}*/
		temp ~= "" ~ result[i] ~ result[i + 1] ~ " ";	
		i += 2;
	}
	return temp;
}

string bytesToString(in byte[] bytes)
{
	auto result = new char[bytes.length * 2];
	size_t i;
	string temp;
	foreach (u; bytes)
	{
		result[i] = std.ascii.hexDigits[u >> 4];
		result[i + 1] = std.ascii.hexDigits[u & 0x0F];	
		/*if( i != bytes.length )
			temp ~= "0x" ~ result[i] ~ result[i + 1] ~ " ";
		else
			temp ~= "0x" ~ result[i] ~ result[i + 1];
		}*/
		temp ~= "" ~ result[i] ~ result[i + 1] ~ " ";	
		i += 2;
	}
	return temp;
}