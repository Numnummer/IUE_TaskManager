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
using System.Threading.Tasks;

namespace NtTaskWebServer.Controller
{
    public class LogInController
    {
        public async Task GetLogInAsync(HttpListenerContext context)
        {
            var view = new View("View/LogIn.htm", "text/html");
            await WebHelper.SendViewAsync(context, view);
        }

        public async Task PostLogInAsync(HttpListenerContext context)
        {
            using var requestStream = context.Request.InputStream;
            var loginData = await JsonSerializer.DeserializeAsync<LoginData>(requestStream);
            var isDataExists = await DatabaseHelper.IsLoginDataExistAsync(loginData);
            var code = WebHelper.GetCode(loginData.UserName);
            if (isDataExists && code!=null)
            {
                try
                {
                    var email = (await DatabaseHelper.GetUserDataAsync(loginData.UserName)).Email;
                    var config = new MailConfig()
                    {
                        MailPort=ConfigurationManager.AppSettings.Get("MailPort"),
                        MailServerUrl=ConfigurationManager.AppSettings.Get("MailServer"),
                        Password=ConfigurationManager.AppSettings.Get("MailPassword")
                    };
                    var letter = new Letter()
                    {
                        Body=$"Вы вошли под именем {loginData.UserName}\nВаш код: {code}",
                        From=ConfigurationManager.AppSettings.Get("MailFrom"),
                        Theme="NtTask Enter",
                        To=email
                    };
                    await MailHelper.SendEmail(config, letter);
                    await WebHelper.SendOkAsync(context, "User accepted");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    await WebHelper.Send400Async(context, "Ошибка отправки почты");
                }
            }
            else
            {
                await WebHelper.Send400Async(context, "Не корректные данные для входа");
            }
        }
    }
}
