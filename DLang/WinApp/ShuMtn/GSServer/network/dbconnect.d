module network.dbconnect;

import std.stdio, std.conv, core.thread;
import shu.text.string, shu.net.interfaces.iservice, shu.net.netclient, network.dbservice;

class DbConnect
{

private:
	static IService m_Service = null;
	static NetClient m_Client = null;
	Thread m_Thread = null;
	
	void runServer()
	{
		m_Service = new DBService();
		m_Client = new NetClient("127.0.0.1", 6100, 4096, &m_Service);
		m_Client.open();
	}
	
public:
	this()
	{
		//m_Thread = new Thread(&runServer);
		runServer();
	}
	
	void start()
	{
		//if(m_Thread !is null) m_Thread.start();
		writefln("Start DBConnect");
	}

	uint send(void* handle, void* data, uint length)
	{
		m_Client.send(handle, data, length);
		
		return length;
	}

	
	void stop()
	{
		//if(m_Thread !is null) m_Thread = null;
		
		writefln("Stop GSServer");
		if(m_Client !is null)
		{
			m_Client.close();
			m_Client = null;
		}
		
		if(m_Service !is null)
		{
			m_Service = null;
		}
	}
}

