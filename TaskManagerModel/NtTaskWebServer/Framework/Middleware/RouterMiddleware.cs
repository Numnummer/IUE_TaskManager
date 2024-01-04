using NtTaskWebServer.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Middleware
{
    public class RouterMiddleware : Middleware
    {
        public RouterMiddleware(Middleware successor) : base(successor) { }

        public override async Task Invoke(HttpListenerContext context)
        {
            await Task.Run(async () =>
            {
                var rawUrlName = context.Request.RawUrl.Trim('/');
                try
                {
                    if (rawUrlName[..3]=="www")
                    {
                        await RouteWwwAsync(context, rawUrlName);
                    }
                }
                catch (Exception exception)
                {
                    await Console.Out.WriteLineAsync(exception.Message);
                }

                MakeActionAndControllerUrls(rawUrlName, out var controllerUrl, out var actionUrl);
                RouteRequests(context, controllerUrl, actionUrl);
            });
            if (_successor!=null)
            {
                await _successor.Invoke(context);
            }
        }
        private void MakeActionAndControllerUrls(string rawUrlName, out string controllerUrl, out string actionUrl)
        {
            var urlNames = rawUrlName.Split('/');
            if (urlNames.Length==1)
            {
                actionUrl=controllerUrl=rawUrlName;
            }
            else
            {
                controllerUrl=urlNames[0];
                actionUrl=urlNames[1];
            }
        }
        private async Task RouteWwwAsync(HttpListenerContext context, string rawUrlName)
        {
            await WwwController.GetWwwAsync(context, rawUrlName);
        }
        private void RouteRequests(HttpListenerContext context, string controllerUrl, string actionUrl)
        {
            var methodName = "Get" + actionUrl + "Async";
            if (context.Request.HttpMethod=="POST")
            {
                methodName = "Post" + actionUrl + "Async";
            }
            var controllerName = "NtTaskWebServer.Controller." + controllerUrl + "Controller";
            var controllerType = Type.GetType(controllerName);

            WebSettings.ControllerType=controllerType;
            WebSettings.ActionName=methodName;
        }
    }
}
