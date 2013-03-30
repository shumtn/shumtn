module shu.net.packet.packethandler;

import shu.net.channel;

class PacketHandler
{
private:
	long m_PacketId = 0;
	long m_PacketLength = 0;
	ReceiveCallBack m_ReceiveCallBack;
		
public:
	@property long PacketId() { return m_PacketId; }
	@property void PacketId( uint value ) { m_PacketId = value; }
		
	@property long PacketLength() { return m_PacketLength; }
	@property void PacketLength( uint value ) { m_PacketLength = value; }

	this(long packetId, long packetLength, ReceiveCallBack receiveCallBack)
	{
		m_PacketId			= packetId;
		m_PacketLength		= packetLength;
		m_ReceiveCallBack	= receiveCallBack;
	}
		
	void OnReceive(channel* c)
	{ 
		m_ReceiveCallBack(c);
	}
}