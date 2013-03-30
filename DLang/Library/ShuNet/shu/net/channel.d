module shu.net.channel;

import std.datetime;
import shu.net.libuv.imports;

//alias void function(channel* c) ReceiveCallBack;
alias void delegate(channel* c) ReceiveCallBack;

struct channel
{
	int			id;				// 客户端编号
	char*		localIp;		// 本地地址
	int 		localPort;		// 本地端口
	
	char* 		remoteIp;		// 远程地址
	int 		remotePort;		// 远程端口
	
	int			size;			// 数据长度
	char*		data;			// 数据
	void*		handle;			// 缓冲指针

	uint send(void* data, uint length)
	{
		net_server_write(handle, cast(char*)data, length);
		
		return length;
	}
}