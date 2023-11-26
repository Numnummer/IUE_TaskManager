using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Helpers
{
    public static class TaskHelper
    {
        private static readonly TaskManagerModel.TaskController _taskController = new();

        public static void UpdateAllTasks()
        {
            _taskController.UpdateAllTasks();
        }

        public static async Task CreateTaskAsync(HttpListenerContext context, string name, DateTimeOffset deadline, uint priorityNumber)
        {
            var taskObject = _taskController.CreateTask(name, deadline, priorityNumber);
            var userName = SessionHelper.GetUserName(context);
            if (userName!=null && !await DatabaseHelper.WriteTaskAsync(userName, taskObject))
            {
                throw new Exception();
            }
        }
    }
}
