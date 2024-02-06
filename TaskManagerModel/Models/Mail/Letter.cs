using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Mail
{
    public class Letter
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Theme { get; set; }
        public string Body { get; set; }
    }
}
