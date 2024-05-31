using log4net;
using Mita.Business.Base;
using Mita.Business.BusinessServices.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading;

namespace Mita.Business.BusinessServices
{
    public class EmailSendService : BaseService<EmailSendService>
    {
        /// <summary>
        /// 	Logger
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public class Attachment
        {
            public string FileName { get; set; }
            public byte[] Content { get; set; }
        }

        public void SendEmail(List<string> toAddress, string subject, string body, 
            string emailUsername, string emailPassword, List<Attachment> attachments, bool isbodyHTML)
        {
            var message = new MailMessage();
            var smtp = new SmtpClient();

            message.From = new MailAddress(EmailSendingConfig.GetSystemConfig().EmailFromAddress);

            for (int i = 0; i < toAddress.Count; i++)
            {
                message.To.Add(toAddress[i]);
            }

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isbodyHTML;

            var mapStreamFile = new Dictionary<Attachment, MemoryStream>();
            try
            {
                foreach (var attachment in attachments)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    mapStreamFile.Add(attachment, ms);

                    ms.Write(attachment.Content, 0, attachment.Content.Length);
                    ms.Position = 0;

                    System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("application/vnd.ms-excel");
                    ct.Name = attachment.FileName;
                    ct.CharSet = "UTF-8";
                    System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(ms, ct);
                    attach.ContentDisposition.FileName = attachment.FileName;
                    message.Attachments.Add(attach);
                }

                smtp.Port = EmailSendingConfig.GetSystemConfig().EmailPort;
                smtp.Host = EmailSendingConfig.GetSystemConfig().EmailHost;
                smtp.EnableSsl = smtp.Port == 587 || smtp.Port == 465;
                smtp.UseDefaultCredentials = EmailSendingConfig.GetSystemConfig().UseDefaultCredentials;
                smtp.Credentials = new NetworkCredential(EmailSendingConfig.GetSystemConfig().EmailUsername, EmailSendingConfig.GetSystemConfig().EmailPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                _logger.DebugFormat("Bắt đầu gửi mail cho: {0} ! ", FormatListData(toAddress));
                smtp.Send(message);
                _logger.DebugFormat("Send mail to address: {0} successfully ! ", FormatListData(toAddress));

                
            }
            finally
            {
                foreach (var stream in mapStreamFile.Values)
                {
                    stream.Close();
                }
            }
        }

        public void SendEmailAsync(List<string> toAddress, string subject, string body, string emailUsername, string emailPassword, List<Attachment> attachments,
            bool isbodyHTML)
        {
            Thread threadStart = new Thread(() =>
            {
                try
                {
                    SendEmail(toAddress, subject, body, emailUsername, emailPassword, attachments, isbodyHTML);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                }
            });
            threadStart.Start();
        }
    }

}
