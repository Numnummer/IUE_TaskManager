using NtTaskWebServer.Framework.Helpers;
using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Middleware
{
    public class AuthorizationMiddleware : Middleware
    {
        public AuthorizationMiddleware(Middleware successor) : base(successor) { }

        public override async Task Invoke(HttpListenerContext context)
        {
            if (WebSettings.IsAuthentificated)
            {
                await Task.Run(async () =>
                {
                    var cookies = context.Request.Cookies;
                    var data = cookies["session"]?.Value;
                    string role = string.Empty;
                    try
                    {
                        role = data.Split(' ')[2];
                    }
                    catch (Exception)
                    {
                        await WebHelper.Send400Async(context, "Не валидная сессия");
                        WebSettings.Role=Role.NoRights;
                        return;
                    }
                    if (Enum.TryParse(typeof(Role), role, false, out var actualRole))
                    {
                        WebSettings.Role=(Role)actualRole;
                        WebHelper.UpdateSession(context);
                        return;
                    }
                    await WebHelper.Send400Async(context, "Не валидная роль");
                    WebSettings.Role=Role.NoRights;
                });
            }
            if (_successor!=null)
            {
                await _successor.Invoke(context);
            }
        }
    }
}
