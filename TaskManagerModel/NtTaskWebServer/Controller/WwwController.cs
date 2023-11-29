using NtTaskWebServer.Framework.Helpers;
using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Controller
{
    public static class WwwController
    {
        public static async Task GetWwwAsync(HttpListenerContext context, string fileName)
        {
            var fileExtention = fileName.Split('.')[^1];
            var mime = string.Empty;
            switch (fileExtention)
            {
                case ".css":
                    mime="text/css";
                    break;
                case ".js":
                    mime="application/javascript";
                    break;
                default:
                    break;
            }
            var view = new View(fileName, mime);
            await WebHelper.SendViewAsync(context, view);
        }
    }
}
