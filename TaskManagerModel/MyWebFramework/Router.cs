using MyWebFramework.Attributes;
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
            var methodName = "Get" + rawUrlName;
            if (context.Request.HttpMethod=="POST")
            {
                methodName = "Post" + rawUrlName;
            }
            var controllerName = rawUrlName + "Controller";
            var controllerType = Type.GetType(controllerName);

            if (controllerType != null)
            {
                var controller = Activator.CreateInstance(controllerType);
                var method = controllerType.GetMethod(methodName);
                if (method != null)
                {
                    await (Task)method.Invoke(controller, null);
                }
                else
                {
                    throw new ArgumentException("Не найден метод для обработки запроса");
                }
            }
            else
            {
                throw new ArgumentException("Не найден контроллер для класса");
            }
        }
    }
}
