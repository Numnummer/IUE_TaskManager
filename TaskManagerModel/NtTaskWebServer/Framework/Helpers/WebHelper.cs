using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Helpers
{
    public static class WebHelper
    {
        public static readonly Dictionary<string, int> UserCode = new();
        public static string? GetCode(string userName)
        {
            Random random = new Random();
            int code = random.Next(100000, 999999);
            if (UserCode.TryAdd(userName, code))
            {
                return code.ToString();
            }
            return null;
        }
        public static async Task Send400Async(HttpListenerContext context, string message)
        {
            context.Response.StatusCode = 400;
            using var stream = context.Response.OutputStream;
            var bytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(bytes, 0, bytes.Length);
            context.Response.Close();
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

        public static void SendSession(HttpListenerContext context, string userName, Role role)
        {
            var cookie = SessionHelper.MakeSessionCookie(userName, role);
            context.Response.Cookies.Add(cookie);
        }

        public static void UpdateSession(HttpListenerContext context)
        {
            var cookie = SessionHelper.GetCookie(context);
            if (cookie==null)
            {
                return;
            }
            cookie.Expires = DateTime.UtcNow.AddMinutes(SessionHelper.CookieLifetimeMinutes);
            context.Response.SetCookie(cookie);
        }

        public static void Send401(HttpListenerContext context)
        {
            context.Response.StatusCode = 401;
            context.Response.Close();
        }

        public static void Send500(HttpListenerContext context)
        {
            context.Response.StatusCode = 500;
            context.Response.Close();
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
                var cookie = SessionHelper.GetCookie(context);
                cookie.Expires = DateTime.Now.AddDays(-1);
                context.Response.SetCookie(cookie);
            }
        }

        public static async Task SendTasksAsync(HttpListenerContext context, TaskManagerModel.Task[] tasks)
        {
            var response = await Task.Run(() => JsonSerializer.Serialize(tasks));
            context.Response.ContentType="application/json";
            await SendOkAsync(context, response);
        }
        public static async Task SendJsonObjectAsync(HttpListenerContext context, object obj)
        {
            var response = await Task.Run(() => JsonSerializer.Serialize(obj));
            context.Response.ContentType="application/json";
            await SendOkAsync(context, response);
        }
    }
}
