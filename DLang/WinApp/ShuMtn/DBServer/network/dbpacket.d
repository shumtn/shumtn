module network.dbpacket;

import std.stdio;
import shu.net.channel, shu.net.packet.bpackethandlers;
import netpacket.loginmsg, database.dbmanager;

class DBPacket : BPacketHandlers
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
		/*CTDLoginMsg msg;
		msg.wVersion = 123;
		msg.dPort = 456;
		void* data_ptr = &msg;
		ubyte[8] data = cast(ubyte[])data_ptr[0..msg.sizeof];
		writefln("data len=>%d hex=>%s\n", msg.sizeof, hexToString(data));
		c.send(&msg, msg.sizeof, true);*/

		writefln("Link_LoginServer size=>%d", c.size);
		c.send(c.data, c.size, true);

		DBManager.GetApp().MySqlStart();
	}
		
	void Test_LoginServer(channel* c)
	{
		writefln("Test_LoginServer size=>%d", c.size);
	}
}

