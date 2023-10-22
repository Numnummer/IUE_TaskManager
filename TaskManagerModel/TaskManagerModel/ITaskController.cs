using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerModel
{
    public interface ITaskController
    {
        bool CreateTask(string name);
        bool RemoveTaskById(Guid taskId);
        TaskManagerModel.Task GetTaskById(Guid taskId);
        void UpdateAllTasks();
    }
}
