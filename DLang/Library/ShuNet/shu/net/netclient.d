module shu.net.netclient;

import shu.net.libuv.imports;

extern (C)
{
	export void net_client_start(char* ip, ushort port, ushort bufferSize, void* connect_cb=null, void* write_cb=null, void* read_cb=null, void* close_cb=null, void* error_cb=null)
	{
		net_client_create(ip, cast(int)port, bufferSize, connect_cb, write_cb, read_cb, close_cb, error_cb);
	}

	export void net_client_stop()
	{
		net_client_close();
	}

	export void net_client_send(void* bev, void* data, uint length)
	{
		net_client_write(bev, cast(char*)data, length);
	}
}