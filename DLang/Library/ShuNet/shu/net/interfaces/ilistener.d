module shu.net.interfaces.ilistener;

interface IListener
{
	/// <summary>
	/// 名称
	/// </summary>
	@property string name();

	/// <summary>
	/// 是否运行
	/// </summary>
	@property bool running();

	/// <summary>
	/// 运行
	/// </summary>
	bool start();

	/// <summary>
	/// 停止
	/// </summary>
	void stop();

	/// <summary>
	/// 释放
	/// </summary>
	void dispose();
}