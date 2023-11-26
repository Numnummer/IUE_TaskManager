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
        {
            return await databaseContext.WriteLoginDataAsync(loginData);
        }

        public static async Task<bool> IsLoginDataExistAsync(LoginData? loginData)
        {
            return await databaseContext.IsLoginDataExistAsync(loginData);
        }

        public static async Task<LoginData> GetUserDataAsync(string name)
        {
            return await databaseContext.GetUserDataAsync(name);
        }

        public static async Task<bool> WriteTaskAsync(string username, TaskManagerModel.Task taskData)
        {
            return await databaseContext.WriteTaskAsync(userName, taskData);
        }
    }
}
