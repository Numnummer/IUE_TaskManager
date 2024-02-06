using Npgsql;
using NtTaskWebServer.Framework.Database;
using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Models;
using static Npgsql.PostgresTypes.PostgresCompositeType;
using Task = System.Threading.Tasks.Task;

namespace NtTaskWebServer.Framework.Helpers
{

    public static class DatabaseHelper
    {
        private static string? _connectionString;
        private static string? _hashSalt;
        public static void Init(string connectionString, string hashSalt)
        {
            _connectionString = connectionString;
            _hashSalt = hashSalt;
        }
        private static readonly Lazy<NttaskDatabaseContext> lazyDatabaseContext = new Lazy<NttaskDatabaseContext>(() => new NttaskDatabaseContext(_connectionString, _hashSalt));

        private static NttaskDatabaseContext databaseContext => lazyDatabaseContext.Value;

        public static async Task<bool> WriteLoginDataAsync(LoginData? loginData)
        {
            try
            {
                return await databaseContext.WriteLoginDataAsync(loginData);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
                return false;
            }
        }

        public static async Task<bool> IsLoginDataExistAsync(LoginData? loginData)
        {
            try
            {
                return await databaseContext.IsLoginDataExistAsync(loginData);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
                return false;
            }
        }

        public static async Task<LoginData?> GetUserDataAsync(string name)
        {
            try
            {
                return await databaseContext.GetUserDataAsync(name);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
                return null;
            }
        }

        public static async Task<bool> WriteTaskAsync(string userName, Models.Task taskData)
        {
            try
            {
                return await databaseContext.WriteTaskAsync(userName, taskData);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
                return false;
            }
        }
        public static async Task<Models.Task[]?> GetTaskDataAsync(string userName)
        {
            try
            {
                return await databaseContext.GetTaskDataAsync(userName);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
                return null;
            }
        }
        public static async Task UpdateTask(Models.Task task)
        {
            try
            {
                await databaseContext.UpdateTask(task);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
            }
        }

        public static async Task<bool> RemoveTaskAsync(string userName, Guid id)
        {
            try
            {
                return await databaseContext.RemoveTaskAsync(userName, id);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
                return false;
            }
        }
        public static async Task<bool> SetTaskStatusAsync(string userName, Guid id, Models.TaskStatus status)
        {
            try
            {
                return await databaseContext.SetTaskStatusAsync(userName, id, status);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
                return false;
            }
        }
        public static async Task<string[]?> GetUsersAsync(string prompt)
        {
            try
            {
                return await databaseContext.GetUsersAsync(prompt);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
                return null;
            }
        }
        public static async Task<bool> AddOrderAsync(string userName, string friendName)
        {
            try
            {
                return await databaseContext.AddOrderAsync(userName, friendName);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
                await Console.Out.WriteLineAsync(exception.Source);
                await Console.Out.WriteLineAsync(exception.TargetSite?.Name);
                await Console.Out.WriteLineAsync(exception.StackTrace);
                return false;
            }
        }

        public static async Task<string[]?> GetOrdersAsync(string userName)
        {
            try
            {
                return await databaseContext.GetOrdersAsync(userName);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
                return null;
            }
        }
        public static async Task<string[]?> GetFriendsAsync(string userName)
        {
            try
            {
                return await databaseContext.GetFriendsAsync(userName);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
                return null;
            }
        }
        public static async Task<bool> HasFriendAsync(string userName, string friend)
        {
            try
            {
                return await databaseContext.HasFriendAsync(userName, friend);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
                return false;
            }
        }

        public static async Task<bool> AcceptOrderAsync(string? order, string userName)
        {
            try
            {
                return await databaseContext.AcceptOrderAsync(order, userName);
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync(exception.Message);
                return false;
            }
        }
    }
}
