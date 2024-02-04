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
using System.Xml.Linq;

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
                $"values(@UserName,@Email,@Login,@Password)";
            var parameters = MakeParameters(("@UserName", loginData.UserName), ("@Email", loginData.Email), ("@Login", loginData.Login), ("@Password", hashedPassword));
            var updated = await ExecuteNonQueryAsync(commandText, parameters);
            return updated > 0;
        }

        public async Task<bool> IsExistUserNameAsync(string userName)
        {
            var commandText = $"select * from userdata where name=@UserName";
            var parameters = MakeParameters(("@UserName", userName));
            var updated = await ExecuteScalarAsync(commandText, parameters);
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
                $"name=@UserName and password=@Password";
            var parameters = MakeParameters(("@UserName", loginData.UserName), ("@Password", hashedPassword));
            var updated = await ExecuteScalarAsync(commandText, parameters);
            return updated != null;
        }

        public async Task<LoginData> GetUserDataAsync(string name)
        {
            var commandText = "select * from userdata " +
                $"where name=@UserName";
            var parameters = MakeParameters(("@UserName", name));
            using var connection = new NpgsqlConnection(_connectionString);

            var reader = await ExecuteReaderAsync(connection, commandText, parameters);

            return await LoginDataBuilder.BuildLoginDataByDataReaderAsync(reader);
        }
        public async Task<TaskManagerModel.Task[]> GetTaskDataAsync(string userName)
        {
            var commandText = "select id,name,start_time,deadline,priority,status" +
                " from tasks " +
                $"where user_name=@UserName";
            var parameters = MakeParameters(("@UserName", userName));

            using var connection = new NpgsqlConnection(_connectionString);

            var reader = await ExecuteReaderAsync(connection, commandText, parameters);
            return await TaskDataBuilder.BuildTaskDataByDataReaderAsync(reader);
        }
        public async Task UpdateTask(TaskManagerModel.Task task)
        {
            var commandText = "update tasks " +
                $"set name=@TaskName, start_time=@StartTime," +
                $"deadline=@Deadline,priority=@Priority," +
                $" status=@Status" +
                $"where tasks.id=@Id";
            var tuples = new List<(string, object?)>()
            {
                ("@TaskName", task.Name),
                ("@StartTime", task.StartTime),
                ("@Deadline", task.Deadline),
                ("@Priority", (int)task.Priority),
                ("@Status", task.Status.ToString()),
                ("@Id", task.Id)
            };
            var parameters = MakeParameters(tuples.ToArray());
            await ExecuteNonQueryAsync(commandText, parameters);
        }
        public async Task<bool> WriteTaskAsync(string username, TaskManagerModel.Task taskData)
        {
            var commandText = "insert into tasks(id,name,start_time,deadline,priority,user_name,status)" +
                $"values (@Id,@TaskName,@StartTime," +
                $"@Deadline,@Priority,@UserName,@Status)";
            var tuples = new List<(string, object?)>()
            {
                ("@TaskName", taskData.Name),
                ("@StartTime", taskData.StartTime),
                ("@Deadline", taskData.Deadline),
                ("@Priority", (int)taskData.Priority),
                ("@Status", taskData.Status.ToString()),
                ("@Id", taskData.Id),
                ("@UserName", username)
            };
            var parameters = MakeParameters(tuples.ToArray());
            var result = await ExecuteNonQueryAsync(commandText, parameters);
            return result > 0;
        }

        public async Task<bool> RemoveTaskAsync(string userName, Guid id)
        {
            var commandText = $"delete from tasks where id = @Id" +
                $" and user_name=@UserName";
            var parameters = MakeParameters(("@Id", id), ("@UserName", userName));
            var result = await ExecuteNonQueryAsync(commandText, parameters);
            return result > 0;
        }

        public async Task<bool> SetTaskStatusAsync(string userName, Guid id, TaskManagerModel.TaskStatus status)
        {
            var commandText = $"update tasks set status=@Status" +
                $" where id=@Id and user_name=@UserName";
            var parameters = MakeParameters(("@Status", status.ToString()), ("@UserName", userName), ("@Id", id));
            var result = await ExecuteNonQueryAsync(commandText, parameters);
            return result > 0;
        }

        public async Task<string[]> GetUsersAsync(string prompt)
        {
            var commandText = $"select \"name\" from userdata" +
                $" where \"name\" like @Prompt || '%' limit 5";
            var parameters = MakeParameters(("@Prompt", prompt));
            using var connection = new NpgsqlConnection(_connectionString);

            var reader = await ExecuteReaderAsync(connection, commandText, parameters);
            return await UserNamesBuilder.BuildUserNamesByDataReaderAsync(reader);
        }

        public async Task<bool> AddOrderAsync(string userName, string friendName)
        {
            var commandText = $"insert into \"order\" values(@UserName,@FriendName)";
            var parameters = MakeParameters(("@UserName", userName), ("@FriendName", friendName));
            return await ExecuteNonQueryAsync(commandText, parameters) > 0;
        }

        public async Task<string[]> GetOrdersAsync(string userName)
        {
            var commandText = $"select user_name from \"order\"" +
                $" where friend_name = @UserName";
            var parameters = MakeParameters(("@UserName", userName));
            using var connection = new NpgsqlConnection(_connectionString);

            var reader = await ExecuteReaderAsync(connection, commandText, parameters);
            return await UserNamesBuilder.BuildUserNamesByDataReaderAsync(reader);
        }

        public async Task<string[]> GetFriendsAsync(string userName)
        {
            var commandText = $"select friend_name from \"friends\"" +
                $" where user_name = @UserName";
            var parameters = MakeParameters(("@UserName", userName));
            using var connection = new NpgsqlConnection(_connectionString);

            var reader = await ExecuteReaderAsync(connection, commandText, parameters);
            return await UserNamesBuilder.BuildUserNamesByDataReaderAsync(reader);
        }

        public async Task<bool> AcceptOrderAsync(string? order, string userName)
        {
            var commands = new string[]
            {
                $"delete from \"order\" where user_name = @Order and friend_name =@UserName",
                $"insert into friends values (@UserName,@Order)",
                $"insert into friends values (@Order,@UserName)"
            };
            var parameters = new List<NpgsqlParameter[]>()
            {
                MakeParameters(("@Order",order),("@UserName",userName)),
                MakeParameters(("@Order",order),("@UserName",userName)),
                MakeParameters(("@Order",order),("@UserName",userName)),
            };
            return await ExecuteTransactionAsync(commands, parameters.ToArray());
        }

        public async Task<bool> HasFriendAsync(string userName, string friend)
        {
            var commandText = $"select 1 from \"friends\"" +
                $" where user_name = @UserName and friend_name=@FriendName";
            var parameters = MakeParameters(("@UserName", userName), ("@FriendName", friend));
            return ExecuteScalarAsync(commandText, parameters)!=null;
        }
    }
}
