module packets.cpacket;

import std.stdio;
import netmodel, shu.net.channel, shu.net.packet.bpackethandlers;
import netmodule.loginmsg;

class CPacket : BPacketHandlers
{

public:
	this()
	{
		
	}
	
	void RegisterPacket()
	{
		AddRegister(cast(long)LoginPacketId.Account_Login, 4, &Link_LoginServer);
		AddRegister(cast(long)LoginPacketId.Account_Logout, 4, &Test_LoginServer);
	}
	
	void Link_LoginServer(channel* c)
	{
		writefln("Link_LoginServer size=>%d", c.size);
		c.send(c.data, c.size);
	}
	
	void Test_LoginServer(channel* c)
	{
		writefln("Test_LoginServer size=>%d", c.size);
	}
}
