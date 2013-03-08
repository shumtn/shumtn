#include <stdio.h>
#include <stddef.h>
#include <stdlib.h>
#include "netserver.h"
//#include <testpool.h>

#define container_of(ptr, type, member) \
  ((type *) ((char *) (ptr) - offsetof(type, member)))

typedef struct
{
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

static uv_loop_t* m_loop;
static uv_tcp_t m_tcpServer;

static channel_event channel_ev;
static int server_closed;
//static jv_pool_t* m_pool;
static int m_buffer_size;

void net_server_create(char* ip, int port, int backlog, int maxConnects, int bufferSize, net_connect_cb connect_cb, net_write_cb write_cb, net_read_cb read_cb, net_close_cb close_cb, net_error_cb error_cb)
{
	struct sockaddr_in addr = uv_ip4_addr(ip, port);
	int ret;

	m_buffer_size = bufferSize;

	m_loop = uv_default_loop();
	if(m_loop == NULL)
	{
		fprintf(stderr, "Loop error\n");
		return;
	}

	/*m_pool = jv_pool_create(maxConnects * bufferSize);
	if(m_pool == NULL)
	{
		fprintf(stderr, "Pool error\n");
		return;
	}*/

	// 事件
	channel_ev.on_connect = connect_cb;
	channel_ev.on_write	= write_cb;
	channel_ev.on_read	= read_cb;
	channel_ev.on_close	= close_cb;
	channel_ev.on_error	= error_cb;

	ret = uv_tcp_init(m_loop, &m_tcpServer);
	if(ret)
	{
		fprintf(stderr, "Socket creation error\n");
		return;
	}

	ret = uv_tcp_bind(&m_tcpServer, addr);
	if(ret)
	{
		fprintf(stderr, "Bind error\n");
		return;
	}
			
	ret = uv_listen((uv_stream_t*)&m_tcpServer, backlog, &on_connect);
	if(ret)
	{
		fprintf(stderr, "Listen error\n");
		return;
	}

	uv_run(m_loop);

	// 释放
	free(m_loop);
	//jv_pool_destroy(m_pool);
}

static void on_connect(uv_stream_t* server, int status)
{
	int r;
	struct sockaddr localAddres, remoteAddres;
	int namelen;

	struct sockaddr_in local_addr, remote_addr;
	char *localIp, *remoteIp;
	int localPort, remotePort;
	net_client_t* client;

	client = (net_client_t*)malloc(sizeof *client);
	if(client == NULL)
	{
		printf("malloc net_client_t error\n");
		return;
	}

	r = uv_tcp_init(server->loop, &client->handle);
	if(r)
	{
		printf("on_connect->uv_tcp_init=>%d\n", r);
		return;
	}

	r = uv_accept(server, (uv_stream_t*)&client->handle);
	if(r)
	{
		printf("on_connect->uv_accept=>%d\n", r);
		return;
	}

	client->channel.id = (int)&client->handle;

	// local
	namelen = sizeof localAddres;
	memset(&localAddres, -1, namelen);	
	r = uv_tcp_getsockname((uv_tcp_t*)server, &localAddres, &namelen);
	if(r)
	{
		printf("on_connect->uv_tcp_getsockname=>%d\n", r);
		return;
	}
	local_addr = *(struct sockaddr_in*)&localAddres;
	client->channel.localIp = inet_ntoa(local_addr.sin_addr);
	client->channel.localPort = ntohs(local_addr.sin_port);

	// remote
	namelen = sizeof remoteAddres;
	memset(&remoteAddres, -1, namelen);
	r = uv_tcp_getpeername(&client->handle, &remoteAddres, &namelen);
	if(r)
	{
		printf("on_connect->uv_tcp_getpeername=>%d\n", r);
		return;
	}
	remote_addr = *(struct sockaddr_in*)&remoteAddres;
	client->channel.remoteIp = inet_ntoa(remote_addr.sin_addr);
	client->channel.remotePort = ntohs(remote_addr.sin_port);
	r = uv_read_start((uv_stream_t*)&client->handle, on_alloc, on_read);
	if(r)
	{
		printf("on_connect->uv_read_start=>%d\n", r);
		return;
	}

	//内存
	//client->channel.data = (char*)jv_pool_alloc(m_pool, 4000);
	//client->channel.size = 4000;
	client->channel.data = (char*)malloc(m_buffer_size);
	client->channel.size = m_buffer_size;	
	client->channel.handle = (void*)&client->handle;

	if(client != NULL)
		channel_ev.on_connect(&client->channel);
}

static uv_buf_t on_alloc(uv_handle_t* handle, size_t suggested_size)
{
	net_client_t* client = container_of(handle, net_client_t, handle);
	return uv_buf_init(client->channel.data, client->channel.size);
	//return uv_buf_init((char*)malloc(suggested_size), suggested_size);
}

static void on_read(uv_stream_t* stream, ssize_t nread, uv_buf_t buf) 
{
	int i;
	write_req_t* wr;
	uv_shutdown_t* req;
	net_client_t* client;

	client = container_of(stream, net_client_t, handle);

	if(nread < 0)
	{
		uv_shutdown(&client->shutdown_req, stream, on_shutdown);
		return;
	}

	if (nread == 0)
	{
		//free(buf.base);
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
			  //free(buf.base);
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

	//free(buf.base);
}

static void on_shutdown(uv_shutdown_t* req, int status)
{
	net_client_t* clinet = container_of(req, net_client_t, shutdown_req);
	uv_close((uv_handle_t*)&clinet->handle, on_client_close);
}

static void on_server_close(uv_handle_t* handle)
{
	net_client_t* client = container_of(handle, net_client_t, handle);

	printf("on_server_close => %d\n", client->channel.id);

	if(client != NULL)
		channel_ev.on_close(&client->channel);

	//jv_pool_free(m_pool, &client->channel.data);
	free(client->channel.data);

	free(client);
}

static void on_client_close(uv_handle_t* handle)
{
	net_client_t* client = container_of(handle, net_client_t, handle);

	//printf("on_client_close => %d\n", client->channel.id);

	if(client != NULL)
		channel_ev.on_close(&client->channel);

	//jv_pool_free(m_pool, &client->channel.data);
	free(client->channel.data);

	free(client);
}

void net_server_write(void* handle, char* data, ssize_t size)
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

	/* Free the read/write buffer and the request */
	wr = (write_req_t*)req;

	// 执行 write 回调
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

	uv_close((uv_handle_t*)req->handle, on_server_close);
}