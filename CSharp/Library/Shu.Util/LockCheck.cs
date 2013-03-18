using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Shu.Util
{
    public struct LockCheck
    {
        /// <summary>
        /// 
        /// </summary>
        private const int LockFalse = 0;
        /// <summary>
        /// 
        /// </summary>
        private const int LockTrue = 1;

        /// <summary>
        /// 
        /// </summary>
        private int m_LockValue;

        /// <summary>
        /// 锁检查
        /// </summary>
        public LockCheck(bool isValid)
        {
            m_LockValue = isValid == true ? LockTrue : LockFalse;
        }

        /// <summary>
        /// 返回是否有效
        /// </summary>
        /// <returns></returns>
        public bool IsValid() { return Interlocked.CompareExchange(ref m_LockValue, LockTrue, LockTrue) == LockTrue; }
        //Interlocked.CompareExchange 如果 第1个参数 和 第3个参数 中的值相等，则 第2个参数 将存储在  第1个参数 中。否则，不执行操作。
        //比较和交换操作以原子操作的形式执行。无论是否发生交换，CompareExchange 的返回值都是  第1个参数 中的原始值。 

        /// <summary>
        /// 设置为有效
        /// </summary>
        /// <returns></returns>
        public bool SetValid() { return Interlocked.CompareExchange(ref m_LockValue, LockTrue, LockFalse) == LockFalse; }
        
        /// <summary>
        /// 设置为无效
        /// </summary>
        /// <returns></returns>
        public bool SetInvalid() { return Interlocked.CompareExchange(ref m_LockValue, LockFalse, LockTrue) == LockTrue; }
    }
}