﻿using Microsoft.Extensions.Caching.Memory;
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
            if (string.IsNullOrWhiteSpace(session)) return false;
            var splitSession = session.Split(' ');
            var sessionId = Guid.Parse(splitSession[0]);
            if (memoryCache.TryGetValue(sessionId, out var cookie))
            {
                if (sessionId == Guid.Parse((cookie as Cookie).Value.Split(' ')[0]))
                {
                    return true;
                }
            }
            return false;
        }
        public static Cookie MakeSessionCookie(string userName, Role role)
        {
            var sessionId = Guid.NewGuid();
            var value = new UserData(userName, sessionId, role);
            var cookie = new Cookie()
            {
                Name = "session",
                Value = value.ToString(),
                Expires = DateTime.UtcNow.AddMinutes(CookieLifetimeMinutes)
            };
            memoryCache.Set(sessionId, cookie);
            return cookie;
        }

        public static bool RemoveCookie(Cookie cookie)
        {
            var userId = Guid.Parse(cookie.Value.Split(' ')[0]);
            memoryCache.Remove(userId);
            return memoryCache.TryGetValue(userId, out _);
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
            return memoryCache.Get(userId) as Cookie;
        }
    }
}
