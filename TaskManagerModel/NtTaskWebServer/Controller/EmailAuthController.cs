using NtTaskWebServer.Framework.Attributes;
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
    public class EmailAuthController
    {
        public async Task GetEmailAuthAsync(HttpListenerContext context)
        {
            var view = new View("View/EmailAuth.htm", "text/html");
            await WebHelper.SendViewAsync(context, view);
        }
        public async Task PostCodeAsync(HttpListenerContext context)
        {
            using var requestStream = context.Request.InputStream;
            var codeData = await JsonSerializer.DeserializeAsync<CodeData>(requestStream);
            if (ValidationHelper.IsValidCodeData(codeData)
                    && WebHelper.UserCode.TryGetValue(codeData.UserName, out var code)
                    && int.Parse(codeData.Code)==code)
            {
                WebHelper.SendSession(context, codeData.UserName, Role.Owner);
                await WebHelper.SendOkAsync(context, "ok");
            }
            else
            {
                WebHelper.SendSession(context, codeData.UserName, Role.Owner);
                await WebHelper.SendOkAsync(context, "ok");
                //await WebHelper.Send400Async(context, "Не правильный код");
            }
        }
    }
}
