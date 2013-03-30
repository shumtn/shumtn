/**
 * 常用的异常类模块
 * License: BSD
 * Authors: Lucifer (786325481@QQ.com)
 * Copyright: Copyright (C) 2008 Lucifer. All rights reserved.
 */
module shu.exception;

public import core.exception;

/**
 * 在 Phoenix Project 中所使用的异常基类。所有的异常均继承该类。
 */
public class BaseException : Exception
{

    private Exception theInnerException;

    public this(string message, Exception innerException)
    {
        super(message);
        theInnerException = innerException;
    }
        this(string message)
        {
                this(message, null);
        }
        public this()
        {
                this(null, null);
        }

    public override string toString()
    {
        string str = message;

        if (str !is null)
            str = className ~ ": " ~ str;
        if (theInnerException !is null)
            str ~= " ---> " ~ theInnerException.toString();

        return str;
    }

    public final Exception innerException()
    {
        return theInnerException;
    }

    public string message()
    {
        if (msg != null)
            return msg;

        return "An exception of type " ~ className ~ " was thrown.";
    }

    private string className()
    {
        string str = this.classinfo.name;
        for (auto i = str.length - 1; i >= 0; i--)
        {
            if (str[i] == '.' && (i + 1) < str.length)
                return str[(i + 1) .. $];
        }
        return null;
    }

}

/** 在无法实现请求的方法或操作时引发的异常。*/
public class NotImplementedException : BaseException
{
    this(string message , Exception innerException)
    {
        super(message, innerException);
    }
        this(string message)
        {
                this(message, null);
        }
        this()
        {
                this("The operation is not implemented.", null);
        }
}

/** 在选中的上下文中所进行的算术运算、类型转换或转换操作导致溢出时引发的异常。*/
public class OverflowException : BaseException
{
    this(string message, Exception innerException)
    {
        super(message, innerException);
    }
        this(string message)
        {
                this(message, null);
        }
        this()
        {
                this("Arithmetic operation resulted in an overflow.", null);
        }
}

/** 在向方法提供的其中一个参数无效时引发的异常。*/
public class ArgumentException : BaseException
{
    private string theParamName;

    this(string message, string paramName, Exception innerException)
    {
        this.theParamName = paramName;
        super(message, innerException);
    }
        this(string message, string paramName)
        {
                this(message, paramName, null);
        }
        this(string message)
        {
                this(message, null, null);
        }
        this()
        {
                this("Argument does not fall within the expected range.", null, null);
        }

    /** 获取导致该异常的参数的名称。*/
    public string paramName() { return this.theParamName; }
}

/** 当参数值超出调用的方法所定义的允许取值范围时引发的异常。*/
public class ArgumentOutOfRangeException : ArgumentException
{
    this(string message, string paramName, Exception innerException)
    {
        super(message, paramName, innerException);
    }
        this(string message, string paramName)
        {
                this(message, paramName, null);
        }
        this(string message)
        {
                this(message, null, null);
        }
        this()
        {
                this("Argument is out of range.", null, null);
        }
}

/**
 * 当方法调用对于对象的当前状态无效时引发的异常。
 */
public class InvalidOperationException : BaseException
{
    this(string message, Exception innerException)
    {
        super(message, innerException);
    }
        this(string message)
        {
                this(message, null);
        }
        this()
        {
                this("Specified cast is not valid.",null);
        }
}

/**
 * 当调用的方法不受支持，或试图读取、查找或写入不支持调用功能的流时引发的异常。
 */
public class NotSupportedException : BaseException
{
    this(string message, Exception innerException)
    {
        super(message, innerException);
    }
        this(string message)
        {
                this(message, null);
        }
        this()
        {
                this("The operation is not supported.", null);
        }
}
