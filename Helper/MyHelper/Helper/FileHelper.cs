using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper.Helper
{
    /// <summary>
    /// 文件帮助类，把信息写入本地文本中
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 从文本文件中获取token，过期则返回空
        /// </summary>
        /// <param name="folder">文件夹名</param>
        /// <param name="file">文件名</param>
        /// <returns></returns>
        public static string GetCacheByFile(string folder, string file)
        {
            var fileString = ReadToken(folder, file);
            return fileString;
        }

        /// <summary>
        /// 把token写到文档中
        /// </summary>
        /// <param name="content">token值</param>
        /// <param name="folder">文件夹名</param>
        /// <param name="file">文件名</param>
        public static void WriteToken(string content, string folder, string file)
        {
            var filePath = GetFilePath(folder, file);
            File.WriteAllText(filePath, content, Encoding.UTF8);
        }


        /// <summary>
        /// 从文档中获取记录
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ReadToken(string folder, string file)
        {
            var filePath = GetFilePath(folder, file);
            var res = string.Empty;
            if (File.Exists(filePath))
            {
                res = File.ReadAllText(filePath);
            }
            return res;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="folder">文件夹名</param>
        /// <param name="file">文件名</param>
        public static void DeleteFile(string folder, string file)
        {
            var filePath = GetFilePath(folder, file);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        #region 私有方法

        /// <summary>
        /// 拼接文件目录
        /// </summary>
        /// <param name="folder">文件夹名</param>
        /// <param name="file">文件名不带后缀</param>
        /// <returns></returns>
        private static string GetFilePath(string folder, string file)
        {
            return CreatePath(folder) + "\\" + file + ".txt";
        }

        /// <summary>
        /// 获取文件所在文件夹目录
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static string CreatePath(string folder)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrEmpty(folder))
            {
                path = path + folder;
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        #endregion
    }
}
