using MyWebFramework;
using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Helpers
{
    public static class WebHelper
    {
        public static async Task Send400Async(HttpListenerContext context, string message)
        {
            context.Response.StatusCode = 400;
            using var stream = context.Response.OutputStream;
            var bytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(bytes, 0, bytes.Length);
        }

        public static async Task SendViewAsync(HttpListenerContext context, View view)
        {
            using var stream = context.Response.OutputStream;
            var buffer = await File.ReadAllBytesAsync(view.FileName);
            context.Response.ContentType = view.ContentType;
            await stream.WriteAsync(buffer);
        }

        public static async Task SendOkAsync(HttpListenerContext context, string message)
        {
            context.Response.StatusCode = 200;
            using var stream = context.Response.OutputStream;
            var bytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(bytes, 0, bytes.Length);
        }

        public static async Task SendSessionAsync(HttpListenerContext context, string userName)
        {
            var role = Role.Owner;
            if (!await DatabaseHelper.WriteRoleAsync(context.Request.RawUrl, userName, role))
            {
                Send500(context);
                return;
            }
            var cookie = SessionHelper.MakeSessionCookie(userName, role);
            context.Response.Cookies.Add(cookie);
        }

        public static void Send401(HttpListenerContext context)
        {
            context.Response.StatusCode = 401;
        }

        public static void Send500(HttpListenerContext context)
        {
            context.Response.StatusCode = 500;
        }

        public static void DeleteSession(HttpListenerContext context)
        {
            var sessionCookie = context.Request.Cookies["session"];
            if (sessionCookie == null)
            {
                return;
            }
            if (SessionHelper.RemoveCookie(sessionCookie))
            {
                sessionCookie.Expires = DateTime.Now.AddDays(-1);
            }
        }
    }
}
