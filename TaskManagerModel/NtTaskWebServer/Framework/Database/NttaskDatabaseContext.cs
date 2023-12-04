using Npgsql;
using NtTaskWebServer.Framework.Helpers;
using NtTaskWebServer.Model;
using NtTaskWebServer.Model.Builder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Database
{
    public class NttaskDatabaseContext : DatabaseContext
    {
        public NttaskDatabaseContext() : base(ConfigurationManager.ConnectionStrings["nttask"].ConnectionString) { }

        public async Task<bool> WriteLoginDataAsync(LoginData loginData)
        {
            if (loginData == null || !ValidationHelper.IsValidLoginData(loginData)
                || await IsExistUserNameAsync(loginData.UserName))
            {
                return false;
            }
            var hashedPassword = CryptoHelper.HashString(loginData.Password);
            var commandText = "insert into userdata(name,email,login,password) " +
                $"values('{loginData.UserName}','{loginData.Email}','{loginData.Login}','{hashedPassword}')";
            var updated = await ExecuteNonQueryAsync(commandText);
            return updated > 0;
        }

        public async Task<bool> IsExistUserNameAsync(string userName)
        {
            var commandText = $"select * from userdata where name='{userName}'";
            var updated = await ExecuteScalarAsync(commandText);
            return updated != null;
        }

        public async Task<bool> IsLoginDataExistAsync(LoginData? loginData)
        {
            if (loginData == null || !ValidationHelper.IsValidLoginDataForEnter(loginData)
                || !await IsExistUserNameAsync(loginData.UserName))
            {
                return false;
            }
            var hashedPassword = CryptoHelper.HashString(loginData.Password);
            var commandText = $"select * from userdata where " +
                $"name='{loginData.UserName}' and password='{hashedPassword}'";
            var updated = await ExecuteScalarAsync(commandText);
            return updated != null;
        }

        public async Task<LoginData> GetUserDataAsync(string name)
        {
            var commandText = "select * from userdata " +
                $"where name='{name}'";
            using var connection = new NpgsqlConnection(_connectionString);

            var reader = await ExecuteReaderAsync(connection, commandText);

            return await LoginDataBuilder.BuildLoginDataByDataReaderAsync(reader);
        }
        public async Task<TaskManagerModel.Task[]> GetTaskDataAsync(string userName)
        {
            var commandText = "select id,name,start_time,deadline,priority,status" +
                " from tasks " +
                $"where user_name='{userName}'";
            using var connection = new NpgsqlConnection(_connectionString);

            var reader = await ExecuteReaderAsync(connection, commandText);

            return await TaskDataBuilder.BuildTaskDataByDataReaderAsync(reader);
        }
        public async Task UpdateTask(TaskManagerModel.Task task)
        {
            var commandText = "update tasks " +
                $"set name='{task.Name}', start_time='{task.StartTime}'," +
                $"deadline='{task.Deadline}',priority='{task.Priority}'," +
                $" status='{task.Status}'" +
                $"where tasks.id='{task.Id}'";
            await ExecuteNonQueryAsync(commandText);
        }
        public async Task<bool> WriteTaskAsync(string username, TaskManagerModel.Task taskData)
        {
            var commandText = "insert into tasks(id,name,start_time,deadline,priority,user_name,status)" +
                $"values ('{taskData.Id}','{taskData.Name}','{taskData.StartTime}'," +
                $"'{taskData.Deadline}','{taskData.Priority}','{username}','{taskData.Status}')";
            var result = await ExecuteNonQueryAsync(commandText);
            return result > 0;
        }

        public async Task<bool> RemoveTaskAsync(string userName, Guid id)
        {
            var commandText = $"delete from tasks where id = '{id}'" +
                $" and user_name='{userName}'";
            var result = await ExecuteNonQueryAsync(commandText);
            return result > 0;
        }

        public async Task<bool> SetTaskStatusAsync(string userName, Guid id, TaskManagerModel.TaskStatus status)
        {
            var commandText = $"update tasks set status='{status}'" +
                $" where id='{id}' and user_name='{userName}'";
            var result = await ExecuteNonQueryAsync(commandText);
            return result > 0;
        }

        public async Task<string[]> GetUsersAsync(string prompt)
        {
            var commandText = $"select \"name\" from userdata" +
                $" where \"name\" like '{prompt}%' limit 5";
            using var connection = new NpgsqlConnection(_connectionString);

            var reader = await ExecuteReaderAsync(connection, commandText);
            return await UserNamesBuilder.BuildUserNamesByDataReaderAsync(reader);
        }

        public async Task<bool> AddOrderAsync(string userName, string friendName)
        {
            var commandText = $"insert into \"order\" values('{userName}','{friendName}')";
            try
            {
                return await ExecuteNonQueryAsync(commandText) > 0;
            }
            catch
            {
                return true;
            }
        }

        public async Task<string[]> GetOrdersAsync(string userName)
        {
            var commandText = $"select user_name from \"order\"" +
                $" where friend_name = '{userName}'";
            using var connection = new NpgsqlConnection(_connectionString);

            var reader = await ExecuteReaderAsync(connection, commandText);
            return await UserNamesBuilder.BuildUserNamesByDataReaderAsync(reader);
        }

        public async Task<string[]> GetFriendsAsync(string userName)
        {
            var commandText = $"select friend_name from \"friends\"" +
                $" where user_name = '{userName}'";
            using var connection = new NpgsqlConnection(_connectionString);

            var reader = await ExecuteReaderAsync(connection, commandText);
            return await UserNamesBuilder.BuildUserNamesByDataReaderAsync(reader);
        }
    }
}
