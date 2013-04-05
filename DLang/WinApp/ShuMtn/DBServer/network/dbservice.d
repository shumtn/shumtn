module network.dbservice;

import std.stdio, std.conv;
import shu.text.string, shu.net.bservice, shu.net.channel, shu.net.packet.packethandler, shu.text.string;
import netpacket.loginmsg;
import network.dbpacket;

class DBService : BService
{

private:
	static DBPacket m_DBPacket = null;

public:
	this()
	{
		m_DBPacket = new DBPacket();
		m_DBPacket.RegisterPacket();
	}
	
	override void OnOpen(channel* c)
	{
		writefln("OnOpen id=>%d ip=>%s:%d", c.id, to!string(c.remoteIp), c.remotePort);
	}

	override void OnRead(channel* c)
	{
		writefln("OnRead id=>%d ip=>%s:%d size=>%d hex=>%s ", c.id, to!string(c.remoteIp), c.remotePort, c.size, BytesToHex(cast(ubyte[])c.data[0..c.size]));


		
		m_DBPacket.ReceivePacket(c);
	}

	override void OnClose(channel* c)
	{
		writefln("OnClose id=>%d ip=>%s:%d \n", c.id, to!string(c.remoteIp), c.remotePort);
	}
	
	override void OnError(channel* c)
	{
		writefln("OnError id=>%d ip=>%s", c.id, c.remoteIp);
	}
}

