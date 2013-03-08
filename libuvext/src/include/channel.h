#include "uv.h"

typedef void (*net_connect_cb)(struct channel* c);
typedef void (*net_write_cb)(struct channel* c);
typedef void (*net_read_cb)(struct channel* c);
typedef void (*net_close_cb)(struct channel* c);
typedef void (*net_error_cb)(struct channel* c);

struct channel_s
{
	int			id;					// 客户端编号
	char*		localIp;			// 本地地址
	int 		localPort;		// 本地端口
			
	char* 		remoteIp;			// 远程地址
	int 		remotePort;		// 远程端口
	
	int			size;			// 数据长度
	char*		data;			// 数据
	//bool		alive;			// 连接 是否还是活动的
	
	//int 		fd;				// 句柄ID
	void*		handle;			// 缓冲指针
	
	//uint		TotalBytes;		// 总字节数
	//uint		CurrentBytes;	// IO大小	
	
    //uint		RecvBytes;		// 接收字节数
    //SysTime		RecvTime;		// 接收时间
	
	//uint		SentBytes;		// 发送字节数
    //SysTime		SentTime;		// 发送时间
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