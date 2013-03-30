module netpacket.loginMsg;

import smessage;

///////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////
// 系统消息类
//=============================================================================================
//DECLARE_MSG_MAP(SSysBaseMsg, SMessage, SMessage::EPRO_SYSTEM_MESSAGE)
//{{AFX
//EPRO_LOGIN,	              // 登陆 检查帐号和密码
//EPRO_CREATE_CHARACTER,      // 创建角色
//EPRO_DEL_CHARACTER,         // 删除角色
//EPRO_SELECT_CHARACTER,      // 选择角色
//EPRO_LOGOUT,                // 退出
//EPRO_CHARACTER_LIST_INFO,   // 设置客户端角色列表
//EPRO_CHARACTER_DATA_INFO,   // 设置客户端主角资料
//EPRO_LOGIN_NOCHECKACCOUNT,  // 不到帐号服务器检查（如从特殊服务器返回原服务器）
//EPRO_ENGINE_FLAGE,          // 用于统计使用引擎 有调色版本设置为1,没有设置为0
//EPRO_PREVENT_INDULGE,		  // 防沉迷
//EPRO_CANCEL_LOGOUT,         //取消退出
//}}AFX
//END_MSG_MAP()
//---------------------------------------------------------------------------------------------

// 基本模版
//=============================================================================================
template BuildPacketId(int id, SMessage fu = SMessage.Account_Message)
{
	const BuildPacketId = (id << 8) | fu;
}

mixin template BuildPacketHead(LoginMsg msg)
{
align(1):
	private	ushort lenght	= this.sizeof;
	private ushort cmds		= msg;
}
//---------------------------------------------------------------------------------------------

// 系统消息类
//=============================================================================================
enum LoginMsg
{
	Account_Login	= BuildPacketId!(1),	// 登陆 检查帐号和密码
	Account_Logout	= BuildPacketId!(2),	// 登陆 检查帐号和密码
	Create_Role,
	Delete_Role,
	Select_Role,
	Role_List_Info,
}
//---------------------------------------------------------------------------------------------

// 登录消息
//=============================================================================================
struct QDLoginMsg
{
	mixin 			BuildPacketHead!(LoginMsg.Account_Login);
	ushort			wVersion;		// 版本信息
	ushort			dPort;			// 端口
	int				hhhhl;
}

struct ALLoginMsg
{
	mixin 		BuildPacketHead!(LoginMsg.Account_Login);
	ushort		wVersion;		// 版本信息
	uint		dIP;			// 地址IP
	ushort		dPort;			// 端口
	char[28] 	streamData;		// 账号&密码		
}
//---------------------------------------------------------------------------------------------

// 登录消息
//=============================================================================================
struct QLoginMsg
{
	mixin 			BuildPacketHead!(LoginMsg.Account_Login);
	ushort			wVersion;		// 版本信息
	ushort			dPort;			// 端口
	int				hhhhl;
}

struct ALoginMsg
{
	mixin 		BuildPacketHead!(LoginMsg.Account_Logout);
	byte		bRetCode;
	ushort		wVersion;		// 版本信息
	uint		dIP;			// 地址IP
	ushort		dPort;			// 端口
	char[28] 	streamData;		// 账号&密码	

	enum RetCode
	{
		ERC_LOGIN_SUCCESS,		        // 登陆成功
		ERC_INVALID_VERSION,	        // 非法的版本号
		ERC_INVALID_ACCOUNT,	        // 无效的账号
		ERC_INVALID_PASSWORD,	        // 错误的密码
	}
}
//---------------------------------------------------------------------------------------------









struct STCLoginMsg // SLoginMsg 
{
	align(1):		
	ushort dwRetCode;	// 操作反馈信息
    ushort wGMLevel;	// GM等级
    uint dwEchoIP;		// 回显IP
	enum ERetCode
	{
		ERC_LOGIN_SUCCESS,		        // 登陆成功
		ERC_INVALID_VERSION,	        // 非法的版本号
		ERC_INVALID_ACCOUNT,	        // 无效的账号
		ERC_INVALID_PASSWORD,	        // 错误的密码
        ERC_LOGIN_ALREADY_LOGIN,        // 此账号已登陆
        ERC_GETLIST_TIMEOUT,            // 获取角色列表超时
        ERC_GETLIST_FAIL,               // 获取角色列表失败
        ERC_CHECKACC_TIMEOUT,           // 账号检测超时
        ERC_SEND_GETCL_TO_DATASRV_FAIL, // 向数据库服务器发送获取列表消息失败
        ERC_SEND_CACC_TO_ACCSRV_FAIL,   // 向账号服务器发送账号检测消息失败
        ERC_ALREADYLOGIN_AND_LINKVALID, // 此账号已登陆，同时相应的连接还未失效
        ERC_ALREADYLOGIN_BUT_INREBIND,  // 此账号已登陆，同时相应的连接已失效，但是处于重定向连接中[BUG]
        ERC_NOTENOUGH_CARDPOINT,        // 此账号点数不足
        ERC_SERVER_UPDATE,              // 服务器更新中，暂时不能登陆
        ERC_LOGIN_ERROR,                // 登陆失败
        ERC_CREATE_TEST_CHARACTER,      // 试玩账号，直接通知进入角色创建画面
        ERC_BLOCKED,                    // 账号被停权
        ERC_LIMITED,                    // 登陆时间太短
        ERC_MAXCLIENTS,                 // 已经达到连接上限
        ERC_LOCKEDON_SERVERID,          // 此账号数据已经被其他游戏服务器锁定，拒绝重复登陆
        ERC_QUEUEWAIT,                  // 排队等待
        ERC_REUPDATE,                   // 重新更新！！！

		ERC_SPLIT = 32,					// （分割标示）
		ERC_LOCKEDON_MOBILPHONE = 99,	// 该账号处于手机锁定状态，请拨打以下电话进行解锁。4008112096 或 4006567023
	}

	char[28] szDescription;

	//SALoginMsg()
	//{
		//memset(szDescription, 0, sizeof(szDescription));
	//}
}


////=============================================================================================
//// 用户登陆
//DECLARE_MSG(SLoginMsg, SSysBaseMsg, SSysBaseMsg::EPRO_LOGIN)
//// 服务器接受玩家连接（简单版），数据中有用户版本号，账号，密码
//struct SQLoginMsg : SLoginMsg 
//{
//	WORD wVersion;			        // 版本信息
//	//DWORD	dIP;					// 地址IP
//	//DWORD	dPort;					// 端口
//    // 账号&密码
//    char streamData[MAX_ACCAPASS];
//};
//
//// 接受登陆请求后的回应（简单版），成功或者失败信息在dwRetCode之中
//struct SALoginMsg : public SLoginMsg 
//{
//	WORD dwRetCode;	// 操作反馈信息
//    WORD wGMLevel;
//    DWORD dwEchoIP; // 回显IP
//	enum ERetCode
//	{
//		ERC_LOGIN_SUCCESS,		        // 登陆成功
//		ERC_INVALID_VERSION,	        // 非法的版本号
//		ERC_INVALID_ACCOUNT,	        // 无效的账号
//		ERC_INVALID_PASSWORD,	        // 错误的密码
//        ERC_LOGIN_ALREADY_LOGIN,        // 此账号已登陆
//        ERC_GETLIST_TIMEOUT,            // 获取角色列表超时
//        ERC_GETLIST_FAIL,               // 获取角色列表失败
//        ERC_CHECKACC_TIMEOUT,           // 账号检测超时
//        ERC_SEND_GETCL_TO_DATASRV_FAIL, // 向数据库服务器发送获取列表消息失败
//        ERC_SEND_CACC_TO_ACCSRV_FAIL,   // 向账号服务器发送账号检测消息失败
//        ERC_ALREADYLOGIN_AND_LINKVALID, // 此账号已登陆，同时相应的连接还未失效
//        ERC_ALREADYLOGIN_BUT_INREBIND,  // 此账号已登陆，同时相应的连接已失效，但是处于重定向连接中[BUG]
//        ERC_NOTENOUGH_CARDPOINT,        // 此账号点数不足
//        ERC_SERVER_UPDATE,              // 服务器更新中，暂时不能登陆
//        ERC_LOGIN_ERROR,                // 登陆失败
//        ERC_CREATE_TEST_CHARACTER,      // 试玩账号，直接通知进入角色创建画面
//        ERC_BLOCKED,                    // 账号被停权
//        ERC_LIMITED,                    // 登陆时间太短
//        ERC_MAXCLIENTS,                 // 已经达到连接上限
//        ERC_LOCKEDON_SERVERID,          // 此账号数据已经被其他游戏服务器锁定，拒绝重复登陆
//        ERC_QUEUEWAIT,                  // 排队等待
//        ERC_REUPDATE,                   // 重新更新！！！
//
//		ERC_SPLIT = 32,					// （分割标示）
//		ERC_LOCKEDON_MOBILPHONE = 99,	// 该账号处于手机锁定状态，请拨打以下电话进行解锁。4008112096 或 4006567023
//	};
//
//	char szDescription[MAX_USERDESC];
//
//	SALoginMsg()
//	{
//		memset(szDescription, 0, sizeof(szDescription));
//	}
//};
////---------------------------------------------------------------------------------------------