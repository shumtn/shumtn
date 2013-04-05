module netserver;

import std.stdio;
import shu.net.interfaces.iservice, shu.net.libuv.imports, shu.net.channel;

class NetServer
{

private:
	string m_Ip = null;
	ushort m_Port = 0;
	ushort m_Backlog = 0;
	ushort m_BufferSize = 0;
	ushort m_MaxConnects = 0;
	static IService* m_IService = null;

public:
	this(string ip, ushort port, ushort backlog=200, ushort maxConnects=5000, ushort bufferSize=4096, IService* iservice=null)
	{
		m_Ip			= ip;
		m_Port 			= port;
		m_Backlog 		= backlog;
		m_MaxConnects	= maxConnects;
		m_BufferSize	= bufferSize;	
		m_IService		= iservice;
	}

	void open()
	{
		net_server_open(cast(char*)m_Ip, m_Port, m_Backlog, m_MaxConnects, m_BufferSize, &on_open, &on_write, &on_read, &on_close, &on_error);
	}

	void close()
	{
		net_server_close();
	}

	uint send(void* handle, void* data, uint length)
	{
		//net_server_write(handle, cast(char*)data, length);

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