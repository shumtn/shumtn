module network.dbservice;

import std.stdio, std.conv;
import shu.net.bservice, shu.net.channel, shu.net.packet.packethandler, shu.text.string;
import network.cpacket, netpacket.loginmsg, network.NetManager;

class DBService : BService
{
	
private:
	static CPacket m_CPacket = null;
	static channel*[ulong] m_Clients;
	
public:
	this()
	{
		m_CPacket = new CPacket();
		m_CPacket.RegisterPacket();
	}
	
	override void OnOpen(channel* c)
	{
		m_Clients[c.id] = c;
		//writefln("OnOpen id=>%d ip=>%s:%d", c.id, to!string(c.remoteIp), c.remotePort);

		QLoginMsg msg;
		msg.wVersion = 123;
		msg.dPort = 456;
		c.send(&msg, msg.sizeof, false);
		writefln("Send DBServer Data len=>%d hex=>%s\n", msg.sizeof, PtrToHex(&msg, msg.sizeof));
	}
	
	/*override void OnWrite(channel* c)
	{
		writefln("OnWrite id=>%d ip=>%s:%d size=>%d", c.id, to!string(c.remoteIp), c.remotePort, c.size);
	}*/
	
	override void OnRead(channel* c)
	{
		writefln("OnRead id=>%d ip=>%s:%d size=>%d hex=>%s ", c.id, to!string(c.remoteIp), c.remotePort, c.size, BytesToHex(cast(ubyte[])c.data[0..c.size]));

		/*QLoginMsg msg;
		msg.wVersion = 123;
		msg.dPort = 456;
		c.send(&msg, msg.sizeof, false);
		//NetManager.Instance().GetDb().send(c.handle, &msg, msg.sizeof);
		writefln("data len=>%d hex=>%s\n", msg.sizeof, PtrToHex(&msg, msg.sizeof));*/
		
		m_CPacket.ReceivePacket(c);
	}
	
	override void OnClose(channel* c)
	{
		int q, h = 0;
		if(m_Clients !is null && m_Clients.length > 0)
		{
			q = m_Clients.length;
			m_Clients.remove(c.id);
			h = m_Clients.length;
		}
		writefln("OnClose id=>%d ip=>%s:%d clients q=>%d h=>%d \n", c.id, to!string(c.remoteIp), c.remotePort, q, h);
	}
	
	override void OnError(channel* c)
	{
		writefln("OnError id=>%d ip=>%s", c.id, c.remoteIp);
	}
}

