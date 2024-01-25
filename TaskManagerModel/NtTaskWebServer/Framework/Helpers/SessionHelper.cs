using Microsoft.Extensions.Caching.Memory;
using NtTaskWebServer.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Helpers
{
    public static class SessionHelper
    {
        private static readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("localhost");
        private static readonly IDatabase _database = _redis.GetDatabase();

        public const byte CookieLifetimeMinutes = 30;

        public static bool IsSessionExist(string session, CookieCollection cookies)
        {
            if (string.IsNullOrWhiteSpace(session)) return false;
            var splitSession = session.Split(' ');
            var sessionId = Guid.Parse(splitSession[0]);
            if (_database.KeyExists(sessionId.ToString()))
            {
                var cookieValue = _database.StringGet(sessionId.ToString());
                if (sessionId == Guid.Parse(cookieValue.ToString().Split(' ')[0]))
                {
                    return true;
                }
            }
            return false;
        }

        public static Cookie MakeSessionCookie(string userName, Model.Role role)
        {
            var sessionId = Guid.NewGuid();
            var value = new UserData(userName, sessionId, role);
            var cookie = new Cookie()
            {
                Name = "session",
                Value = value.ToString(),
                Expires = DateTime.Now.AddMinutes(CookieLifetimeMinutes)
            };
            _database.StringSet(sessionId.ToString(), cookie.Value, expiry: TimeSpan.FromMinutes(CookieLifetimeMinutes));
            return cookie;
        }

        public static bool RemoveCookie(Cookie cookie)
        {
            var userId = Guid.Parse(cookie.Value.Split(' ')[0]);
            return _database.KeyDelete(userId.ToString());
        }

        public static string? GetUserName(HttpListenerContext context)
        {
            try
            {
                var cookie = context.Request.Cookies["session"];
                return cookie.Value.Split(' ')[1];
            }
            catch
            {
                return null;
            }
        }

        public static Cookie? GetCookie(HttpListenerContext context)
        {
            var cookie = context.Request.Cookies["session"];
            var userId = Guid.Parse(cookie.Value.Split(' ')[0]);
            var cookieValue = _database.StringGet(userId.ToString());
            if (!cookieValue.IsNull)
            {
                return new Cookie()
                {
                    Name = "session",
                    Value = cookieValue.ToString(),
                    Expires = DateTime.Now.AddMinutes(CookieLifetimeMinutes)
                };
            }
            return null;
        }
    }
}
