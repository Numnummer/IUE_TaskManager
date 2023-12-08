using NtTaskWebServer.Framework.Helpers;
using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
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
            if (isDataWritten)
            {
                WebHelper.SendSession(context, loginData.UserName, Role.Owner);
                await WebHelper.SendOkAsync(context, "User accepted");
                await MailHelper.SendEmail(loginData.Email, "NtTask Registration", $"Вы зарегистрировались под именем {loginData.UserName}");
            }
            else
            {
                await WebHelper.Send400Async(context, "Не корректные данные для регистрации");
            }
        }
    }
}
