using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Shu.Util
{
    /// <summary>
    /// 安全的值的集
    /// </summary>
    public class SafeHashSet<KeyT> : IEnumerable<KeyT>
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public SafeHashSet()
        {

        }

        /// <summary>
        /// 构造
        /// </summary>
        public SafeHashSet(IEnumerable<KeyT> collection)
        {
            m_HashSet = new HashSet<KeyT>(collection);
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取集中包含的元素数
        /// </summary>
        public int Count
        {
            get { return m_HashSet.Count; }
        }
        #endregion

        #region 方法
        #region 私有成员变量
        /// <summary>
        /// 当前集
        /// </summary>
        private HashSet<KeyT> m_HashSet = new HashSet<KeyT>();
        /// <summary>
        /// 多线程锁
        /// </summary>
        private ReaderWriterLockSlim m_LockHashSet = new ReaderWriterLockSlim();
        #endregion
        /// <summary>
        /// 将指定的元素添加到集中
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="realm"></param>
        public void Add(KeyT key)
        {
            m_LockHashSet.EnterWriteLock();
            {
                m_HashSet.Add(key);

                m_bIsValueChange = true;
            }
            m_LockHashSet.ExitWriteLock();
        }

        /// <summary>
        /// 将指定的元素集合添加到集中
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="realm"></param>
        public void AddRange(IEnumerable<KeyT> collection)
        {
            m_LockHashSet.EnterWriteLock();
            {
                foreach (KeyT key in collection)
                {
                    m_HashSet.Add(key);
                }

                m_bIsValueChange = true;
            }
            m_LockHashSet.ExitWriteLock();
        }

        /// <summary>
        /// 从 SafeHashSet 对象中移除指定的元素
        /// </summary>
        /// <param name="serial"></param>
        public void Remove(KeyT key)
        {
            m_LockHashSet.EnterWriteLock();
            {
                if (m_HashSet.Remove(key) == true)
                {
                    m_bIsValueChange = true;
                }
            }
            m_LockHashSet.ExitWriteLock();
        }

        /// <summary>
        /// 从当前 SafeHashSet 对象中移除指定集合中的所有元素。
        /// </summary>
        /// <param name="other"></param>
        public void ExceptWith(IEnumerable<KeyT> other)
        {
            m_LockHashSet.EnterWriteLock();
            {
                m_HashSet.ExceptWith(other);
                m_bIsValueChange = true;
            }
            m_LockHashSet.ExitWriteLock();
        }

        /// <summary>
        /// 从当前 SafeHashSet 对象中移除指定匹配条件的所有元素。
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public int RemoveAll(Predicate<KeyT> match)
        {
            KeyT[] keyArray = this.ToArray();
            if (keyArray.Length <= 0)
            {
                return 0;
            }

            List<KeyT> removeList = new List<KeyT>(keyArray.Length);

            for (int iIndex = 0; iIndex < keyArray.Length; iIndex++)
            {
                KeyT key = keyArray[iIndex];
                if (match(key) == true)
                {
                    removeList.Add(key);
                }
            }

            if (removeList.Count <= 0)
            {
                return 0;
            }

            int iRemoveCount = 0;

            m_LockHashSet.EnterWriteLock();
            {
                foreach (KeyT itemKey in removeList)
                {
                    if (m_HashSet.Remove(itemKey) == true)
                    {
                        iRemoveCount++;
                    }
                }

                if (iRemoveCount > 0)
                {
                    m_bIsValueChange = true;
                }
            }
            m_LockHashSet.ExitWriteLock();

            return iRemoveCount;
        }

        /// <summary>
        /// 确定 SafeHashSet 对象是否包含指定的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(KeyT key)
        {
            bool bIsContains = false;

            m_LockHashSet.EnterReadLock();
            {
                bIsContains = m_HashSet.Contains(key);
            }
            m_LockHashSet.ExitReadLock();

            return bIsContains;
        }

        /// <summary>
        /// 修改当前的 SafeHashSet 对象，以仅包含该对象和指定集合中存在的元素。
        /// </summary>
        /// <param name="other"></param>
        public void IntersectWith(IEnumerable<KeyT> other)
        {
            m_LockHashSet.EnterWriteLock();
            {
                m_HashSet.IntersectWith(other);
                m_bIsValueChange = true;
            }
            m_LockHashSet.ExitWriteLock();
        }

        /// <summary>
        /// 确定 SafeHashSet 对象是否为指定集合的真子集。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsProperSubsetOf(IEnumerable<KeyT> other)
        {
            bool bIsOK = false;

            m_LockHashSet.EnterReadLock();
            {
                bIsOK = m_HashSet.IsProperSubsetOf(other);
            }
            m_LockHashSet.ExitReadLock();

            return bIsOK;
        }

        /// <summary>
        /// 确定 SafeHashSet 对象是否为指定集合的真超集。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsProperSupersetOf(IEnumerable<KeyT> other)
        {
            bool bIsOK = false;

            m_LockHashSet.EnterReadLock();
            {
                bIsOK = m_HashSet.IsProperSupersetOf(other);
            }
            m_LockHashSet.ExitReadLock();

            return bIsOK;
        }

        /// <summary>
        /// 确定 SafeHashSet 对象是否为指定集合的子集。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsSubsetOf(IEnumerable<KeyT> other)
        {
            bool bIsOK = false;

            m_LockHashSet.EnterReadLock();
            {
                bIsOK = m_HashSet.IsSubsetOf(other);
            }
            m_LockHashSet.ExitReadLock();

            return bIsOK;
        }

        /// <summary>
        /// 确定 SafeHashSet 对象是否为指定集合的超集。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsSupersetOf(IEnumerable<KeyT> other)
        {
            bool bIsOK = false;

            m_LockHashSet.EnterReadLock();
            {
                bIsOK = m_HashSet.IsSupersetOf(other);
            }
            m_LockHashSet.ExitReadLock();

            return bIsOK;
        }

        /// <summary>
        /// 确定当前的 SafeHashSet 对象是否与指定的集合重叠。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlaps(IEnumerable<KeyT> other)
        {
            bool bIsOK = false;

            m_LockHashSet.EnterReadLock();
            {
                bIsOK = m_HashSet.Overlaps(other);
            }
            m_LockHashSet.ExitReadLock();

            return bIsOK;
        }

        /// <summary>
        /// 确定 SafeHashSet 对象与指定的集合中是否包含相同的元素。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool SetEquals(IEnumerable<KeyT> other)
        {
            bool bIsOK = false;

            m_LockHashSet.EnterReadLock();
            {
                bIsOK = m_HashSet.SetEquals(other);
            }
            m_LockHashSet.ExitReadLock();

            return bIsOK;
        }

        /// <summary>
        /// 修改当前的 SafeHashSet 对象，以仅包含该对象或指定集合中存在的元素（但不可同时包含两者中的元素）。
        /// </summary>
        /// <param name="other"></param>
        public void SymmetricExceptWith(IEnumerable<KeyT> other)
        {
            m_LockHashSet.EnterWriteLock();
            {
                m_HashSet.SymmetricExceptWith(other);
                m_bIsValueChange = true;
            }
            m_LockHashSet.ExitWriteLock();
        }

        /// <summary>
        /// 修改当前的 SafeHashSet 对象，以包含该对象本身和指定集合中存在的所有元素。
        /// </summary>
        /// <param name="other"></param>
        public void UnionWith(IEnumerable<KeyT> other)
        {
            m_LockHashSet.EnterWriteLock();
            {
                m_HashSet.UnionWith(other);
                m_bIsValueChange = true;
            }
            m_LockHashSet.ExitWriteLock();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            m_LockHashSet.EnterWriteLock();
            {
                m_HashSet.Clear();

                // 清空
                m_KeyArray = s_ZeroArray;
                m_bIsValueChange = false;
            }
            m_LockHashSet.ExitWriteLock();
        }

        #region 私有成员变量
        #region 私有常量
        /// <summary>
        /// 
        /// </summary>
        private readonly static KeyT[] s_ZeroArray = new KeyT[0];
        #endregion
        /// <summary>
        /// 
        /// </summary>
        private volatile KeyT[] m_KeyArray = s_ZeroArray;
        /// <summary>
        /// 
        /// </summary>
        private volatile bool m_bIsValueChange = true;
        #endregion

        #region 私有方法
        /// <summary>
        /// 
        /// </summary>
        private void InternalToCached()
        {
            if (m_bIsValueChange == false)
                return;

            m_LockHashSet.EnterReadLock();
            {
                if (m_bIsValueChange == true)
                {
                    m_KeyArray = new KeyT[m_HashSet.Count];

                    int iIndex = 0;
                    foreach (KeyT keyValuePair in m_HashSet)
                    {
                        m_KeyArray[iIndex] = keyValuePair;
                        ++iIndex;
                    }

                    // 最后设置
                    m_bIsValueChange = false;
                }
            }
            m_LockHashSet.ExitReadLock();
        }
        #endregion
        /// <summary>
        /// 这里假设读非常多，写比较少。 
        /// </summary>
        /// <returns></returns>
        public KeyT[] ToArray()
        {
            InternalToCached();

            return m_KeyArray;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public KeyT[] ToArrayAndClear()
        {
            KeyT[] keyArray = s_ZeroArray;

            m_LockHashSet.EnterWriteLock();
            {
                if (m_bIsValueChange == true)
                {
                    keyArray = new KeyT[m_HashSet.Count];

                    int iIndex = 0;
                    foreach (KeyT keyValuePair in m_HashSet)
                    {
                        keyArray[iIndex] = keyValuePair;
                        ++iIndex;
                    }

                    // 最后设置
                    m_bIsValueChange = false;
                }
                else
                    keyArray = m_KeyArray;

                // 清空
                m_HashSet.Clear();
                m_KeyArray = s_ZeroArray;
            }
            m_LockHashSet.ExitWriteLock();

            return keyArray;
        }
        #endregion

        #region 接口
        /// <summary>
        /// 等同于ToArray()
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyT> GetEnumerator()
        {
            KeyT[] tempKeyArray = this.ToArray();
            if (tempKeyArray == null)
            {
                yield break;
            }

            for (int iIndex = 0; iIndex < tempKeyArray.Length; iIndex++)
            {
                yield return tempKeyArray[iIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}

