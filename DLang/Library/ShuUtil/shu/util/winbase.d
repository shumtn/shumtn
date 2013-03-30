module shu.util.winbase;

import std.c.windows.windows, std.c.windows.winsock, core.vararg;
import core.memory;

//导入库：kernel32.lib 头文件：winbase.h
struct SYSTEM_INFO
{
	union
	{
		DWORD dwOemId;
		struct
		{
			WORD wProcessorArchitecture;//{处理器的体系结构}
			WORD wReserved;	//{保留}
		}
	}
	DWORD dwPageSize;	//{分页大小}
	PVOID lpMinimumApplicationAddress;	//{最小寻址空间}
	PVOID lpMaximumApplicationAddress;	//{最大寻址空间}
	DWORD dwActiveProcessorMask;	//{处理器掩码; 0..31 表示不同的处理器}
	DWORD dwNumberOfProcessors;	//{处理器数目}
	DWORD dwProcessorType;	//{处理器类型}
	DWORD dwAllocationGranularity;	//{虚拟内存空间的粒度}
	WORD  wProcessorLevel;	//{处理器等级}
	WORD  wProcessorRevision;	//{处理器版本}
}
alias SYSTEM_INFO* LPSYSTEM_INFO;

extern(Windows)
{
	//GetSystemInfo返回关于当前系统的信息。
	void GetSystemInfo(LPSYSTEM_INFO);
	
	//HGLOBAL GlobalAlloc(UINT, DWORD);	//分配
	//HGLOBAL GlobalFree(HGLOBAL);	//释放

	BOOL CloseHandle(HANDLE);
	
	void OutputDebugStringA(LPCSTR);
	void OutputDebugStringW(LPCWSTR);
	
	DWORD GetCurrentDirectoryA(DWORD, LPSTR);
	DWORD GetCurrentDirectoryW(DWORD, LPWSTR);
	
	DWORD FormatMessageA(DWORD, void*, DWORD, DWORD, LPSTR, DWORD, va_list*);
	DWORD FormatMessageW(DWORD, void*, DWORD, DWORD, LPWSTR, DWORD, va_list*);
}
alias FormatMessageW FormatMessage;
alias GetCurrentDirectoryA GetCurrentDirectory;










// 异步事件句柄
alias HANDLE WSAEVENT;
// 异步完成端口
alias OVERLAPPED WSAOVERLAPPED;
// 异步完成端口指针
alias OVERLAPPED* LPWSAOVERLAPPED;
alias OVERLAPPED* POVERLAPPED, LPOVERLAPPED;

	//GUID结构
	struct GUID
	{
		align(1):
			DWORD Data1;
			WORD  Data2;
			WORD  Data3;
			BYTE  Data4[8];
	}

	// 异步 协议链 结构
	struct WSAPROTOCOLCHAIN
	{
		// 协议链长度
	    int			ChainLen;
	    DWORD[7]	ChainEntries;
	}
	// 异步 协议链 结构指针
	alias WSAPROTOCOLCHAIN* LPWSAPROTOCOLCHAIN;	
	// 异步 协议链 长度
	const WSAPROTOCOL_LEN = 255;
	// Io错误等待
	const ERROR_IO_PENDING = 997;

	// 异步 协议链信息 结构
	struct WSAPROTOCOL_INFOW
	{
		// 位字段。已定义了 19 位。2^19 = 512K 个合法值。（x 524,288
	    DWORD dwServiceFlags1;
		// 保留（x 1）
	    DWORD dwServiceFlags2;
		// 保留（x 1）
	    DWORD dwServiceFlags3;
		// 保留（x 1）
	    DWORD dwServiceFlags4;
		// 保留（x 1）
	    DWORD dwProviderFlags;
		// 一个 GUID，消除提供相同协议的多个供应商之间的歧义。（x 1）
	    GUID ProviderId;
		// 由 WS2_32.DLL 为每个 WSAPROTOCOL_INFO 结构指定的唯一标识。（x 1）
	    DWORD dwCatalogEntryId;
		// 由 7 个项组成的结构。该结构表示在基本协议的顶部由一个或多个协议构成的协议链。（x 1）
	    WSAPROTOCOLCHAIN ProtocolChain;
		// 协议版本标识符。（x 1）
	    int iVersion;
		// 地址系列。大概与 WSASocket 接口中的相同。（x 1）
	    int iAddressFamily;
		// “最大地址长度”--? （x 1）
	    int iMaxSockAddr;
		// “最小地址长度”--? （x 1）
	    int iMinSockAddr;
		// 套接字类型。2 个值，但参数已在 socket() API 中说明。（x 1）
	    int iSocketType;
		// 与 socket() API 中的相同。我们将只考虑一个（x 1）
	    int iProtocol;
		// 特定于 Windows --? （x 1）
	    int iProtocolMaxOffset;
		// BIGENDIAN 或 LITTLEENDIAN（x 2） 网络字节序
	    int iNetworkByteOrder;
		// 只定义了一个。（x 1）
	    int iSecurityScheme;
		// 最大消息大小。定义了三个特殊值，加上任何实际协议支持的值。（x 3）
	    DWORD dwMessageSize;
		// 保留。
	    DWORD dwProviderReserved;
		// 可能是一个标识协议的 Unicode 字符数组。（x 1）
	    WCHAR[WSAPROTOCOL_LEN+1] szProtocol;
	}
	// 异步 协议链信息 结构指针
	alias WSAPROTOCOL_INFOW* LPWSAPROTOCOL_INFOW;
	// 位字段
	const WSA_FLAG_OVERLAPPED = 0x01;

	//==========================================
	// 异步 缓冲 结构
	struct WSABUF
	{
		// 缓冲区大小
	    uint  len;
		// 缓冲区指针
	    char* buf;   
	}
	// 异步缓冲指针
	alias WSABUF* LPWSABUF;
	
	// Socket地址结构
	struct SOCKADDR
	{
	    ushort		sa_family;
	    char[14]	sa_data;
	}
	// Socket地址指针
	alias SOCKADDR* LPSOCKADDR;
	// ？？？
	alias uint GROUP;  
	// 导出WindowsAip
	extern(Windows)
	{		
	    alias void function(DWORD, DWORD, LPWSAOVERLAPPED, DWORD) LPWSAOVERLAPPED_COMPLETION_ROUTINE;

		// 异步Socket
	    SOCKET WSASocketW(int, int, int, LPWSAPROTOCOL_INFOW, GROUP, DWORD);
		// 异步发送
	    int WSASend(SOCKET, LPWSABUF, DWORD, LPDWORD, DWORD, LPWSAOVERLAPPED, LPWSAOVERLAPPED_COMPLETION_ROUTINE);
		// 异步接收
	    int WSARecv(SOCKET, LPWSABUF, DWORD, LPDWORD, LPDWORD, LPWSAOVERLAPPED, LPWSAOVERLAPPED_COMPLETION_ROUTINE);
		// 扩展应答
		bool AcceptEx(SOCKET, SOCKET, PVOID, DWORD, DWORD, DWORD, LPDWORD, LPDWORD);
	
		bool ConnectEx(SOCKET, PVOID, DWORD, PVOID, DWORD, LPDWORD, LPOVERLAPPED);
	
		//int WSAConnect ( SOCKET s, sockaddr FAR * name, int namelen, LPWSABUF lpCallerData, LPWSABUF lpCalleeData,  LPQOS lpSQOS, LPQOS lpGQOS );

	
	
		// 创建Io完成端口
	    HANDLE CreateIoCompletionPort(HANDLE, HANDLE, DWORD, DWORD);
	
		// 获取完成端口状态
	    bool GetQueuedCompletionStatus(HANDLE, PDWORD, PDWORD, LPWSAOVERLAPPED*, DWORD);
	
		//返回指定套接口上一个重叠操作的结果
		bool WSAGetOverlappedResult(SOCKET, LPWSAOVERLAPPED, LPDWORD, BOOL, LPDWORD);
		
		// 发送投递 建议不用 多线程下 导致对方接收到的数据可能出现乱序
		//bool PostQueuedCompletionStatus(HANDLE, DWORD, DWORD, LPOVERLAPPED);
	
		void* GlobalAlloc( UINT, SIZE_T );
	}
	// 异步Socket对象
	alias WSASocketW WSASocket;

	// Io事件类型枚举
	enum IO_OPERATION
	{
		ACCEPT,	//应答
		RECV,	//接收
		SEND	//发送
	}

	/*struct PIO_DATA
	{
	    WSAOVERLAPPED               ol;
	    char                        Buffer[128];
	    WSABUF                      wsabuf;					// 缓冲大小
	    int                         nTotalBytes;			// 总字节
	    int                         nSentBytes;				// 已发送字节
	    IO_OPERATION                opCode;					// 执行事件类型
	    SOCKET                      activeSocket;			// 活动Socket对象
	}*/

	/*struct OVERLAPPEDPLUS
	{
	    OVERLAPPED		ol;
	    SOCKET			s;
	    int				OpCode;
	    WSABUF			wbuf;
	    uint			dwTransferredBytes;
	    uint			dwTotalBytes;
	    uint			dwFlags;
	    uint			dwSequence;
	    LPVOID			pvContext;
	    ulong			dnidClient;					// 客户端编号

	    uint			dwRecvBytes;
	    uint			dwRecvTimes;

	    //cAutoSizeMemory pOperationBuffer;
	}*/

/*struct IocpData
{
	OVERLAPPED		Overlapped;
	SOCKET			Socket;
	ulong			SocketId;					// 客户端编号
	WSABUF          Wsabuf;	
	char            buffer[4096];		
    
    int				OpCode;
    uint			TransferredBytes;
    uint			TotalBytes;
    uint			Flags;
    uint			Sequence;
    LPVOID			Context;

    uint			RecvBytes;
    uint			RecvTimes;

	uint			SentBytes;
    uint			SentTimes;
}
alias IocpData* LpIocpData;*/

/*struct HandleData
{
	SOCKET	Socket;
}
alias HandleData* LpHandleData;*/

static ulong generateDnid()
{
	long li;
    QueryPerformanceCounter(&li);
    return li;
}