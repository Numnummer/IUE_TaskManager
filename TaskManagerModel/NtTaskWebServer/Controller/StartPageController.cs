using MyWebFramework;
using NtTaskWebServer.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Controller
{
    public class StartPageController
    {
        public async Task GetStartPageAsync(HttpListenerContext context)
        {
            var view = new View("View/StartPage.htm", "text/html");
            await WebHelper.SendViewAsync(context, view);
        }
    }
}
