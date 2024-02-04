using NtTaskWebServer.Framework.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework
{
    public class WebServer
    {
        private readonly HttpListener _listener = new();
        public bool IsServerWork => _listener.IsListening;

        public async Task ListenAsync(string prefix, CancellationToken cancellationToken)
        {
            _listener.Prefixes.Add(prefix);

            _listener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                var context = await _listener.GetContextAsync();
                await MiddlewareRunner.RunMiddleware(context);
            }
            _listener.Close();
        }
    }
}
