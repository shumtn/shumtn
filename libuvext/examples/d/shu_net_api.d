module network.network;

import std.datetime;

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

extern(C)
{
	void net_server_start(string ip, ushort port, ushort backlog, int maxConnects, int bufferSize=4096, void* connect_cb=null, void* write_cb=null, void* read_cb=null, void* close_cb=null, void* error_cb=null);
	void net_server_send(void* bev, char* data, int length);

	void net_client_start(string ip, int port, int bufferSize=4096, void* connect_cb=null, void* write_cb=null, void* read_cb=null, void* close_cb=null, void* error_cb=null);
	void net_client_send(void* bev, char* data, int length);
}

