module shu.net.interfaces.iservice;

private import shu.net.channel;

interface IService
{
	/**
	* 连接完成后触发
	* Parames:
	* c = 网络事件
	* */
	void OnOpen(channel* c);

	/**
	* 发送数据时触发
	* Parames:
	* c = 网络事件
	* */
	void OnWrite(channel* c);

	/**
	* 数据到达时触发
	* Parames:
	* c = 网络事件
	* */
	void OnRead(channel* c);

	/**
	* 关闭连接
	* Parames:
	* c = 网络事件
	* */
	void OnClose(channel* c);

	/**
	* 网络异常时触发
	* Parames:
	* message = 异常信息
	* */
	void OnError(channel* c);
}