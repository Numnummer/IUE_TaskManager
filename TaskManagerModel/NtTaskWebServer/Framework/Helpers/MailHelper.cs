using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Helpers
{
    public static class MailHelper
    {
        private static readonly string _from = ConfigurationManager.AppSettings["MailString"];
        private static readonly string _password = ConfigurationManager.AppSettings["MailPassword"];
        public static async Task<bool> SendEmail(string to, string theme, string body)
        {
            using var mail = new MailMessage(_from, to);
            mail.Body = body;
            mail.Subject = theme;
            var mailServerUrl = ConfigurationManager.AppSettings["MailServer"];
            var mailPort = int.Parse(ConfigurationManager.AppSettings["MailPort"]);
            using var smtpClient = new SmtpClient(mailServerUrl, mailPort);
            smtpClient.Credentials = new NetworkCredential(_from, _password);
            smtpClient.EnableSsl = true;
            try
            {
                await smtpClient.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
