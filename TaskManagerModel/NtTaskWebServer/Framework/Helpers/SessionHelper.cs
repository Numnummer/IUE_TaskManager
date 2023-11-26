using Microsoft.Extensions.Caching.Memory;
using NtTaskWebServer.Model;
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
        private static readonly MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

        public const byte CookieLifetimeMinutes = 30;
        public static bool IsSessionExist(string session, CookieCollection cookies)
        {
            var splitSession = session.Split(' ');
            var sessionId = Guid.Parse(splitSession[0]);
            var userName = splitSession[1];
            if (memoryCache.TryGetValue(userName, out var actualSessionId))
            {
                if (sessionId == (actualSessionId as Guid?))
                {
                    return true;
                }
            }
            return false;
        }
        public static Cookie MakeSessionCookie(string userName, Role role)
        {
            var sessionId = Guid.NewGuid();
            memoryCache.Set(userName, sessionId);
            var value = new UserData(userName, sessionId, role);
            return new Cookie()
            {
                Name = "session",
                Value = value.ToString(),
                Expires = DateTime.UtcNow.AddMinutes(CookieLifetimeMinutes)
            };
        }

        public static bool RemoveCookie(Cookie cookie)
        {
            var userName = cookie.Value.Split(' ')[1];
            memoryCache.Remove(userName);
            return memoryCache.TryGetValue(userName, out _);
        }
    }
}
