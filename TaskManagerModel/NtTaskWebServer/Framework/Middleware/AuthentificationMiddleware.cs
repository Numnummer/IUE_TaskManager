using NtTaskWebServer.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Middleware
{
    public class AuthentificationMiddleware : Middleware
    {
        public AuthentificationMiddleware(Middleware successor) : base(successor) { }

        public override async Task Invoke(HttpListenerContext context)
        {
            await Task.Run(() =>
            {
                var cookies = context.Request.Cookies;
                var session = cookies["session"]?.Value;
                try
                {
                    if (session==null || !SessionHelper.IsSessionExist(session, cookies))
                    {
                        WebSettings.IsAuthentificated=false;
                    }
                    else
                    {
                        WebSettings.IsAuthentificated=true;
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

            });
            if (_successor!=null)
            {
                await _successor.Invoke(context);
            }
        }
    }
}
