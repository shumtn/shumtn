module netserver;

import std.stdio;
import shu.net.libuv.imports;

extern (C)
{
	/**
	* 新服务器
	* parame:
	*	ip = 地址
	* 	port = 4502 服务器端口 
	* 	backlog = 200 同时监听数
	*   bufferSize = 4096 缓冲大小
	* */
	export void net_server_start(char* ip, ushort port, ushort backlog, int maxConnects, int bufferSize=4096, void* connect_cb=null, void* write_cb=null, void* read_cb=null, void* close_cb=null, void* error_cb=null)
	{
		net_server_create(ip, cast(int)port, cast(int)backlog, maxConnects, bufferSize, connect_cb, write_cb, read_cb, close_cb, error_cb);
	}

	/**
	* 关闭监听
	* parame:
	* 	server_ptr = 监听指针
	* */
	export void net_server_close_client(void* server_ptr)
	{
		net_server_close_client(server_ptr);
	}

	/**
	* 服务端发送消息
	* parame:
	* 	bev		= 对象
	*	data	= 数据
	*	length	= 长度
	* */
	export void net_server_send(void* bev, void* data, int length)
	{
		net_server_write(bev, cast(char*)data, length);
	}
}

