using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper.Helper
{
    /// <summary>
    /// 发送邮件帮助类
    /// </summary>
    public class EmailHelper
    {
        private readonly string _emailSmtp;  //邮箱服务器
        private readonly int _emailPort;  //邮箱端口
        private readonly string _emailAccount; //邮箱账号
        private readonly string _emailPwd;  //邮箱密码
        private readonly string _displayName; //发送者邮箱别名

        public EmailHelper(string emailSmtp, int emailPort, string emailAccount, string emailPwd, string displayName)
        {
            _emailSmtp = emailSmtp;
            _emailPort = emailPort;
            _emailAccount = emailAccount;
            _emailPwd = emailPwd;
            _displayName = displayName;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public bool Send(string to, string subject, string body, string displayName)
        {
            //初始化邮件类
            var smtpClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = false,
                Host = _emailSmtp,  //服务器
                Port = _emailPort,  //端口
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(_emailAccount, _emailPwd)
            };

            if (string.IsNullOrEmpty(displayName))
            {
                displayName = _displayName;
            }

            var message = GetMailMessageNoAttachments(displayName, to, subject, body);
            try
            {
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                smtpClient.Dispose();
            }
        }


        /// <summary>
        /// 构建邮件内容不带附件
        /// </summary>
        /// <param name="displayName">发件人别名</param>
        /// <param name="to">收件人邮箱地址</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件内容</param>
        /// <returns></returns>
        private MailMessage GetMailMessageNoAttachments(string displayName,string to,string subject,string body)
        {
            var message = new MailMessage {From = new MailAddress(_emailAccount, displayName, Encoding.UTF8)};//使用别名初始化
            message.To.Add(to);
            message.Subject = subject;
            message.SubjectEncoding = Encoding.UTF8;
            message.IsBodyHtml = true; //设置正文为html，可以正常显示THML页面内容
            message.Body = body;
            message.BodyEncoding = Encoding.UTF8;
            return message;
        }

        /// <summary>
        /// 构建邮件内容对象带附件
        /// </summary>
        /// <param name="displayName">发送者别名</param>
        /// <param name="to">收件者地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="stream">附件流</param>
        /// <param name="contentType">附件mime字符串</param>
        /// <param name="name">附件名称</param>
        /// <returns></returns>
        private MailMessage GetMailMessageContainAttachments(string displayName, string to, string subject, string body,
            Stream stream, string contentType, string name)
        {
            var message = GetMailMessageNoAttachments(displayName, to, subject, body);
            var type = new ContentType
            {
                MediaType = contentType,
                Name = name
            };
            if (stream == null) return message;
            var attac = new Attachment(stream, type);
            message.Attachments.Add(attac);
            return message;
        }
    }
}
