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
    public class UserProfileController
    {
        public async Task GetUserProfileAsync(HttpListenerContext context)
        {
            var view = new View("View/UserProfile.htm", "text/html");
            await WebHelper.SendViewAsync(context, view);
        }
    }
}
