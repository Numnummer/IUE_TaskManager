using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Mail
{
    public class MailConfig
    {
        public string MailServerUrl { get; set; }
        public string MailPort { get; set; }
        public string Password { get; set; }
    }
}
