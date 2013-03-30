module network.dblistener;

import std.stdio, std.conv, core.thread;
import shu.text.string, shu.net.interfaces.iservice, shu.net.netserver, network.dbservice;

class DBListener
{

private:
	IService m_Service = null;
	NetServer m_Server = null;
	Thread m_Thread = null;

	void runServer()
	{
		m_Service = new DBService();
		m_Server = new NetServer("0.0.0.0", 6100, 200, 5000, 4096, &m_Service);
		if(m_Server !is null) m_Server.open();
	}

public:
	this()
	{
		// 设置语言
		setLocale();

		//m_Thread = new Thread(&runServer, 3);
		runServer();
	}

	void start()
	{
		if(m_Thread !is null) m_Thread.start();
		writefln("开始监听 DBServer 0.0.0.0");
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
			//m_Service.close();
			m_Service = null;
		}
	}
}

