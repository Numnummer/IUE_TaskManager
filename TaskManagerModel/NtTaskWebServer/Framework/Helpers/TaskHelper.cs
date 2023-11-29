using NtTaskWebServer.Model;
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
        static TaskHelper()
        {
            _taskController.TaskUpdated+=OnTaskUpdate;
        }
        public static void OnTaskUpdate(TaskManagerModel.Task updatedTask)
        {
            Task.Run(async () =>
            {
                await DatabaseHelper.UpdateTask(updatedTask);
            });
        }
        public static async Task<TaskManagerModel.Task[]> UpdateAllTasksAsync(HttpListenerContext context)
        {
            var tasks = await DatabaseHelper.GetTaskDataAsync(SessionHelper.GetUserName(context));
            _taskController.AddIfNotExist(tasks);
            _taskController.UpdateAllTasks();
            return tasks;
        }

        public static bool StartTaskById(Guid id)
        {
            return _taskController.StartTaskById(id);
        }

        public static async Task CreateTaskAsync(HttpListenerContext context, string name, DateTime deadline, uint priorityNumber)
        {
            var taskObject = _taskController.CreateTask(name, deadline, priorityNumber);
            var userName = SessionHelper.GetUserName(context);
            if (userName!=null && !await DatabaseHelper.WriteTaskAsync(userName, taskObject))
            {
                throw new Exception();
            }
        }

        public static TaskView[] MakeTaskViews(TaskManagerModel.Task[] tasks)
        {
            var views = new List<TaskView>();
            foreach (var task in tasks)
            {
                views.Add(new TaskView("TaskCard.htm", "text/html", task));
            }
            return views.ToArray();
        }
    }
}
