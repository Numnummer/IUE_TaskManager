using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerModel
{
    public class Task : Entity
    {
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public TaskStatus Status { get; set; }
        public uint Priority { get; set; }

        public Task(string? name, DateTimeOffset deadline, uint priority) : base(name)
        {
            Deadline = deadline;
            Priority = priority;
        }
    }
}
