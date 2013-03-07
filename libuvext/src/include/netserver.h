#include "channel.h"

UV_EXTERN void net_server_create(char* ip, int port, int backlog, int maxConnects, int bufferSize, void* connect_cb, void* write_cb, void* read_cb, void* close_cb, void* error_cb);
UV_EXTERN void net_server_write(void* handle, char* data, ssize_t size);


