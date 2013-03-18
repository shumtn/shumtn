module shu.net.libuv.imports;

import std.socket, std.datetime;

//version(Windows)
//{
//    pragma(lib, "libuv.lib");
//}
//else 
//{
//    pragma(lib, "libuv.a");
//}

//version(Windows)
//	import std.c.windows.winsock;
//else
//	import core.sys.posix.netinet.in_;

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
}
alias channel* channel_ptr;

extern(C):
	void net_server_create(char* ip, int port, int backlog, int maxConnects, int bufferSize, void* connect_cb, void* write_cb, void* read_cb, void* close_cb, void* error_cb);
	void net_server_write(void* handle, char* data, int size);
	void net_server_close_client(void* handle);

	void net_client_create(char* ip, int port, int bufferSize, void* connect_cb, void* write_cb, void* read_cb, void* close_cb, void* error_cb);
	void net_client_write(void* handle, char* data, int size);
	void net_client_close();

	
