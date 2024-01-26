using NtTaskWebServer.Model.Task;
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

        public static async Task<TaskManagerModel.Task> CreateTaskAsync(HttpListenerContext context, string name, DateTime deadline, uint priorityNumber)
        {
            var taskObject = _taskController.CreateTask(name, deadline, priorityNumber);
            var userName = SessionHelper.GetUserName(context);
            if (userName!=null && !await DatabaseHelper.WriteTaskAsync(userName, taskObject))
            {
                throw new Exception();
            }
            return taskObject;
        }

        public static async Task<bool> RemoveTaskAsync(HttpListenerContext context, Guid id)
        {
            var isRemoved = _taskController.RemoveTaskById(id);
            if (!isRemoved)
            {
                return false;
            }
            var userName = SessionHelper.GetUserName(context);
            if (userName!=null && !await DatabaseHelper.RemoveTaskAsync(userName, id))
            {
                throw new Exception();
            }
            return true;
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

        public static async Task<bool> DecreaseTaskStatusAsync(HttpListenerContext context, Guid id)
        {
            var task = _taskController.GetTaskById(id);
            var userName = SessionHelper.GetUserName(context);
            switch (task.Status)
            {
                case TaskManagerModel.TaskStatus.NotStarted:
                    return false;
                case TaskManagerModel.TaskStatus.InProcess:
                    _taskController.SetTaskStatusById(id, TaskManagerModel.TaskStatus.NotStarted);
                    await DatabaseHelper.SetTaskStatusAsync(userName, id, TaskManagerModel.TaskStatus.NotStarted);
                    break;
                case TaskManagerModel.TaskStatus.Done:
                    _taskController.SetTaskStatusById(id, TaskManagerModel.TaskStatus.InProcess);
                    await DatabaseHelper.SetTaskStatusAsync(userName, id, TaskManagerModel.TaskStatus.InProcess);
                    break;
                case TaskManagerModel.TaskStatus.Expired:
                    return false;
            }
            return true;
        }

        public static async Task<bool> IncreaseTaskStatusAsync(HttpListenerContext context, Guid id)
        {
            var task = _taskController.GetTaskById(id);
            var userName = SessionHelper.GetUserName(context);
            switch (task.Status)
            {
                case TaskManagerModel.TaskStatus.NotStarted:
                    _taskController.StartTaskById(id);
                    await DatabaseHelper.SetTaskStatusAsync(userName, id, TaskManagerModel.TaskStatus.InProcess);
                    break;
                case TaskManagerModel.TaskStatus.InProcess:
                    _taskController.CompleteTaskById(id);
                    await DatabaseHelper.SetTaskStatusAsync(userName, id, TaskManagerModel.TaskStatus.Done);
                    break;
                case TaskManagerModel.TaskStatus.Done:
                    return false;
                case TaskManagerModel.TaskStatus.Expired:
                    return false;
            }
            return true;
        }
    }
}
