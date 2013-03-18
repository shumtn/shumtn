using System;
using System.Data;

namespace Shu.Util
{
    /// <summary>
    /// 目录操作
    /// </summary>
    public class Paths
    {
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="folderName">目录名称</param>
        /// <returns>bool</returns>
        public static bool Create(string folderName)
        {
            if (folderName.Trim().Length > 0)
            {
                try
                {
                    if (!System.IO.Directory.Exists(folderName))
                    {
                        System.IO.Directory.CreateDirectory(folderName);
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除整个文件夹及其字文件夹和文件
        /// </summary>
        /// <param name="folderName">目录名称</param>
        /// <returns>bool</returns>
        public static bool Remove(string folderName)
        {
            if (folderName.Trim().Length > 0)
            {
                try
                {
                    if (System.IO.Directory.Exists(folderName))
                    {
                        System.IO.Directory.Delete(folderName, true);
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 删除整个目录和文件
        /// </summary>
        /// <param name="folderPathName">目录名称</param>
        /// <returns>bool</returns>
        public static bool RemoveFolder(string folderPathName)
        {
            try
            {
                System.IO.DirectoryInfo delFolder = new System.IO.DirectoryInfo(folderPathName);
                if (delFolder.Exists)
                {
                    delFolder.Delete();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 遍历目录
        /// </summary>
        /// <param name="filePath">目录路径</param>
        /// <returns>DataTable</returns>
        public static DataTable Walk(string filePath)
        {
            DataTable table = new DataTable();
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("fullname", typeof(string));
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(filePath);

            foreach (System.IO.DirectoryInfo dChild in dir.GetDirectories("*"))
            {
                //如果用GetDirectories("ab*"),那么全部以ab开头的目录会被显示
                DataRow Dr = table.NewRow();
                Dr["name"] = dChild.Name; //打印目录名
                Dr["fullname"] = dChild.FullName; //打印路径和目录名
                table.Rows.Add(Dr);

            }
            return table;
        }
    }
}
