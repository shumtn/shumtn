module shu.net.libuv.imports;

extern(C):
	void net_server_create(char* ip, int port, int backlog, int maxConnects, int bufferSize, void* connect_cb, void* write_cb, void* read_cb, void* close_cb, void* error_cb);
	void net_server_write(void* handle, char* data, int size);
	void net_server_close_client(void* handle);

	void net_client_create(char* ip, int port, int bufferSize, void* connect_cb, void* write_cb, void* read_cb, void* close_cb, void* error_cb);
	void net_client_write(void* handle, char* data, int size);
	void net_client_close();

	
