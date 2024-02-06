using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Model.Task
{
    public class TaskData
    {
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public string Priority { get; set; }
    }
}
