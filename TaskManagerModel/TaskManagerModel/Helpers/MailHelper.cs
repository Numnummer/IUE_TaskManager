using Models.Mail;
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
        public static async Task<bool> SendEmail(MailConfig mailConfig, Letter letter)
        {
            using var mail = new MailMessage(letter.From, letter.To);
            mail.Body = letter.Body;
            mail.Subject = letter.Theme;
            var mailServerUrl = mailConfig.MailServerUrl;
            var mailPort = int.Parse(mailConfig.MailPort);
            using var smtpClient = new SmtpClient(mailServerUrl, mailPort);
            smtpClient.Credentials = new NetworkCredential(letter.From, mailConfig.Password);
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
