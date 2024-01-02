using NtTaskWebServer.Framework.Helpers;
using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
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
                await WebHelper.SendOkAsync(context, "User accepted");
                await MailHelper.SendEmail(loginData.Email, "NtTask Enter", $"Вы вошли под именем {loginData.UserName}\nВаш код: {code}");
            }
            else
            {
                await WebHelper.Send400Async(context, "Не корректные данные для входа");
            }
        }
    }
}
