using System;
using System.Data;
using System.Text;

namespace Shu.Util
{
    public class Open
    {
        /// <summary>
        /// 取得文件后缀
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>string</returns>
        public static string GetFileExtends(string fileName)
        {
            string ext = null;
            if (fileName.IndexOf('.') > 0)
            {
                string[] fs = fileName.Split('.');
                ext = fs[fs.Length - 1];
            }
            return ext;
        }

        /// <summary>
        /// 生成一个文件
        /// </summary>
        /// <param name="fileContent">文件内容</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>bool</returns>
        public static bool WriteFile(string fileContent, string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                byte[] bcontent = null;

                using (System.IO.FileStream fs = System.IO.File.Create(filePath))
                {
                    bcontent = Encoding.UTF8.GetBytes(fileContent);
                    fs.Write(bcontent, 0, bcontent.Length);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 读入文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>string</returns>
        public static string ReadWrite(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    return System.IO.File.ReadAllText(filePath, Encoding.UTF8); //Encoding.GetEncoding("gb2312")
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileContent">文件内容</param>
        /// <returns>bool</returns>
        public static bool UpdateFile(string filePath, string fileContent)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    //文件不存在，生成文件
                    System.IO.FileStream Fs = System.IO.File.Create(filePath);
                    byte[] bcontent = Encoding.UTF8.GetBytes(fileContent);
                    Fs.Write(bcontent, 0, bcontent.Length);
                    Fs.Close();
                    Fs = null;
                    return true;
                }

                System.IO.FileStream FileRead = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                System.IO.StreamReader FileReadWord = new System.IO.StreamReader(FileRead, Encoding.Default);
                string OldString = FileReadWord.ReadToEnd().ToString();
                OldString = OldString + fileContent;
                //把新的内容重新写入 
                System.IO.StreamWriter FileWrite = new System.IO.StreamWriter(FileRead, Encoding.Default);
                FileWrite.Write(OldString);
                //关闭 
                FileWrite.Close();
                FileReadWord.Close();
                FileRead.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>bool</returns>
        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 遍历一个目录下的全部文件
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <returns>DataTable</returns>
        public static DataTable GetFile(string filePath)
        {
            DataTable table = new DataTable();
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("fullname", typeof(string));
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(filePath);

            foreach (System.IO.FileInfo dChild in dir.GetFiles("*"))
            {
                //如果用GetDirectories("ab*"),那么全部以ab开头的目录会被显示
                DataRow Dr = table.NewRow();
                Dr["name"] = dChild.Name; //打印文件名
                Dr["fullname"] = dChild.FullName; //打印路径和文件名
                table.Rows.Add(Dr);

            }
            return table;
        }
    }
}
