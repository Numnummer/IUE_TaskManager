using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerModel
{
    public interface ITaskController
    {
        Task CreateTask(string name, DateTimeOffset deadline, uint priority);
        bool RemoveTaskById(Guid taskId);
        TaskManagerModel.Task GetTaskById(Guid taskId);
        void UpdateAllTasks();
    }
}
