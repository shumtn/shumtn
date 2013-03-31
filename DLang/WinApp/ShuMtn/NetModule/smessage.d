module smessage;

enum SMessage
{
	Account_Message = 5,
	Move_Message,
}

/*struct SMessage
{
    enum
    {
		Account_Message = 5,
		Move_Message,
        //EPRO_MOVE_MESSAGE = 32,	// 移动消息
        EPRO_CHAT_MESSAGE,			// 对话消息
        EPRO_FIGHT_MESSAGE,			// 战斗消息
        EPRO_SCRIPT_MESSAGE,		// 脚本消息
        EPRO_REGION_MESSAGE,        // 场景消息
        EPRO_ITEM_MESSAGE,          // 道具消息
        EPRO_SYSTEM_MESSAGE,		// 系统管理消息
        EPRO_UPGRADE_MESSAGE,       // 升级消息（玩家属性变化）
        EPRO_TEAM_MESSAGE,          // 组队相关的消息  （包括聊天的消息） 
        EPRO_TONG_MESSAGE,          // 帮会相关的消息  （包括聊天的消息）
        EPRO_MENU_MESSAGE,          // 菜单选择操作
        EPRO_NAMECARD_BASE,         // 名片
        EPRO_RELATION_MESSAGE,      // 好友,黑名单等等
        EPRO_SPORT_MESSAGE,         // 运动、竞技
		EPRO_BUILDING_MESSAGE,		// 动态建筑相关消息
        EPRO_PLUGINGAME_MESSAGE,	// 插入式小游戏
		ERPO_MOUNTS_MESSAGE,		// 坐骑相关消息
		EPRO_PRACTICE_MESSAGE,		// 自动挂机相关消息
		EPRO_CONTEST_MESSAGE,		// 比武大赛
		ERPO_MYTHAGO_MESSAGE,		// 元神消息
		EPRO_EXCHANGE_MESSAGE,		// 交易消息
		RPRO_WAREHOUSE_BASE,        // 仓库基类
		RPRO_MARRIAGE_BASE,         // 婚姻消息
		EPRO_RANK_MESSAGE,			// 排行榜消息
        EPRO_COLLECT_MESSAGE = 0xD0,// 数据采集功能类消息

        EPRO_GAMEMNG_MESSAGE = 0xe0,// GM指令（现在用来做验证了）
        EPRO_GMM_MESSAGE,           // GM模块指令（增补）
        EPRO_GMIDCHICK_MSG,         // 处理GM身份验证消息

        // 底层消息段
        EPRO_REFRESH_MESSAGE = 0xf0,// 数据刷新消息
        EPRO_SERVER_CONTROL,        // 服务器控制消息
        EPRO_REBIND_MESSAGE,        // 连接重定向消息
        EPRO_DATABASE_MESSAGE,      // 数据库相关消息
        EPRO_ACCOUNT_MESSAGE,       // 帐号检测相关消息
        EPRO_ORB_MESSAGE,           // 跨区域服务器相关消息
        EPRO_DATATRANS_MESSAGE,     // 数据传送相关消息
        EPRO_DBOP_MESSAGE,          // 数据库操作用消息
        EPRO_POINTMODIFY_MESSAGE,   // 点数交易操作相关消息
		EPRO_MAIL_MESSAGE,			// 留言系统相关消息
		EPRO_PHONE_MESSAGE,			// 电话系统相关消息
        EPRO_UNION_MESSAGE,         // 结义相关消息
		EPRO_EMAIL_MESSAGE,			// 邮件系统相关消息
		EPRO_ACTIVE_MESSAGE,		// 活动消息
		EPRO_ACHIEVEMENTS_MESSAGE,	// 成就消息
		EPRO_ACTIVITIES_MESSAGE,	// 活动消息 时间通知开关
    }
}*/

