module shu.util.message;

//import std.array : appender;
import std.array;
import std.utf : toUTFz;
import std.stdio, std.format, core.vararg,core.sys.windows.windows;
import shu.util.winbase;

/**消息字体颜色*/
public enum MessageColor
{
	/**空*/
	None		,
	/**红色*/
	Red			= FOREGROUND_RED,
	/**绿色*/
	Green		= FOREGROUND_INTENSITY | FOREGROUND_GREEN,
	/**蓝色*/
	Blue		= FOREGROUND_INTENSITY | BACKGROUND_BLUE,
	/**亮度*/
	Intensity	= FOREGROUND_INTENSITY,
}

/**显示消息*/
public class Message
{
	/**
	 * 输出消息
	 * Params:
	 * 	 messageColr	= 消息颜色
	 *   stringFormat	= 显示文本
	 * */
	public static void Write(MessageColor, T...)(MessageColor messageColor, T args)
	{	
		version(Windows)
		{
			HANDLE consoleHwnd; //创建句柄，详细句柄知识，请百度一下或查MSDN
			
			consoleHwnd = GetStdHandle(STD_OUTPUT_HANDLE); //实例化句柄
			
			//控制台信息
			CONSOLE_SCREEN_BUFFER_INFO consoleInfo;
			
			//获取默认控制台信息
			GetConsoleScreenBufferInfo(consoleHwnd, &consoleInfo);
			
			//判断控制台颜色
			switch (messageColor)
			{
				case MessageColor.Red:
					SetConsoleTextAttribute(consoleHwnd, FOREGROUND_INTENSITY | FOREGROUND_RED);
					break;
				case MessageColor.Green:
					SetConsoleTextAttribute(consoleHwnd, FOREGROUND_INTENSITY | FOREGROUND_GREEN);
					break;
				case MessageColor.Blue:
					SetConsoleTextAttribute(consoleHwnd, FOREGROUND_INTENSITY | FOREGROUND_BLUE);
					break;
				default:
					break;
			}
			
			// 格式化参数
			auto writer = appender!string();			
			formattedWrite(writer, args);
			
			// 输出文字
			write(writer.data);
			//std write writeln(writer.data,"");
				
			//还原默认颜色
			SetConsoleTextAttribute(consoleHwnd, consoleInfo.wAttributes);
		}
		else version(Posix)
		{
			write("Posix系统","");
		}
		else
		{
			write("未知系统","");
		}
	}
	
	/**
	 * 调试
	 * Params:
	 *   stringFormat	= 显示文本
	 * */
	public static void None(T...)(T stringFormat)
	{
		WriteLine(MessageColor.Blue, stringFormat);
	}
	
	/**
	 * 调试 蓝色
	 * Params:
	 *   stringFormat	= 显示文本
	 * */
	public static void Debug(T...)(T stringFormat)
	{
		debug WriteLine(MessageColor.Blue, stringFormat);
	}
	
	/**
	 * 错误 红色
	 * Params:
	 *   stringFormat	= 显示文本
	 * */
	public static void Error(T...)(T stringFormat)
	{
		//stringFormat = "[错误]:" ~ stringFormat;
		WriteLine(MessageColor.Red, stringFormat);
	}
	
	/**
	 * 输出消息行
	 * Params:
	 * 	 messageColr	= 消息颜色
	 *   stringFormat	= 显示文本
	 * */
	public static void WriteLine(MessageColor, T...)(MessageColor messageColor, T stringFormat)
	{	
		version(Windows)
		{
			HANDLE consoleHwnd; //创建句柄，详细句柄知识，请百度一下或查MSDN
			
			consoleHwnd = GetStdHandle(STD_OUTPUT_HANDLE); //实例化句柄
			
			//控制台信息
			CONSOLE_SCREEN_BUFFER_INFO consoleInfo;
			
			//获取默认控制台信息
			GetConsoleScreenBufferInfo(consoleHwnd, &consoleInfo);
			
			//判断控制台颜色
			switch (messageColor)
			{
				case MessageColor.Red:
					SetConsoleTextAttribute(consoleHwnd, FOREGROUND_INTENSITY | FOREGROUND_RED);
					break;
				case MessageColor.Green:
					SetConsoleTextAttribute(consoleHwnd, FOREGROUND_INTENSITY | FOREGROUND_GREEN);
					break;
				case MessageColor.Blue:
					SetConsoleTextAttribute(consoleHwnd, FOREGROUND_INTENSITY | FOREGROUND_BLUE);
					break;
				default:
					break;
			}
			
			// 格式化参数
			auto stream = appender!string();
			formattedWrite(stream, stringFormat);
			
			// 输出文字
			writeln(stream.data,"");
			//writeln(stringFormat);
			
			//OutputDebugStringW(toUTFz!(const(wchar)*)(stream.data));
			//stream.clear();
			
			
			// 打印调试
			debug
			{
				//OutputDebugStringA( stringFormat.ptr );
				//OutputDebugStringW(toUTFz!(const(wchar)*)(writer.data));
			}
				
			//还原默认颜色
			SetConsoleTextAttribute(consoleHwnd, consoleInfo.wAttributes);
		}
		else version(Posix)
		{
			writeln("Posix系统","");
			/*上面的01表示加粗，34表示是蓝色，后面\033[0m表示恢复所有的
			属性为原来的默认值。更多关于颜色的参考，这篇文章有非常详细
			的叙述。也可以把上述的\033字符用\e替换。可以采用多种配色方案，
			比如上面提到的\033[01;04;32;41m，04表示下划线，32表示前景色
			是绿色，然后41表示背景色是红色。*/
			printf("\033[01;34m "~ stringFormat ~" \033[0m\n");
		}
		else
		{
			writeln("未知系统","");
		}
	}
}

/**写消息文件*/
public class MessageFile
{
	
}

