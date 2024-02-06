using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Task : Entity
    {
        public DateTime StartTime { get; set; }
        public DateTime Deadline { get; set; }
        public TaskStatus Status { get; set; }
        public uint Priority { get; set; }

        public Task(string? name, DateTime deadline, uint priority) : base(name)
        {
            Deadline = deadline;
            Priority = priority;
        }
        public Task() { }

    }
}
