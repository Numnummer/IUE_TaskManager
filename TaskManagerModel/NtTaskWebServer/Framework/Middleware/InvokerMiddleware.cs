using NtTaskWebServer.Framework.Attributes;
using NtTaskWebServer.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Middleware
{
    public class InvokerMiddleware : Middleware
    {
        public InvokerMiddleware(Middleware successor) : base(successor) { }

        private bool CanCallController(MethodInfo method)
        {
            var attribute = (NeedAuthAttribute?)method.GetCustomAttribute(typeof(NeedAuthAttribute));
            if (attribute==null)
            {
                return true;
            }
            return attribute.Role<=WebSettings.Role && WebSettings.IsAuthentificated;
        }
        public override async Task Invoke(HttpListenerContext context)
        {
            await Task.Run(async () =>
            {
                if (WebSettings.ControllerType!=null)
                {
                    var method = WebSettings.ControllerType.GetMethod(WebSettings.ActionName);
                    if (CanCallController(method))
                    {
                        var controller = Activator.CreateInstance(WebSettings.ControllerType);
                        await CallControllerAsync(controller, method, context);
                    }
                    else
                    {
                        WebHelper.Send401(context);
                    }
                }
                else
                {
                    await WebHelper.Send400Async(context, "Не найден контроллер для запроса");
                }
            });
            if (_successor!=null)
            {
                await _successor.Invoke(context);
            }
            else
            {
                context.Response.Close();
            }
        }

        private async Task CallControllerAsync(object controller, MethodInfo method, HttpListenerContext context)
        {
            if (method != null)
            {
                await (Task)method.Invoke(controller, new object[] { context });
            }
            else
            {
                await WebHelper.Send400Async(context, "Не найден метод для обработки запроса");
            }
        }

    }
}
