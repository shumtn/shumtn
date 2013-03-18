using System;
using System.IO;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace Shu.Util
{
    /// <summary>
    /// 多文本输出
    /// </summary>
    public class MultiTextWriter : TextWriter
    {
        #region 构造和初始化和清理
        #region 私有成员变量
        /// <summary>
        /// 存储多个数据输出流的列表
        /// </summary>
        private TextWriter[] m_TextWriterArray = new TextWriter[0];
        #endregion
        /// <summary>
        /// 添加多个数据输出流至列表中
        /// </summary>
        public MultiTextWriter(params TextWriter[] streamsArray)
        {
            if (streamsArray == null)
                throw new Exception("MultiTextWriter.MultiTextWriter(...) - streamsArray == null error!");

            m_TextWriterArray = new TextWriter[streamsArray.Length];
            for (int iIndex = 0; iIndex < m_TextWriterArray.Length; ++iIndex)
                m_TextWriterArray[iIndex] = streamsArray[iIndex];

            if (m_TextWriterArray.Length <= 0)
                throw new ArgumentException("你须至少指定一个控制台调试输出信息的数据输出流");
        }
        #endregion

        #region 属性覆盖
        /// <summary>
        /// 返回系统的缺省代码页的编码
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
        #endregion

        #region 公有方法
        #region 私有成员变量
        /// <summary>
        /// 只锁定添加和删除操作(因为是数组，其它的地方可以不用锁定的)
        /// </summary>
        private ReaderWriterLockSlim m_OnlyLockAddRemove = new ReaderWriterLockSlim();
        #endregion
        /// <summary>
        /// 添加一个数据输出流
        /// </summary>
        public void Add(TextWriter textWriter)
        {
            if (textWriter == null)
                throw new Exception("MultiTextWriter.Add(...) - textWriter == null error!");

            m_OnlyLockAddRemove.EnterWriteLock();
            {
                // 创建新的TextWriter数组,添加数据,交换数组数据,不需要锁定,没有引用时自动会回收数据
                TextWriter[] tempTextWriter = new TextWriter[m_TextWriterArray.Length + 1];

                for (int iIndex = 0; iIndex < m_TextWriterArray.Length; ++iIndex)
                    tempTextWriter[iIndex] = m_TextWriterArray[iIndex];

                tempTextWriter[m_TextWriterArray.Length] = textWriter;

                m_TextWriterArray = tempTextWriter;
            }
            m_OnlyLockAddRemove.ExitWriteLock();
        }

        /// <summary>
        /// 移去一个数据输出流
        /// </summary>
        public void Remove(TextWriter textWriter)
        {
            if (textWriter == null)
                throw new Exception("MultiTextWriter.Remove(...) - textWriter == null error!");

            m_OnlyLockAddRemove.EnterWriteLock();
            {
                List<TextWriter> textWriterList = new List<TextWriter>();

                for (int iIndex = 0; iIndex < m_TextWriterArray.Length; ++iIndex)
                {
                    TextWriter itemTextWriter = m_TextWriterArray[iIndex];

                    if (itemTextWriter != textWriter)
                        textWriterList.Add(itemTextWriter);
                }

                m_TextWriterArray = textWriterList.ToArray();
            }
            m_OnlyLockAddRemove.ExitWriteLock();
        }
        #endregion

        #region 方法覆盖
        /// <summary>
        /// 在数据输出流中写入一个字符信息
        /// </summary>
        public override void Write(char charWrite)
        {
            for (int iIndex = 0; iIndex < m_TextWriterArray.Length; ++iIndex)
                m_TextWriterArray[iIndex].Write(charWrite);
        }

        /// <summary>
        /// 在数据输出流中写入一行文本信息
        /// </summary>
        public override void WriteLine(string strLine)
        {
            for (int iIndex = 0; iIndex < m_TextWriterArray.Length; ++iIndex)
                m_TextWriterArray[iIndex].WriteLine(strLine);
        }

        /// <summary>
        /// 在数据输出流中写入一行具有参数的文本信息
        /// </summary>
        public override void WriteLine(string strLine, params object[] args)
        {
            WriteLine(string.Format(strLine, args));
        }
        #endregion
    }
}
