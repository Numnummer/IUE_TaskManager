using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Middleware
{
    public abstract class Middleware
    {
        protected Middleware? _successor;
        public Middleware(Middleware? successor)
        {
            _successor=successor;
        }
        public abstract Task Invoke(HttpListenerContext context);
    }
}
