using Models.Mail;
using NtTaskWebServer.Framework.Helpers;
using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace NtTaskWebServer.Controller
{
    public class RegistrationController
    {
        public async Task GetRegistrationAsync(HttpListenerContext context)
        {
            var view = new View("View/Registration.htm", "text/html");
            await WebHelper.SendViewAsync(context, view);
        }

        public async Task PostRegistrationAsync(HttpListenerContext context)
        {
            using var requestStream = context.Request.InputStream;
            var loginData = await JsonSerializer.DeserializeAsync<LoginData>(requestStream);
            var isDataWritten = await DatabaseHelper.WriteLoginDataAsync(loginData);
            var code = WebHelper.GetCode(loginData.UserName);
            if (isDataWritten && code!=null)
            {
                await WebHelper.SendOkAsync(context, "User accepted");
                var config = new MailConfig()
                {
                    MailPort=ConfigurationManager.AppSettings.Get("MailPort"),
                    MailServerUrl=ConfigurationManager.AppSettings.Get("MailServer"),
                    Password=ConfigurationManager.AppSettings.Get("MailPassword")
                };
                var letter = new Letter()
                {
                    Body=$"Вы зарегистрировались под именем {loginData.UserName}\nВаш код: {code}",
                    From=ConfigurationManager.AppSettings.Get("MailFrom"),
                    Theme="NtTask Registration",
                    To=loginData.Email
                };
                await MailHelper.SendEmail(config, letter);
            }
            else
            {
                await WebHelper.Send400Async(context, "Не корректные данные для регистрации");
            }
        }
    }
}
