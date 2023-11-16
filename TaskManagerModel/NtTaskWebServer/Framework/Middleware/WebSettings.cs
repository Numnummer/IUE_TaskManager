using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Middleware
{
    public static class WebSettings
    {
        public static Type ControllerType { get; set; }
        public static string ActionName { get; set; }
        public static bool IsAuthentificated { get; set; }
        public static Role? Role { get; set; }
    }
}
