module packets.lgpacket;

import std.stdio;
import shu.net.channel, shu.net.packet.bpackethandlers;
import netmodule.loginmsg, database.dbmanager;

class LGPacket : BPacketHandlers
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
		//DBManager.GetApp().MySqlStart();
		c.send(c.data, c.size);
	}
		
	void Test_LoginServer(channel* c)
	{
		writefln("Test_LoginServer size=>%d", c.size);
	}
}

