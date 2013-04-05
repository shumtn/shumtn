module shu.net.netclient;

import std.stdio;
import shu.net.interfaces.iservice, shu.net.libuv.imports, shu.net.channel;

class NetClient
{
	
private:
	string m_Ip = null;
	ushort m_Port = 0;
	ushort m_BufferSize = 0;
	static IService* m_IService = null;
	
public:
	this(string ip, ushort port, ushort bufferSize=4096, IService* iservice=null)
	{
		m_Ip			= ip;
		m_Port 			= port;
		m_BufferSize	= bufferSize;	
		m_IService		= iservice;
	}
	
	void open()
	{
		net_client_open(cast(char*)m_Ip, m_Port, m_BufferSize, &on_open, &on_write, &on_read, &on_close, &on_error);
	}
	
	void close()
	{
		writefln("start close");
		net_client_close();
	}

	uint send(void* handle, void* data, uint length)
	{
		//net_client_write(handle, cast(char*)data, length);

		return length;
	}
	
private:
extern(C):
	static void on_open(channel* c)
	{
		if(m_IService !is null) m_IService.OnOpen(c);
	}
	static void on_write(channel* c)
	{
		if(m_IService !is null) m_IService.OnWrite(c);
	}
	static void on_read(channel* c)
	{
		if(m_IService !is null) m_IService.OnRead(c);
	}
	static void on_close(channel* c)
	{
		if(m_IService !is null) m_IService.OnClose(c);
	}
	static void on_error(channel* c)
	{
		if(m_IService !is null) m_IService.OnError(c);
	}
}

/*extern (C)
{
	export void net_client_start(char* ip, ushort port, ushort bufferSize, void* connect_cb=null, void* write_cb=null, void* read_cb=null, void* close_cb=null, void* error_cb=null)
	{
		net_client_create(ip, cast(int)port, bufferSize, connect_cb, write_cb, read_cb, close_cb, error_cb);
	}

	export void net_client_stop()
	{
		//net_client_close();
	}

	export void net_client_send(void* bev, void* data, uint length)
	{
		net_client_write(bev, cast(char*)data, length);
	}
}*/