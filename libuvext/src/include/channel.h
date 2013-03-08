#include "uv.h"

typedef void (*net_connect_cb)(struct channel* c);
typedef void (*net_write_cb)(struct channel* c);
typedef void (*net_read_cb)(struct channel* c);
typedef void (*net_close_cb)(struct channel* c);
typedef void (*net_error_cb)(struct channel* c);

struct channel_s
{
	int			id;					// �ͻ��˱��
	char*		localIp;			// ���ص�ַ
	int 		localPort;		// ���ض˿�
			
	char* 		remoteIp;			// Զ�̵�ַ
	int 		remotePort;		// Զ�̶˿�
	
	int			size;			// ���ݳ���
	char*		data;			// ����
	//bool		alive;			// ���� �Ƿ��ǻ��
	
	//int 		fd;				// ���ID
	void*		handle;			// ����ָ��
	
	//uint		TotalBytes;		// ���ֽ���
	//uint		CurrentBytes;	// IO��С	
	
    //uint		RecvBytes;		// �����ֽ���
    //SysTime		RecvTime;		// ����ʱ��
	
	//uint		SentBytes;		// �����ֽ���
    //SysTime		SentTime;		// ����ʱ��
};
typedef struct channel_s channel;

struct channel_event_s
{
	net_connect_cb	on_connect;
	net_write_cb	on_write;
	net_read_cb		on_read;
	net_close_cb	on_close;
	net_error_cb	on_error;
};
typedef struct channel_event_s channel_event;