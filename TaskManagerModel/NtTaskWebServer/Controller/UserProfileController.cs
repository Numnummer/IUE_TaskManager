using MyWebFramework;
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
    public class UserProfileController
    {
        [NeedAuth(Role.Reader)]
        public async Task GetUserProfileAsync(HttpListenerContext context)
        {
            var view = new View("View/UserProfile.htm", "text/html");

            await WebHelper.SendViewAsync(context, view);
        }
        [NeedAuth(Role.Reader)]
        public async Task GetUserDataAsync(HttpListenerContext context)
        {
            var cookie = context.Request.Cookies["session"];
            if (cookie == null)
            {
                WebHelper.Send401(context);
                return;
            }
            var userData = await DatabaseHelper.GetUserDataAsync(cookie.Value.Split(' ')[1]);
            var response = JsonSerializer.Serialize(userData);
            await WebHelper.SendOkAsync(context, response);
        }
    }
}
