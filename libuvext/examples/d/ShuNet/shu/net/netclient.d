module shu.net.netclient;

import shu.net.libuv.imports;

extern (C)
{
	export void net_client_start(string ip, ushort port, ushort bufferSize, void* connect_cb=null, void* write_cb=null, void* read_cb=null, void* close_cb=null, void* error_cb=null)
	{
		net_client_create(cast(char*)ip.ptr, cast(int)port, bufferSize, connect_cb, write_cb, read_cb, close_cb, error_cb);
	}

	/*export void net_client_stop(void* client_ptr)
	{

	}*/

	export void net_client_send(void* bev, void* data, uint length)
	{
		net_client_write(bev, cast(char*)data, length);
	}
}