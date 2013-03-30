module shu.net.bservice;

import shu.net.interfaces.iservice, shu.net.channel;

/**
* 服务基类
* */
abstract class BService : IService
{	
	/**
	* 连接完成后触发
	* Parames:
	* c = 网络事件
	* */
	public void OnOpen(channel* c) { }

    /**
	* 发送数据时触发
	* Parames:
	* c = 网络事件
	* */
	public void OnWrite(channel* c) { }

    /**
	* 数据到达时触发
	* Parames:
	* c = 网络事件
	* */
	public void OnRead(channel* c) { }

	/**
	* 连接断开
	* Parames:
	* c = 网络事件
	* */
	public void OnClose(channel* c) { }

    /**
	* 网络异常时触发
	* Parames:
	* c = 异常信息
	* */
	public void OnError(channel* c) { }
}