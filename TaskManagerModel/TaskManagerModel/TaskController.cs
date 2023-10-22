using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerModel
{
    public class TaskController : ITaskController
    {
        public bool CreateTask(string name)
        {
            throw new NotImplementedException();
        }

        public bool CreateTask(string name, DateTimeOffset deadline)
        {
            throw new NotImplementedException();
        }

        public Task GetTaskById(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveTaskById(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public bool SetTaskStatusById(Guid taskId, TaskStatus status)
        {
            throw new NotImplementedException();
        }

        public bool SetTaskDeadlineById(Guid taskId, DateTimeOffset deadline)
        {
            throw new NotImplementedException();
        }

        public bool StartTaskById(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public void UpdateTaskById(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public void UpdateAllTasks()
        {
            throw new NotImplementedException();
        }
    }
}
