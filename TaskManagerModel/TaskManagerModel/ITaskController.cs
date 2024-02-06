using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerDomain
{
    public interface ITaskController
    {
        Models.Task CreateTask(string name, DateTime deadline, uint priority);
        bool RemoveTaskById(Guid taskId);
        Models.Task GetTaskById(Guid taskId);
        void UpdateAllTasks();
        event Action<Models.Task> TaskUpdated;
    }
}
