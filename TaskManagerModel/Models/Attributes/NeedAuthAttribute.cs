using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Attributes
{
    public class NeedAuthAttribute : Attribute
    {
        public Role Role { get; set; }
        public NeedAuthAttribute(Role role)
        {
            Role=role;
        }
    }
}
