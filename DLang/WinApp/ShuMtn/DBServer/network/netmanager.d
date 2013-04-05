module network.netmanager;

import network.dblistener;

class NetManager
{

private:
	static NetManager m_instance = null;
	DBListener m_DBListener = null;

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
		m_DBListener = new DBListener;
		m_DBListener.start();
	}

	void stop()
	{
		if(m_DBListener !is null)
		{
			m_DBListener.stop();
			m_DBListener = null;
		}
	}
}

