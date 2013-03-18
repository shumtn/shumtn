using System;
using System.Threading;

namespace Shu.Util
{
    public struct LockInOut
    {
        /// <summary>
        /// 
        /// </summary>
        private const int TRUE = 1;
        /// <summary>
        /// 
        /// </summary>
        private const int FALSE = 0;

        #region 构造和初始化和清理
        /// <summary>
        /// 表示当前已加入在处理列表中 0 == FALSE 1 == TRUE 
        /// </summary>
        private int m_IsLock;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isLock"></param>
        public LockInOut(bool isLock)
        {
            m_IsLock = isLock == true ? TRUE : FALSE;
        }
        #endregion

        #region 共有方法
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool InLock()
        {
            return Interlocked.CompareExchange(ref m_IsLock, TRUE, FALSE) == FALSE;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OutLock()
        {
            Interlocked.CompareExchange(ref m_IsLock, FALSE, TRUE);
        }
        #endregion
    }
}
