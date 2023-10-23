using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerModel
{
    public class TaskController : ITaskController
    {
        private readonly Dictionary<Guid, Task> _tasks = new Dictionary<Guid, Task>();

        public bool CreateTask(string name, DateTimeOffset deadline, uint priority)
        {
            bool isArgsInvalid = string.IsNullOrWhiteSpace(name) ||
                deadline <= DateTimeOffset.UtcNow;
            if (isArgsInvalid)
            {
                return false;
            }
            var task = new Task(name, deadline, priority);
            _tasks.Add(task.Id, task);
            return true;
        }

        public Task GetTaskById(Guid taskId)
        {
            return _tasks[taskId];
        }

        public bool TryGetTaskById(Guid taskId, out Task? task)
        {
            return _tasks.TryGetValue(taskId, out task);
        }

        public bool RemoveTaskById(Guid taskId)
        {
            return _tasks.Remove(taskId);
        }

        private bool SetTaskStatusById(Guid taskId, TaskStatus status)
        {
            if (_tasks.TryGetValue(taskId, out var task))
            {
                task.Status=status;
                return true;
            }
            return false;
        }

        public bool SetTaskDeadlineById(Guid taskId, DateTimeOffset deadline)
        {
            if (_tasks.TryGetValue(taskId, out var task) &&
                deadline > DateTimeOffset.UtcNow)
            {
                task.Deadline=deadline;
                return true;
            }
            return false;
        }

        public bool SetTaskPriorityById(Guid taskId, uint priority)
        {
            if (_tasks.TryGetValue(taskId, out var task))
            {
                task.Priority=priority;
                return true;
            }
            return false;
        }

        public bool StartTaskById(Guid taskId)
        {
            if (SetTaskStatusById(taskId, TaskStatus.InProcess))
            {
                var task = _tasks[taskId];
                task.StartTime=DateTimeOffset.UtcNow;
                return true;
            }
            return false;
        }

        public bool CompleteTaskById(Guid taskId)
        {
            return SetTaskStatusById(taskId, TaskStatus.Done);
        }

        public bool UpdateTaskById(Guid taskId)
        {
            if (_tasks.TryGetValue(taskId, out var task))
            {
                if (task.Deadline<=DateTimeOffset.UtcNow)
                {
                    task.Status=TaskStatus.Expired;
                }
                return true;
            }
            return false;
        }

        public void UpdateAllTasks()
        {
            foreach (var pair in _tasks)
            {
                if (pair.Value.Deadline<=DateTimeOffset.UtcNow)
                {
                    pair.Value.Status=TaskStatus.Expired;
                }
            }
        }
    }
}
