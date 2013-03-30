module network.dbservice;

import std.stdio, std.conv;
import shu.net.bservice, shu.net.channel, shu.net.packet.packethandler, shu.text.string;
import netmodule.loginmsg;
import packets.lgpacket;

class DBService : BService
{

private:
	static LGPacket m_LGPacket = null;
	static channel*[ulong] m_Clients;

public:
	this()
	{
		m_LGPacket = new LGPacket();
		m_LGPacket.RegisterPacket();
	}
	
	override void OnOpen(channel* c)
	{
		m_Clients[c.id] = c;
		writefln("OnOpen id=>%d ip=>%s:%d", c.id, to!string(c.remoteIp), c.remotePort);
	}
	
	/*override void OnWrite(channel* c)
	{
		writefln("OnWrite id=>%d ip=>%s:%d size=>%d", c.id, to!string(c.remoteIp), c.remotePort, c.size);
	}*/

	override void OnRead(channel* c)
	{
		writefln("OnRead id=>%d ip=>%s:%d size=>%d hex=>%s ", c.id, to!string(c.remoteIp), c.remotePort, c.size, hexToString(cast(ubyte[])c.data[0..c.size]));
		//writefln("OnRead id=>%d ip=>%s:%d size=>%d %s<=>%s ", c.id, to!string(c.remoteIp), c.remotePort, c.size, c.data[0..c.size], hexToString(cast(ubyte[])c.data[0..c.size]));		
		//c.Send(&userToByte, userToByte.sizeof);		
		//DBManager.GetApp.MySqlStart();
		//SQLoginMsg msg;
		//msg.dIP = 32432;
		//msg.dPort = 2000;
		//msg.wVersion = 332;
		//msg.streamData = "34324234234234";
		
		CTDLoginMsg msg;
		msg.wVersion = 123;
		msg.dPort = 456;		
		//writefln("msg sizeof=>%d", msg.sizeof);
		//c.send(&msg, msg.sizeof);	

		void* data_ptr = &msg;
		ubyte[8] data = cast(ubyte[])data_ptr[0..msg.sizeof];
		writefln("data len=>%d hex=>%s\n", msg.sizeof, hexToString(data));

		
		m_LGPacket.ReceivePacket(c);
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

