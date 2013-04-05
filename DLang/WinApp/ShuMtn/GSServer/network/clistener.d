module network.clistener;

import std.stdio, std.conv, core.thread;
import shu.text.string, shu.net.interfaces.iservice, shu.net.netserver, network.cservice;

class CListener
{

private:
	static IService m_Service = null;
	static NetServer m_Server = null;
	Thread m_Thread = null;
	
	void runServer()
	{
		m_Service = new CService();
		m_Server = new NetServer("0.0.0.0", 6000, 200, 5000, 4096, &m_Service);
		m_Server.open();
	}

public:
	this()
	{		
		m_Thread = new Thread(&runServer);
		//runServer();
	}
	
	void start()
	{
		if(m_Thread !is null) m_Thread.start();
		writefln("Start GSServer 0.0.0.0");
	}

	uint send(void* handle, void* data, uint length)
	{
		m_Server.send(handle, data, length);
		
		return length;
	}
	
	void stop()
	{
		//if(m_Thread !is null) m_Thread = null;

		writefln("Stop GSServer");
		if(m_Server !is null)
		{
			m_Server.close();
			m_Server = null;
		}
		
		if(m_Service !is null)
		{
			m_Service = null;
		}
	}
}