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

        [NeedAuth(Role.Owner)]
        public async Task PostUsersAsync(HttpListenerContext context)
        {
            using var stream = context.Request.InputStream;
            var prompt = await JsonSerializer.DeserializeAsync<string>(stream);
            if (prompt == null)
            {
                await WebHelper.Send400Async(context, "prompt not valid");
                return;
            }
            var users = await DatabaseHelper.GetUsersAsync(prompt);
            await WebHelper.SendJsonObjectAsync(context, users);
        }

        [NeedAuth(Role.Owner)]
        public async Task PostOrderAsync(HttpListenerContext context)
        {
            using var stream = context.Request.InputStream;
            var friendName = await JsonSerializer.DeserializeAsync<string>(stream);
            if (friendName == null)
            {
                await WebHelper.Send400Async(context, "user name not valid");
                return;
            }
            var userName = SessionHelper.GetUserName(context);
            if (friendName==userName)
            {
                await WebHelper.Send400Async(context, "cannot add self as friend");
                return;
            }
            if (await DatabaseHelper.AddOrderAsync(userName, friendName))
            {
                await WebHelper.SendOkAsync(context, "add");
                return;
            }
            await WebHelper.Send400Async(context, "cannot add");
        }

        [NeedAuth(Role.Reader)]
        public async Task GetOrdersAsync(HttpListenerContext context)
        {
            var userName = SessionHelper.GetUserName(context);
            if (string.IsNullOrEmpty(userName))
            {
                await WebHelper.Send400Async(context, "user not valid");
                return;
            }
            var orders = await DatabaseHelper.GetOrdersAsync(userName);
            await WebHelper.SendJsonObjectAsync(context, orders);
        }

        [NeedAuth(Role.Reader)]
        public async Task GetFriendsAsync(HttpListenerContext context)
        {
            var userName = SessionHelper.GetUserName(context);
            if (string.IsNullOrEmpty(userName))
            {
                await WebHelper.Send400Async(context, "user not valid");
                return;
            }
            var orders = await DatabaseHelper.GetFriendsAsync(userName);
            await WebHelper.SendJsonObjectAsync(context, orders);
        }

        [NeedAuth(Role.Owner)]
        public async Task PostAcceptOrderAsync(HttpListenerContext context)
        {
            using var stream = context.Request.InputStream;
            var order = await JsonSerializer.DeserializeAsync<string>(stream);

            var userName = SessionHelper.GetUserName(context);
            if (string.IsNullOrEmpty(userName))
            {
                await WebHelper.Send400Async(context, "user not valid");
                return;
            }

            if (await DatabaseHelper.AcceptOrderAsync(order, userName))
            {
                await WebHelper.SendOkAsync(context, "ok");
                return;
            }
            await WebHelper.Send400Async(context, "cannot accept");
        }
    }
}
