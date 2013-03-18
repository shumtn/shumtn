using System;

namespace Shu.Util
{
    /// <summary>
    /// 表示定义一组条件并确定指定对象是否符合这些条件的方法
    /// </summary>
    /// <typeparam name="KEY"></typeparam>
    /// <typeparam name="VALUE"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate bool Predicate<KEY, VALUE>(KEY key, VALUE value);
    /// <summary>
    /// 封装一个方法，该方法不返回值
    /// </summary>
    /// <typeparam name="KEY"></typeparam>
    /// <typeparam name="VALUE"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public delegate void Action<KEY, VALUE>(KEY key, VALUE value);
}
