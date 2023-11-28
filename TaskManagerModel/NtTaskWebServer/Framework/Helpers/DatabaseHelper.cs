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
    }
}
