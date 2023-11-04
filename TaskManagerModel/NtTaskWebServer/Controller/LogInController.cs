using MyWebFramework;
using NtTaskWebServer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
    }
}
