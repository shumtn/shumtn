module shu.net.packet.bpackethandlers;

import std.array, std.stdio;
import shu.net.channel, shu.net.packet.packethandler;

class BPacketHandlers
{

private:
	PacketHandler[long] m_Handlers;
	uint m_Size = 0;
	ubyte[] m_Buffer = null;
	
public:
	this()
	{
			
	}
		
	void AddRegister(long packetId, long packetLength, ReceiveCallBack receiveCallBack)
	{
		m_Handlers[packetId] = new PacketHandler(packetId, packetLength, receiveCallBack);
	}

	/// <summary>
	/// 获取 数据包 处理调用者
	/// </summary>
	/// <param name="packetId">包Id</param>
	PacketHandler GetHandler(long packetId)
	{
		if(m_Handlers !is null && packetId in m_Handlers)
		{
			return m_Handlers[packetId];
		}

		return null;
	}

	/// <summary>
	/// 删除 数据包 处理调用者
	/// </summary>
	/// <param name="packetId">包Id</param>
	bool DelHandler(long packetId)
	{
		bool iReturn = false;
		if(m_Handlers !is null && packetId in m_Handlers)
		{
			m_Handlers.remove(packetId);
			iReturn = true;
		}
		return iReturn;
	}
		
	void ReceivePacket(channel* c)
	{
		/*is(c is null || c.size <= 0)
		{
			debug writefln("c is null || c.size <= 0");
			return;
		}*/

		m_Size = c.size;
		m_Buffer = cast(ubyte[])c.data[0..c.size];			
		if(m_Buffer is null)
		{
			debug writefln("m_Buffer is null");
			return;
		}
			
		// 获取长度
		long pLength = GetPacketLength();
		if(pLength != m_Size)
		{
			debug writefln("pLength %d != m_Size %d", pLength, m_Size);
		}
			
		// 获取编号
		long packetId = GetPacketId();			
		//writefln("ReceivePacket id=>%d GetPacketLength=>%d DataLength=>%d GetPacketId=>%d", c.id, plength, m_Size, packetId);
		/*foreach( PacketHandler p; m_Handlers)
		{
			writefln("p.PacketId=>%d", p.PacketId);
		}*/

		PacketHandler preceive = null;
		if(packetId in m_Handlers)
		{
			preceive = m_Handlers[packetId];
			if(preceive is null)
			{
				debug writefln("PacketId=>%d not exist!", packetId);
			}
			else
			{
				preceive.OnReceive(c);
			}
		}
		else
		{
			debug writefln("m_Handlers[%d] value not exist!", packetId);
		}
	}
			
	long GetPacketLength(int head = 2, int index = 0)
	{
		long iReturn = 0;

		//m_LockBuffer.EnterWriteLock();
		//{
			if (m_Size >= head) //数据长度 必须 大于 head(换句话说 是包的长度) 
			{
				iReturn = cast(long)((m_Buffer[index + 1] << 8) | m_Buffer[index]);
			}
		//}
		//m_LockBuffer.ExitWriteLock();

		return iReturn;
	}
			
	/// <summary>
	/// 获取PacketId 不移动包的 索引位置
	/// </summary>
	/// <param name="head">包头大小 默认 4</param>
	/// <param name="index">位置 默认 2</param>
	/// <returns></returns>
	long GetPacketId(int head = 4, int index = 2)
	{
		long iReturn = 0;

		//m_LockBuffer.EnterWriteLock();
		//{
			if (m_Size >= head) // 数据长度 要大于 包头
			{
				iReturn = cast(long)((m_Buffer[index + 1] << 8) | m_Buffer[index]);
			}
		//}
		//m_LockBuffer.ExitWriteLock();

		return iReturn;
	}
}