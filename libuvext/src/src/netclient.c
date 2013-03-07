#include <stdio.h>
#include <stddef.h>
#include <stdlib.h>
#include "netclient.h"

#define container_of(ptr, type, member) \
  ((type *) ((char *) (ptr) - offsetof(type, member)))

typedef struct
{
	uv_connect_t connect;
	uv_tcp_t handle;
	uv_shutdown_t shutdown_req;
	channel channel;
} net_client_t;

typedef struct
{
  uv_write_t req;
  uv_buf_t buf;
} write_req_t;

static void on_connect(uv_stream_t*, int status);
static uv_buf_t on_alloc(uv_handle_t* handle, size_t suggested_size);
static void on_read(uv_stream_t*, ssize_t nread, uv_buf_t buf);
static void on_shutdown(uv_shutdown_t* req, int status);
static void on_server_close(uv_handle_t* handle);
static void on_client_close(uv_handle_t* handle);
static void on_write(uv_write_t* req, int status);

static uv_loop_t* m_loop_ptr;
static uv_tcp_t m_tcp;
static uv_connect_t m_connect;

static channel_event channel_ev;
static int server_closed;

void net_client_create(char* ip, int port, int bufferSize, net_connect_cb connect_cb, net_write_cb write_cb, net_read_cb read_cb, net_close_cb close_cb, net_error_cb error_cb)
{
	struct sockaddr_in addr = uv_ip4_addr(ip, port);
	int ret;
	uint64_t start;
	uint64_t stop;
	net_client_t* client;

	client = (net_client_t*)malloc(sizeof *client);
	if(client == NULL)
	{
		printf("malloc net_client_t error\n");
		return;
	}

	m_loop_ptr = uv_default_loop();
	if(m_loop_ptr == NULL)
	{
		fprintf(stderr, "Loop error\n");
		return;
	}

	// 事件
	channel_ev.on_connect = connect_cb;
	channel_ev.on_write	= write_cb;
	channel_ev.on_read	= read_cb;
	channel_ev.on_close	= close_cb;
	channel_ev.on_error	= error_cb;

	ret = uv_tcp_init(m_loop_ptr, (uv_tcp_t*)&client->handle);
	if(ret)
	{
		fprintf(stderr, "Socket error\n");
		return;
	}

	ret = uv_tcp_connect(&client->connect, (uv_tcp_t*)&client->handle, addr, &on_connect);
	if(ret)
	{
		fprintf(stderr, "Connect error\n");
		return;
	}

	client->channel.id	= (int)&client->handle;
	client->channel.handle = (void*)&client->handle;
	// 内存
	client->channel.data = (char*)malloc(bufferSize);
	client->channel.size = bufferSize;
	// 远程
	client->channel.remoteIp		= ip;
	client->channel.remotePort	= port;
	client->connect.data = client;
	client->handle.data = client;

	start = uv_hrtime();
	uv_run(m_loop_ptr);
	stop = uv_hrtime();
	printf("客户端连接运行 in %.2fs.\n", (stop - start) / 1e9);
}

static void on_connect(uv_connect_t* req, int status)
{
	int r;
	struct sockaddr localAddres;
	int namelen;
	struct sockaddr_in local_addr;
	net_client_t* connect;

	connect = (net_client_t*)req->data;
	if(connect == NULL)
	{
		printf("on_connect connect error\n");
		return;
	}

	namelen = sizeof localAddres;
	memset(&localAddres, -1, namelen);	
	r = uv_tcp_getsockname((uv_tcp_t*)&connect->handle, &localAddres, &namelen);
	if(r)
	{
		printf("on_connect->uv_tcp_getsockname=>%d\n", r);
		return;
	}
	local_addr = *(struct sockaddr_in*)&localAddres;
	connect->channel.localIp = inet_ntoa(local_addr.sin_addr);
	connect->channel.localPort = ntohs(local_addr.sin_port);

	r = uv_read_start(&connect->handle, on_alloc, on_read);
	if(r)
	{
		printf("on_connect->uv_read_start=>%d\n", r);
		return;
	}

	if(connect != NULL)
		channel_ev.on_connect(&connect->channel);
}

static uv_buf_t on_alloc(uv_handle_t* handle, size_t suggested_size)
{
	net_client_t* client = container_of(handle, net_client_t, handle);
	return uv_buf_init(client->channel.data, client->channel.size);
}

static void on_read(uv_stream_t* stream, ssize_t nread, uv_buf_t buf) 
{
	int i;
	write_req_t* wr;
	uv_shutdown_t* req;
	net_client_t* client;

	client = container_of(stream, net_client_t, handle);
	if(client == NULL)
	{
		printf("on_read client error\n");
		return;
	}

	if(nread < 0) // 服务器断开
	{
		uv_shutdown(&client->shutdown_req, stream, on_shutdown);
		return;
	}

	if (nread == 0)
	{
		return;
	}

	if (!server_closed)
	{
		for (i = 0; i < nread; i++)
		{
		  if (buf.base[i] == 'Q')
		  {
			if (i + 1 < nread && buf.base[i + 1] == 'S')
			{
			  uv_close((uv_handle_t*)&client->handle, on_client_close);
			  return;
			}
			else
			{
			  uv_close((uv_handle_t*)&client->handle, on_server_close);
			  server_closed = 1;
			}
		  }
		}
	}

	client->channel.data = buf.base;
	client->channel.size = nread;

	if(client != NULL)
		channel_ev.on_read(&client->channel);
}

static void on_shutdown(uv_shutdown_t* req, int status)
{
	net_client_t* clinet = container_of(req, net_client_t, shutdown_req);
	uv_close((uv_handle_t*)&clinet->handle, on_server_close);
}

static void on_server_close(uv_handle_t* handle)
{
	net_client_t* client = container_of(handle, net_client_t, handle);

	//printf("on_server_close => %d\n", client->channel.id);

	if(client != NULL)
		channel_ev.on_close(&client->channel);

	free(client->channel.data);

	free(client);
}

static void on_client_close(uv_handle_t* handle)
{
	net_client_t* client = container_of(handle, net_client_t, handle);

	//printf("on_client_close => %d\n", client->channel.id);

	if(client != NULL)
		channel_ev.on_close(&client->channel);

	free(client->channel.data);

	free(client);
}

void net_client_write(void* handle, char* data, ssize_t size)
{
	write_req_t* wr;
	wr = (write_req_t*)malloc(sizeof *wr);
	wr->buf = uv_buf_init(data, size);
	if(uv_write(&wr->req, (uv_stream_t*)handle, &wr->buf, 1, on_write))
	{
		printf("uv_write failed");
	}
}

static void on_write(uv_write_t* req, int status)
{
	write_req_t* wr;
	uv_err_t err;
	net_client_t* client;

	// 执行 write 回调
	wr = (write_req_t*)req;
	client = container_of(req->handle, net_client_t, handle);
	if(client != NULL)
		channel_ev.on_write(&client->channel);

	//free(wr->buf.base);
	free(wr);

	if (status == 0)
		return;

	err = uv_last_error(&client->handle.loop);
	fprintf(stderr, "uv_write error: %s\n", uv_strerror(err));

	if (err.code == UV_ECANCELED)
		return;

	uv_close((uv_handle_t*)req->handle, on_client_close);
}