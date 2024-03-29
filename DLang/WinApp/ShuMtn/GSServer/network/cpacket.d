module network.cpacket;

import std.stdio;
import shu.net.channel, shu.net.packet.bpackethandlers;
import netpacket.loginmsg, scripts.ScriptManager;

class CPacket : BPacketHandlers
{

public:
	this()
	{
		
	}
	
	void RegisterPacket()
	{
		AddRegister(LoginMsg.Account_Login, 4, &Link_LoginServer);
		AddRegister(LoginMsg.Account_Logout, 4, &Test_LoginServer);
	}

	void Link_LoginServer(channel* c)
	{
		writefln("Link_LoginServer size=>%d", c.size);
		//c.send(c.data, c.size);
	}
	
	void Test_LoginServer(channel* c)
	{
		writefln("Test_LoginServer size=>%d", c.size);
	}
}