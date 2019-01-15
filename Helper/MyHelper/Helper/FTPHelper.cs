using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace MyHelper.Helper
{
    public class FtpHelper
    {
        private readonly string _ftpIp; //FTP服务器ip
        private readonly string _ftpPassword; //FTP服务器密码
        private readonly string _ftpUser; //FTP服务器账号
        private FtpWebRequest _reqFtp; //文件传输客户端

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="ftpIp"></param>
        /// <param name="ftpUser"></param>
        /// <param name="ftpPassword"></param>
        public FtpHelper(string ftpIp, string ftpUser, string ftpPassword)
        {
            this._ftpIp = ftpIp;
            this._ftpUser = ftpUser;
            this._ftpPassword = ftpPassword;
        }

        /// <summary>
        ///     根据文件夹地址拼接ftpURI地址
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetPath(string path)
        {
            return "ftp://" + _ftpIp + path;
        }

        /// <summary>
        ///     连接ftp
        /// </summary>
        /// <param name="uri"></param>
        private void Connect(string uri)
        {
            _reqFtp = (FtpWebRequest) WebRequest.Create(uri); //建立一个ftp连接
            _reqFtp.UseBinary = true; //指定数据传输类型
            _reqFtp.Credentials = new NetworkCredential(_ftpUser, _ftpPassword);
        }

        /// <summary>
        ///     上传
        /// </summary>
        /// <param name="path">文件夹目录</param>
        /// <param name="files">文件byte</param>
        public void Upload(string path, byte[] files)
        {
            CreateDir(path);
            var uri = GetPath(path);
            Connect(uri); //打开一个ftp连接
            //设置请求完成后是否保持连接默认为true
            _reqFtp.KeepAlive = false;
            //指定执行命令
            _reqFtp.Method = WebRequestMethods.Ftp.UploadFile;
            _reqFtp.ContentLength = files.Length;

            //缓存大小
            var bufferLength = 2048;

            var buff = new byte[bufferLength];

            // 打开一个文件流(System.IO.FileStream) 去读上传的文件

            Stream strm = null;
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(files);
                strm = _reqFtp.GetRequestStream();

                var contentLen = ms.Read(buff, 0, bufferLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = ms.Read(buff, 0, bufferLength);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                ms?.Close();
                strm?.Close();
            }
        }

        /// <summary>
        ///     删除指定文件夹
        ///     注意：文件夹必须是空文件夹，否则会删除失败，必须先删除文件夹里面的文件
        /// </summary>
        /// <param name="dirName"></param>
        public void DeleteDir(string dirName)
        {
            var uri = GetPath(dirName);
            Connect(uri);
            try
            {
                _reqFtp.Method = WebRequestMethods.Ftp.RemoveDirectory;
                var response = _reqFtp.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     删除指定文件
        /// </summary>
        /// <param name="fileName">文件目录字符串</param>
        public void DeleteFileName(string fileName)
        {
            var uri = GetPath(fileName);
            Connect(uri);
            try
            {
                _reqFtp.Method = WebRequestMethods.Ftp.DeleteFile;
                var response = _reqFtp.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        ///     获取目录文件夹下的所有文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<string> GetDirtory(string path)
        {
            var strs = new List<string>();
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                var dirPath = GetPath(path);
                Connect(dirPath);
                _reqFtp.Method = WebRequestMethods.Ftp.ListDirectoryDetails; //获取该路径下所有文件夹以及文件
                response = _reqFtp.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                var line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Contains("<DIR>")) //所有文件夹都会带有“<DIR>”这一项
                    {
                        var msg = line.Substring(line.LastIndexOf("<DIR>", StringComparison.Ordinal) + 5).Trim();

                        strs.Add(msg);
                    }

                    line = reader.ReadLine();
                }

                return strs;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取目录出错" + ex.Message);
            }
            finally
            {
                reader?.Close();
                response?.Close();
            }

            return strs;
        }

        /// <summary>
        ///     创建文件夹(没有创建，有则直接返回)
        /// </summary>
        /// <param name="path"></param>
        private void CreateDir(string path)
        {
            var uri = "ftp://" + _ftpIp + GetDir(path); //ftp目录地址
            Connect(uri);
            WebResponse response = null;
            try
            {
                _reqFtp.Method = WebRequestMethods.Ftp.MakeDirectory; //创建文件夹
                response = _reqFtp.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                response?.Close();
            }
        }

        /// <summary>
        ///     获取文件夹目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetDir(string path)
        {
            var index = path.LastIndexOf('/'); //获取最后一个"/"索引位置
            if (index > 0)
            {
                var dir = path.Substring(0, index);
                return dir;
            }

            return path;
        }
    }
}