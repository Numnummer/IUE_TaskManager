using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework
{
    public static class SessionHelper
    {
        private static readonly Dictionary<string, Guid> _sessions = new();
        internal static Dictionary<string, Guid> Sessions => _sessions;
        public const byte CookieLifetimeMinutes = 2;
        public static bool IsSessionExist(string session, CookieCollection cookies)
        {
            var splitSession = session.Split(' ');
            var sessionId = Guid.Parse(splitSession[0]);
            var userName = splitSession[1];
            if (_sessions.TryGetValue(userName, out var actualSessionId))
            {
                if (sessionId==actualSessionId)
                {
                    return true;
                }
            }
            return false;
        }
        public static Cookie GetSessionCookie(string userName)
        {
            var sessionId = Guid.NewGuid();
            _sessions.Add(userName, sessionId);
            return new Cookie()
            {
                Name="session",
                Value=sessionId.ToString()+' '+userName,
                Expires=DateTime.UtcNow.AddMinutes(CookieLifetimeMinutes)
            };
        }
    }
}
