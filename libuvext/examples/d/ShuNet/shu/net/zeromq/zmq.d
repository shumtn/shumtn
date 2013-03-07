module shu.net.zeromq.zmq;

version (Windows)
{
	pragma (lib, "libzmq_d.lib");
}
else version (linux)
{
	pragma (lib, "libzmq_d.a");
}
else version (darwin)
{
	pragma (lib, "libzmq_d.a");
}
else
{
	static assert (0);
}

extern (C):


/*  Socket types.                                                             */
immutable enum
{
    ZMQ_PAIR        = 0,
	ZMQ_PUB         = 1,
	ZMQ_SUB         = 2,
	ZMQ_REQ         = 3,
	ZMQ_REP         = 4,
	ZMQ_DEALER      = 5,
	ZMQ_ROUTER      = 6,
	ZMQ_PULL        = 7,
	ZMQ_PUSH        = 8,
	ZMQ_XPUB        = 9,
	ZMQ_XSUB        = 10,
	ZMQ_XREQ        = ZMQ_DEALER,   /*  Old alias, remove in 3.x  */
	ZMQ_XREP        = ZMQ_ROUTER,   /*  Old alias, remove in 3.x  */
	ZMQ_UPSTREAM    = ZMQ_PULL,     /*  Old alias, remove in 3.x  */
	ZMQ_DOWNSTREAM  = ZMQ_PUSH      /*  Old alias, remove in 3.x  */
}


immutable enum
{
	/*  Maximal size of "Very Small Message". VSMs are passed by value            */
	/*  to avoid excessive memory allocation/deallocation.                        */
	/*  If VMSs larger than 255 bytes are required, type of 'vsm_size'            */
	/*  field in zmq_msg_t structure should be modified accordingly.              */
    ZMQ_MAX_VSM_SIZE    = 30,

	/*  Message types. These integers may be stored in 'content' member of the    */
	/*  message instead of regular pointer to the data.                           */
	ZMQ_DELIMITER       = 31,
	ZMQ_VSM             = 32,

	/*  Message flags. ZMQ_MSG_SHARED is strictly speaking not a message flag     */
	/*  (it has no equivalent in the wire format), however, making  it a flag     */
	/*  allows us to pack the stucture tigher and thus improve performance.       */
	ZMQ_MSG_MORE        = 1,
	ZMQ_MSG_SHARED      = 128,
	ZMQ_MSG_MASK        = 129
}

/*  A message. Note that 'content' is not a pointer to the raw data.          */
/*  Rather it is pointer to zmq::msg_content_t structure                      */
/*  (see src/msg_content.hpp for its definition).                             */
struct zmq_msg_t
{
    void* content;
    ubyte flags;
    ubyte vsm_size;
    ubyte vsm_data[ZMQ_MAX_VSM_SIZE];
}
__gshared struct zmq_pollitem_t
{
    void* socket;
    version (win32)
    {
        SOCKET fd;
    }
    else
    {
        int fd;
    }
    short events;
    short revents;
}

int zmq_bind(void* s, const char* addr);
int zmq_close(void* s);
int zmq_connect(void* s, immutable char* addr);
// zmq_ctx_destroy
// zmq_ctx_get
// zmq_ctx_new
// zmq_ctx_set
// zmq_ctx_set_monitor
int zmq_device(int device, void* insocket, void* outsocket);
// zmq_disconnect
// zmq_erron
int zmq_getsockopt(void* s, int option, void* optval, size_t *optvallen);
void* zmq_init(int io_threads);
int zmq_msg_close(zmq_msg_t* msg);
int zmq_msg_copy(zmq_msg_t* dest, zmq_msg_t* src);
void* zmq_msg_data(zmq_msg_t* msg);
// zmq_msg_get
int zmq_msg_init(zmq_msg_t* msg);
int zmq_msg_init_data(zmq_msg_t* msg, void* data, size_t size, void function(void* data, void* hint), void* hint);
int zmq_msg_init_size(zmq_msg_t* msg, size_t size);
// zmq_msg_more
int zmq_msg_move(zmq_msg_t* dest, zmq_msg_t* src);
// zmq_msg_recv
// zmq_msg_send
// zmq_msg_set
// zmq_msg_size
int zmq_poll(zmq_pollitem_t* items, int nitems, long timeout);
int zmq_recv(void* s, zmq_msg_t* msg, int flags);
// zmq_recviov
// zmq_recvmsg
int zmq_send(void* s, zmq_msg_t* msg, int flags);
// zmq_sendiov
// zmq_sendmsg
int zmq_setsockopt(void* s, int option, void* optval, size_t optvallen);
// zmq_sleep
void* zmq_socket(void* context, int type);
// zmq_stopwatch_start
// zmq_stopwatch_stop
char* zmq_strerror(int errnum);
int zmq_term(void* context);
// zmq_unbind
void zmq_version(int* major, int* minor, int* patch);/*  Run-time API version detection */