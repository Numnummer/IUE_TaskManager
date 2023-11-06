using NtTaskWebServer.Controller;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework
{
    public static class Router
    {
        public static async Task RouteAsync(HttpListenerContext context)
        {
            var rawUrlName = context.Request.RawUrl.Trim('/');
            if (rawUrlName==string.Empty)
            {
                rawUrlName=GetRawUrl(context);
            }
            if (rawUrlName[..3]=="www")
            {
                await RouteWwwAsync(context, rawUrlName);
            }

            var methodName = "Get" + rawUrlName + "Async";
            if (context.Request.HttpMethod=="POST")
            {
                methodName = "Post" + rawUrlName + "Async";
            }
            var controllerName = "NtTaskWebServer.Controller." + rawUrlName + "Controller";
            var controllerType = Type.GetType(controllerName);

            await CallController(controllerType, methodName, context);
        }

        private static string GetRawUrl(HttpListenerContext context)
        {
            var cookies = context.Request.Cookies;
            var session = cookies["session"]?.Value;
            if (session==null || !SessionHelper.IsSessionExist(session, cookies))
            {
                return "StartPage";
            }
            return "Dashboard";
        }

        private static async Task CallController(Type controllerType, string methodName, HttpListenerContext context)
        {
            if (controllerType != null)
            {
                var controller = Activator.CreateInstance(controllerType);
                var method = controllerType.GetMethod(methodName);
                if (method != null)
                {
                    await (Task)method.Invoke(controller, new object[] { context });
                }
                else
                {
                    await WebHelper.Send400Async(context, "Не найден метод для обработки запроса");
                }
            }
            else
            {
                await WebHelper.Send400Async(context, "Не найден контроллер для запроса");
            }
        }

        private static async Task RouteWwwAsync(HttpListenerContext context, string rawUrlName)
        {
            await WwwController.GetWwwAsync(context, rawUrlName);
        }
    }
}
