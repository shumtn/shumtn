module network.NetManager;

import network.Clistener, network.dbconnect;

class NetManager
{

private:
	static NetManager m_instance = null;
	CListener m_Listener = null;
	DbConnect m_Client = null;

public:
	static NetManager Instance()
	{
		if(m_instance is null) m_instance = new NetManager();
		
		return m_instance;
	}

	this()
	{

	}

	void start()
	{
		// 初始化监听
		//m_Listener = new CListener();
		//m_Listener.start();

		m_Client = new DbConnect();
		m_Client.start();
	}

	CListener GetServer()
	{
		return m_Listener;
	}

	DbConnect GetDb()
	{
		return m_Client;
	}

	void stop()
	{
		if(m_Client !is null)
		{
			m_Client.stop();
			m_Client = null;
		}

		if(m_Listener !is null)
		{
			m_Listener.stop();
			m_Listener = null;
		}
	}
}

