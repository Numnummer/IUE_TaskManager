
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

        public static async Task<bool> IsSessionExistAsync(string session, CookieCollection cookies)
        {
            if (string.IsNullOrWhiteSpace(session)) return false;
            var splitSession = session.Split(' ');
            var sessionId = Guid.Parse(splitSession[0]);
            if (await _database.KeyExistsAsync(sessionId.ToString()))
            {
                var cookieValue = await _database.StringGetAsync(sessionId.ToString());
                if (sessionId == Guid.Parse(cookieValue.ToString().Split(' ')[0]))
                {
                    return true;
                }
            }
            return false;
        }

        public static async Task<Cookie> MakeSessionCookieAsync(string userName, Model.Role role)
        {
            var sessionId = Guid.NewGuid();
            var value = new UserData(userName, sessionId, role);
            var cookie = new Cookie()
            {
                Name = "session",
                Value = value.ToString(),
                Expires = DateTime.Now.AddMinutes(CookieLifetimeMinutes)
            };
            await _database.StringSetAsync(sessionId.ToString(), cookie.Value, expiry: TimeSpan.FromMinutes(CookieLifetimeMinutes));
            return cookie;
        }

        public static async Task<bool> RemoveCookieAsync(Cookie cookie)
        {
            var userId = Guid.Parse(cookie.Value.Split(' ')[0]);
            return await _database.KeyDeleteAsync(userId.ToString());
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

        public static async Task<Cookie?> GetCookieAsync(HttpListenerContext context)
        {
            var cookie = context.Request.Cookies["session"];
            var userId = Guid.Parse(cookie.Value.Split(' ')[0]);
            var cookieValue = await _database.StringGetAsync(userId.ToString());
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

        public static async Task<bool> UpdateSessionAsync(Cookie cookie)
        {
            var userId = Guid.Parse(cookie.Value.Split(' ')[0]);
            var newExpiry = TimeSpan.FromMinutes(CookieLifetimeMinutes);
            return await _database.KeyExpireAsync(userId.ToString(), newExpiry);
        }
    }
}
