using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Middleware
{
    public static class MiddlewareRunner
    {
        public static async Task RunMiddleware(HttpListenerContext context)
        {
            var invoker = new InvokerMiddleware(null);
            var authorization = new AuthorizationMiddleware(invoker);
            var authentification = new AuthentificationMiddleware(authorization);
            var router = new RouterMiddleware(authentification);

            await router.Invoke(context);
        }
    }
}
