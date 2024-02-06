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
        private static readonly TaskManagerDomain.TaskController _taskController = new();
        static TaskHelper()
        {
            _taskController.TaskUpdated+=OnTaskUpdate;
        }
        public static void OnTaskUpdate(Models.Task updatedTask)
        {
            Task.Run(async () =>
            {
                await DatabaseHelper.UpdateTask(updatedTask);
            });
        }
        public static async Task<Models.Task[]> UpdateAllTasksAsync(HttpListenerContext context)
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

        public static async Task<Models.Task> CreateTaskAsync(HttpListenerContext context, string name, DateTime deadline, uint priorityNumber)
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

        public static async Task<bool> SetTaskStatusAsync(HttpListenerContext context, TaskStatusDto dto)
        {
            var id = Guid.Parse(dto.Id);
            var userName = SessionHelper.GetUserName(context);
            if (Enum.TryParse(typeof(Models.TaskStatus), dto.Status, out var status)
                && status is Models.TaskStatus
                && userName!=null)
            {
                _taskController.SetTaskStatusById(id, (Models.TaskStatus)status);
                return await DatabaseHelper.SetTaskStatusAsync(userName, id, (Models.TaskStatus)status);
            }
            else
            {
                return false;
            }
        }

        public static async Task<bool> DecreaseTaskStatusAsync(HttpListenerContext context, Guid id)
        {
            var task = _taskController.GetTaskById(id);
            var userName = SessionHelper.GetUserName(context);
            switch (task.Status)
            {
                case Models.TaskStatus.NotStarted:
                    return false;
                case Models.TaskStatus.InProcess:
                    _taskController.SetTaskStatusById(id, Models.TaskStatus.NotStarted);
                    await DatabaseHelper.SetTaskStatusAsync(userName, id, Models.TaskStatus.NotStarted);
                    break;
                case Models.TaskStatus.Done:
                    _taskController.SetTaskStatusById(id, Models.TaskStatus.InProcess);
                    await DatabaseHelper.SetTaskStatusAsync(userName, id, Models.TaskStatus.InProcess);
                    break;
                case Models.TaskStatus.Expired:
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
                case Models.TaskStatus.NotStarted:
                    _taskController.StartTaskById(id);
                    await DatabaseHelper.SetTaskStatusAsync(userName, id, Models.TaskStatus.InProcess);
                    break;
                case Models.TaskStatus.InProcess:
                    _taskController.CompleteTaskById(id);
                    await DatabaseHelper.SetTaskStatusAsync(userName, id, Models.TaskStatus.Done);
                    break;
                case Models.TaskStatus.Done:
                    return false;
                case Models.TaskStatus.Expired:
                    return false;
            }
            return true;
        }

        public static Models.Task? GetTaskById(string id)
        {
            if (Guid.TryParse(id, out var taskId))
            {
                return _taskController.GetTaskById(taskId);
            }
            else
            {
                return null;
            }
        }
    }
}
