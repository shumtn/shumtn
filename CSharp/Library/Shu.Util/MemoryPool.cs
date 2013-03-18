using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;

namespace Shu.Util
{
    public class MemoryPool<T> where T : new()
    {
        #region 共有的结构
        /// <summary>
        /// 
        /// </summary>
        public struct PoolInfo
        {
            #region 共有属性
            #region 私有成员变量
            /// <summary>
            /// 
            /// </summary>
            private string m_strName;
            #endregion
            /// <summary>
            /// 
            /// </summary>
            public string Name
            {
                get { return m_strName; }
                internal set { m_strName = value; }
            }

            #region 私有成员变量
            /// <summary>
            /// 
            /// </summary>
            private int m_iFreeCount;
            #endregion
            /// <summary>
            /// 
            /// </summary>
            public int FreeCount
            {
                get { return m_iFreeCount; }
                internal set { m_iFreeCount = value; }
            }

            #region 私有成员变量
            /// <summary>
            /// 
            /// </summary>
            private int m_iInitialCapacity;
            #endregion
            /// <summary>
            /// 
            /// </summary>
            public int InitialCapacity
            {
                get { return m_iInitialCapacity; }
                internal set { m_iInitialCapacity = value; }
            }

            #region 私有成员变量
            /// <summary>
            /// 
            /// </summary>
            private int m_iCurrentCapacity;
            #endregion
            /// <summary>
            /// 
            /// </summary>
            public int CurrentCapacity
            {
                get { return m_iCurrentCapacity; }
                internal set { m_iCurrentCapacity = value; }
            }
            #region 私有成员变量
            /// <summary>
            /// 
            /// </summary>
            private int m_iGrowFactor;
            #endregion
            /// <summary>
            /// 增长因子
            /// </summary>
            public int GrowFactor
            {
                get { return m_iGrowFactor; }
                internal set { m_iGrowFactor = value; }
            }
            #region 私有成员变量
            /// <summary>
            /// 
            /// </summary>
            private int m_iMisses;
            #endregion
            /// <summary>
            /// 
            /// </summary>
            public int Misses
            {
                get { return m_iMisses; }
                internal set { m_iMisses = value; }
            }
            #endregion
        }
        #endregion

        #region 私有成员变量
        /// <summary>
        /// 内存池的名字
        /// </summary>
        private string m_Name = string.Empty;
        /// <summary>
        /// 内存池的容量
        /// </summary>
        private int m_InitialCapacity = 0;
        /// <summary>
        /// 内存池再申请的增长量
        /// </summary>
        private int m_GrowFactor = 100;
        /// <summary>
        /// 内存池的容量不足时再次请求数据的次数
        /// </summary>
        private int m_Misses = 0;
        /// <summary>
        /// 内存池
        /// </summary>
        private ConcurrentQueue<T> m_FreePool = new ConcurrentQueue<T>();
        #endregion

        #region 构造和初始化和清理
        /// <summary>
        /// 初始化内存池
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="iInitialCapacity"></param>
        public MemoryPool(string strName, int iInitialCapacity, int iGrowFactor)
        {
            m_Name = strName;
            m_GrowFactor = iGrowFactor;
            m_InitialCapacity = iInitialCapacity;

            for (int iIndex = 0; iIndex < iInitialCapacity; ++iIndex)
            {
                m_FreePool.Enqueue(new T());
            }
        }
        #endregion

        #region 共有方法
        /// <summary>
        /// 内存池请求数据
        /// </summary>
        /// <returns></returns>
        public T AcquirePoolContent()
        {
            T returnT = default(T);

            do
            {
                if (m_FreePool.Count > 0)
                {
                    if (m_FreePool.TryDequeue(out returnT) == true)
                        break;
                }

                ++m_Misses;

                for (int iIndex = 0; iIndex < m_InitialCapacity; ++iIndex)
                    m_FreePool.Enqueue(new T());

                if (m_FreePool.TryDequeue(out returnT) == true)
                    break;
            } while (true);

            return returnT;
        }

        /// <summary>
        /// 内存池释放数据
        /// </summary>
        /// <param name="TContent"></param>
        public void ReleasePoolContent(T contentT)
        {
            if (contentT == null)
                throw new ArgumentNullException("TContent", "TContent == null");

            m_FreePool.Enqueue(contentT);
        }

        /// <summary>
        /// 释放内存池内全部的数据
        /// </summary>
        public void Free()
        {
            m_FreePool = new ConcurrentQueue<T>();
        }

        /// <summary>
        /// 给出内存池的详细信息
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="iFreeCount"></param>
        /// <param name="iInitialCapacity"></param>
        /// <param name="iCurrentCapacity"></param>
        /// <param name="iMisses"></param>
        public MemoryPool<T>.PoolInfo GetPoolInfo()
        {
            MemoryPool<T>.PoolInfo poolInfo = new MemoryPool<T>.PoolInfo();

            poolInfo.Name = m_Name;
            poolInfo.FreeCount = m_FreePool.Count;
            poolInfo.InitialCapacity = m_InitialCapacity;
            poolInfo.GrowFactor = m_GrowFactor;
            poolInfo.CurrentCapacity = m_InitialCapacity + m_GrowFactor * m_Misses;
            poolInfo.Misses = m_Misses;

            return poolInfo;
        }
        #endregion
    }
}