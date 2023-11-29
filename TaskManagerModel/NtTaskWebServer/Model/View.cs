using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Model
{
    public class View
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public View(string fileName, string contentType)
        {
            FileName = fileName;
            ContentType = contentType;
        }
    }
}
