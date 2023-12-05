using Npgsql;
using NtTaskWebServer.Framework.Database;
using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Helpers
{
    //TODO: обернуть методы в try catch
    public static class DatabaseHelper
    {
        private static readonly NttaskDatabaseContext databaseContext = new();
        public static async Task<bool> WriteLoginDataAsync(LoginData? loginData)
            => await databaseContext.WriteLoginDataAsync(loginData);

        public static async Task<bool> IsLoginDataExistAsync(LoginData? loginData)
            => await databaseContext.IsLoginDataExistAsync(loginData);

        public static async Task<LoginData> GetUserDataAsync(string name)
            => await databaseContext.GetUserDataAsync(name);

        public static async Task<bool> WriteTaskAsync(string userName, TaskManagerModel.Task taskData)
            => await databaseContext.WriteTaskAsync(userName, taskData);
        public static async Task<TaskManagerModel.Task[]> GetTaskDataAsync(string userName)
            => await databaseContext.GetTaskDataAsync(userName);
        public static async Task UpdateTask(TaskManagerModel.Task task)
            => await databaseContext.UpdateTask(task);

        public static async Task<bool> RemoveTaskAsync(string userName, Guid id)
            => await databaseContext.RemoveTaskAsync(userName, id);
        public static async Task<bool> SetTaskStatusAsync(string userName, Guid id, TaskManagerModel.TaskStatus status)
            => await databaseContext.SetTaskStatusAsync(userName, id, status);
        public static async Task<string[]> GetUsersAsync(string prompt)
            => await databaseContext.GetUsersAsync(prompt);
        public static async Task<bool> AddOrderAsync(string userName, string friendName)
            => await databaseContext.AddOrderAsync(userName, friendName);

        public static async Task<string[]> GetOrdersAsync(string userName)
            => await databaseContext.GetOrdersAsync(userName);
        public static async Task<string[]> GetFriendsAsync(string userName)
            => await databaseContext.GetFriendsAsync(userName);
        public static async Task<bool> HasFriendAsync(string userName, string friend)
            => await databaseContext.HasFriendAsync(userName, friend);

        public static async Task<bool> AcceptOrderAsync(string? order, string userName)
            => await databaseContext.AcceptOrderAsync(order, userName);
    }
}
