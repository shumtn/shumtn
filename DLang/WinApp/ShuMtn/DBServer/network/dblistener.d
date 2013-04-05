module network.dblistener;

import std.stdio, std.conv, core.thread;
import shu.net.interfaces.iservice, shu.net.netserver, network.dbservice;

class DBListener
{

private:
	IService m_Service = null;
	NetServer m_Server = null;
	Thread m_Thread = null;

	void runServer()
	{
		m_Service = new DBService();
		m_Server = new NetServer("0.0.0.0", 6100, 200, 1000, 4096, &m_Service);
		m_Server.open();
	}

public:
	this()
	{
		m_Thread = new Thread(&runServer);
	}

	void start()
	{
		if(m_Thread !is null) m_Thread.start();
	}

	void stop()
	{
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

