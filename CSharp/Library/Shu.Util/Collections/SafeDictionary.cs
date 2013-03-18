using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Shu.Util
{
    /// <summary>
    /// 具有线程锁定的Dictionary类
    /// </summary>
    /// <typeparam name="KeyT">字典key类型</typeparam>
    /// <typeparam name="ValueT">字典值类型</typeparam>
    public class SafeDictionary<KeyT, ValueT> : IEnumerable<KeyValuePair<KeyT, ValueT>>
    {
        #region 变量
        /// <summary>
        /// 字典集
        /// </summary>
        private Dictionary<KeyT, ValueT> m_Dictionary = new Dictionary<KeyT, ValueT>();
        /// <summary>
        /// 多线程锁
        /// </summary>
        private ReaderWriterLockSlim m_LockDictionary = new ReaderWriterLockSlim();
        /// <summary>
        /// 空的键集合
        /// </summary>
        private readonly static KeyT[] s_ZeroKeyArray = new KeyT[0];
        /// <summary>
        /// 空的值集合
        /// </summary>
        private readonly static ValueT[] s_ZeroValueArray = new ValueT[0];
        /// <summary>
        /// 空的键值对集合
        /// </summary>
        private readonly static KeyValuePair<KeyT, ValueT>[] s_ZeroKeyValuePairArray = new KeyValuePair<KeyT, ValueT>[0];

        /// <summary>
        /// 字典的键集合
        /// </summary>
        private volatile KeyT[] m_KeyArray = s_ZeroKeyArray;
        /// <summary>
        /// 字典的值集合
        /// </summary>
        private volatile ValueT[] m_ValueArray = s_ZeroValueArray;
        /// <summary>
        /// 字典的键值对集合
        /// </summary>
        private volatile KeyValuePair<KeyT, ValueT>[] m_KeyValuePairArray = s_ZeroKeyValuePairArray;
        /// <summary>
        /// 字典值改变标志
        /// </summary>
        private volatile bool m_bIsValueChange = true;
        #endregion

        #region 构造
        /// <summary>
        /// 默认空构造
        /// </summary>
        public SafeDictionary()
        {

        }

        /// <summary>
        /// 泛型字典
        /// </summary>
        public SafeDictionary(IDictionary<KeyT, ValueT> dictionary)
        {
            m_Dictionary = new Dictionary<KeyT, ValueT>(dictionary);
        }

        /// <summary>
        /// 字典容量
        /// </summary>
        /// <param name="iCapacity"></param>
        public SafeDictionary(int iCapacity)
        {
            m_Dictionary = new Dictionary<KeyT, ValueT>(iCapacity);
        }
        #endregion

        #region 属性
        /// <summary>
        /// 返回字典大小
        /// </summary>
        public int Count
        {
            get { return m_Dictionary.Count; }
        }

        /// <summary>
        /// 根据key，返回对应值
        /// </summary>
        /// <param name="key">泛型key</param>
        /// <returns>泛型值</returns>
        public ValueT this[KeyT key]
        {
            get
            {
                ValueT tempVALUE = default(ValueT);

                m_LockDictionary.EnterReadLock();
                {
                    tempVALUE = m_Dictionary[key];
                }
                m_LockDictionary.ExitReadLock();

                return tempVALUE;
            }
            set
            {
                m_LockDictionary.EnterWriteLock();
                {
                    m_Dictionary[key] = value;

                    m_bIsValueChange = true;
                }
                m_LockDictionary.ExitWriteLock();
            }
        }
        #endregion

        #region 方法

        #region 公开
        /// <summary>
        /// 整加个键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Add(KeyT key, ValueT value)
        {
            m_LockDictionary.EnterWriteLock();
            {
                m_Dictionary[key] = value;

                m_bIsValueChange = true;
            }
            m_LockDictionary.ExitWriteLock();
        }

        /// <summary>
        /// 增加多个键值对
        /// </summary>
        /// <param name="dictionary"></param>
        public void AddRange(IEnumerable<KeyValuePair<KeyT, ValueT>> dictionary)
        {
            m_LockDictionary.EnterWriteLock();
            {
                foreach (KeyValuePair<KeyT, ValueT> keyValuePair in dictionary)
                    m_Dictionary[keyValuePair.Key] = keyValuePair.Value;

                m_bIsValueChange = true;
            }
            m_LockDictionary.ExitWriteLock();
        }

        /// <summary>
        /// 通过键移出一对值
        /// </summary>
        /// <param name="key"></param>
        public void Remove(KeyT key)
        {
            m_LockDictionary.EnterWriteLock();
            {
                if (m_Dictionary.Remove(key) == true)
                    m_bIsValueChange = true;
            }
            m_LockDictionary.ExitWriteLock();
        }

        /// <summary>
        /// 移出多对值
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public int RemoveAll(Predicate<KeyT, ValueT> match)
        {
            KeyValuePair<KeyT, ValueT>[] valueArray = this.ToArray();
            if (valueArray.Length <= 0)
            {
                return 0;
            }

            List<KeyT> removeList = new List<KeyT>(valueArray.Length);

            for (int iIndex = 0; iIndex < valueArray.Length; iIndex++)
            {
                KeyValuePair<KeyT, ValueT> itemT = valueArray[iIndex];
                if (match(itemT.Key, itemT.Value) == true)
                {
                    removeList.Add(itemT.Key);
                }
            }

            if (removeList.Count <= 0)
            {
                return 0;
            }

            int iRemoveCount = 0;

            m_LockDictionary.EnterWriteLock();
            {
                foreach (KeyT itemKey in removeList)
                {
                    if (m_Dictionary.Remove(itemKey) == true)
                    {
                        iRemoveCount++;
                    }
                }

                if (iRemoveCount > 0)
                {
                    m_bIsValueChange = true;
                }
            }
            m_LockDictionary.ExitWriteLock();

            return iRemoveCount;
        }

        /// <summary>
        /// 返回某个对应键的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueT GetValue(KeyT key)
        {
            ValueT returnVALUE = default(ValueT);

            m_LockDictionary.EnterReadLock();
            {
                m_Dictionary.TryGetValue(key, out returnVALUE);
            }
            m_LockDictionary.ExitReadLock();

            return returnVALUE;
        }

        /// <summary>
        /// 返回某个对应键的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(KeyT key, out ValueT value)
        {
            value = default(ValueT);

            bool returnValue = false;

            m_LockDictionary.EnterReadLock();
            {
                returnValue = m_Dictionary.TryGetValue(key, out value);
            }
            m_LockDictionary.ExitReadLock();

            return returnValue;
        }

        /// <summary>
        /// 查询是否包含指定键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(KeyT key)
        {
            bool bIsContains = false;

            m_LockDictionary.EnterReadLock();
            {
                bIsContains = m_Dictionary.ContainsKey(key);
            }
            m_LockDictionary.ExitReadLock();

            return bIsContains;
        }

        /// <summary>
        /// 查询是否包含指定值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(ValueT value)
        {
            bool bIsContains = false;

            m_LockDictionary.EnterReadLock();
            {
                bIsContains = m_Dictionary.ContainsValue(value);
            }
            m_LockDictionary.ExitReadLock();

            return bIsContains;
        }

        /// <summary>
        /// 查询是否包含指定键值对
        /// </summary>
        /// <param name="match"></param>
        /// <param name="findKeyValuePair"></param>
        /// <returns></returns>
        public bool Find(Predicate<KeyT, ValueT> match, out KeyValuePair<KeyT, ValueT> findKeyValuePair)
        {
            findKeyValuePair = new KeyValuePair<KeyT, ValueT>(default(KeyT), default(ValueT));

            KeyValuePair<KeyT, ValueT>[] valueArray = this.ToArray();
            if (valueArray.Length <= 0)
            {
                return false;
            }

            for (int iIndex = 0; iIndex < valueArray.Length; iIndex++)
            {
                KeyValuePair<KeyT, ValueT> keyValuePair = valueArray[iIndex];
                if (match(keyValuePair.Key, keyValuePair.Value) == true)
                {
                    findKeyValuePair = keyValuePair;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 寻找匹配键值对
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public KeyValuePair<KeyT, ValueT>[] FindAll(Predicate<KeyT, ValueT> match)
        {
            List<KeyValuePair<KeyT, ValueT>> keyValuePairList = new List<KeyValuePair<KeyT, ValueT>>();

            KeyValuePair<KeyT, ValueT>[] valueArray = this.ToArray();
            if (valueArray.Length <= 0)
            {
                return keyValuePairList.ToArray();
            }
            else
            {
                keyValuePairList.Capacity = valueArray.Length;
            }

            for (int iIndex = 0; iIndex < valueArray.Length; iIndex++)
            {
                KeyValuePair<KeyT, ValueT> keyValuePair = valueArray[iIndex];
                if (match(keyValuePair.Key, keyValuePair.Value) == true)
                {
                    keyValuePairList.Add(keyValuePair);
                }
            }

            return keyValuePairList.ToArray();
        }

        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<KeyT, ValueT> action)
        {
            KeyValuePair<KeyT, ValueT>[] valueArray = this.ToArray();
            if (valueArray.Length <= 0)
            {
                return;
            }

            for (int iIndex = 0; iIndex < valueArray.Length; iIndex++)
            {
                KeyValuePair<KeyT, ValueT> keyValuePair = valueArray[iIndex];
                action(keyValuePair.Key, keyValuePair.Value);
            }
        }

        /// <summary>
        /// 查询是否存在指定匹配
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool Exists(Predicate<KeyT, ValueT> match)
        {
            KeyValuePair<KeyT, ValueT>[] valueArray = this.ToArray();
            if (valueArray.Length <= 0)
            {
                return false;
            }

            for (int iIndex = 0; iIndex < valueArray.Length; iIndex++)
            {
                KeyValuePair<KeyT, ValueT> keyValuePair = valueArray[iIndex];
                if (match(keyValuePair.Key, keyValuePair.Value) == true)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 清空字典
        /// </summary>
        public void Clear()
        {
            m_LockDictionary.EnterWriteLock();
            {
                m_Dictionary.Clear();

                // 清空
                m_KeyArray = s_ZeroKeyArray;
                m_ValueArray = s_ZeroValueArray;
                m_KeyValuePairArray = s_ZeroKeyValuePairArray;
                m_bIsValueChange = false;
            }
            m_LockDictionary.ExitWriteLock();
        }
        #endregion

        #region 私有
        /// <summary>
        /// 内部缓存键集合,值集合,键值对集合
        /// </summary>
        private void InternalToCached()
        {
            if (m_bIsValueChange == false)
            {
                return;
            }

            m_LockDictionary.EnterReadLock();
            {
                if (m_bIsValueChange == true)
                {
                    m_KeyArray = new KeyT[m_Dictionary.Count];
                    m_ValueArray = new ValueT[m_Dictionary.Count];
                    m_KeyValuePairArray = new KeyValuePair<KeyT, ValueT>[m_Dictionary.Count];

                    int iIndex = 0;
                    foreach (KeyValuePair<KeyT, ValueT> keyValuePair in m_Dictionary)
                    {
                        m_KeyArray[iIndex] = keyValuePair.Key;
                        m_ValueArray[iIndex] = keyValuePair.Value;
                        m_KeyValuePairArray[iIndex] = keyValuePair;
                        ++iIndex;
                    }

                    // 最后设置
                    m_bIsValueChange = false;
                }
            }
            m_LockDictionary.ExitReadLock();
        }
        #endregion

        /// <summary>
        /// 返回所有键值对 
        /// </summary>
        /// <returns>返回所有键值对</returns>
        public KeyValuePair<KeyT, ValueT>[] ToArray()
        {
            InternalToCached();

            return m_KeyValuePairArray;
        }

        /// <summary>
        /// 返回值集合 
        /// </summary>
        /// <returns>返回值集合</returns>
        public ValueT[] ToArrayValues()
        {
            InternalToCached();

            return m_ValueArray;
        }

        /// <summary>
        /// 返回键集合 
        /// </summary>
        /// <returns>返回键集合</returns>
        public KeyT[] ToArrayKeys()
        {
            InternalToCached();

            return m_KeyArray;
        }

        /// <summary>
        /// 返回所有键值对,并清空字典
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<KeyT, ValueT>[] ToArrayAndClear()
        {
            KeyValuePair<KeyT, ValueT>[] keyValuePairArray = s_ZeroKeyValuePairArray;

            m_LockDictionary.EnterWriteLock();
            {
                if (m_bIsValueChange == true)
                {
                    keyValuePairArray = new KeyValuePair<KeyT, ValueT>[m_Dictionary.Count];

                    int iIndex = 0;
                    foreach (KeyValuePair<KeyT, ValueT> keyValuePair in m_Dictionary)
                    {
                        keyValuePairArray[iIndex] = keyValuePair;
                        ++iIndex;
                    }

                    // 最后设置
                    m_bIsValueChange = false;
                }
                else
                {
                    keyValuePairArray = m_KeyValuePairArray;
                }

                // 清空
                m_Dictionary.Clear();
                m_KeyArray = s_ZeroKeyArray;
                m_ValueArray = s_ZeroValueArray;
                m_KeyValuePairArray = s_ZeroKeyValuePairArray;
            }
            m_LockDictionary.ExitWriteLock();

            return keyValuePairArray;
        }

        /// <summary>
        /// 返回所有值集合,并清空字典
        /// </summary>
        /// <returns></returns>
        public ValueT[] ToArrayValuesAndClear()
        {
            ValueT[] valueArray = s_ZeroValueArray;

            m_LockDictionary.EnterWriteLock();
            {
                if (m_bIsValueChange == true)
                {
                    valueArray = new ValueT[m_Dictionary.Count];

                    int iIndex = 0;
                    foreach (KeyValuePair<KeyT, ValueT> keyValuePair in m_Dictionary)
                    {
                        valueArray[iIndex] = keyValuePair.Value;
                        ++iIndex;
                    }

                    // 最后设置
                    m_bIsValueChange = false;
                }
                else
                {
                    valueArray = m_ValueArray;
                }

                // 清空
                m_Dictionary.Clear();
                m_KeyArray = s_ZeroKeyArray;
                m_ValueArray = s_ZeroValueArray;
                m_KeyValuePairArray = s_ZeroKeyValuePairArray;
            }
            m_LockDictionary.ExitWriteLock();

            return valueArray;
        }

        /// <summary>
        /// 返回所有键集合,并清空字典
        /// </summary>
        /// <returns></returns>
        public KeyT[] ToArrayKeysAndClear()
        {
            KeyT[] keyArray = s_ZeroKeyArray;

            m_LockDictionary.EnterWriteLock();
            {
                if (m_bIsValueChange == true)
                {
                    keyArray = new KeyT[m_Dictionary.Count];

                    int iIndex = 0;
                    foreach (KeyValuePair<KeyT, ValueT> keyValuePair in m_Dictionary)
                    {
                        keyArray[iIndex] = keyValuePair.Key;
                        ++iIndex;
                    }

                    // 最后设置
                    m_bIsValueChange = false;
                }
                else
                {
                    keyArray = m_KeyArray;
                }

                // 清空
                m_Dictionary.Clear();
                m_KeyArray = s_ZeroKeyArray;
                m_ValueArray = s_ZeroValueArray;
                m_KeyValuePairArray = s_ZeroKeyValuePairArray;
            }
            m_LockDictionary.ExitWriteLock();

            return keyArray;
        }
        #endregion

        #region 接口
        /// <summary>
        /// 等同于ToArray()
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<KeyT, ValueT>> GetEnumerator()
        {
            KeyValuePair<KeyT, ValueT>[] tempKeyValuePairArray = this.ToArray();
            if (tempKeyValuePairArray == null)
            {
                yield break; //迭代结束
            }

            for (int iIndex = 0; iIndex < tempKeyValuePairArray.Length; iIndex++)
            {
                yield return tempKeyValuePairArray[iIndex];
            }
        }

        /// <summary>
        /// 返回迭代器
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
