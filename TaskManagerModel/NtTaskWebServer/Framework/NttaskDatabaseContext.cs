using Npgsql;
using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework
{
    public class NttaskDatabaseContext : DatabaseContext
    {
        public NttaskDatabaseContext() : base(ConfigurationManager.ConnectionStrings["nttask"].ConnectionString) { }

        public async Task<bool> WriteLoginDataAsync(LoginData loginData)
        {
            if (loginData==null || !ValidationHelper.IsValidLoginData(loginData)
                || await IsExistUserNameAsync(loginData.UserName))
            {
                return false;
            }
            var hashedPassword = CryptoHelper.HashString(loginData.Password);
            var commandText = "insert into userdata(name,email,login,password) " +
                $"values('{loginData.UserName}','{loginData.Email}','{loginData.Login}','{hashedPassword}')";
            var updated = await ExecuteNonQueryAsync(commandText);
            return updated>0;
        }

        public async Task<bool> IsExistUserNameAsync(string userName)
        {
            var commandText = $"select * from userdata where name='{userName}'";
            var updated = await ExecuteScalarAsync(commandText);
            return updated!=null;
        }

        public async Task<bool> IsLoginDataExistAsync(LoginData? loginData)
        {
            if (loginData==null || !ValidationHelper.IsValidLoginDataForEnter(loginData)
                || !await IsExistUserNameAsync(loginData.UserName))
            {
                return false;
            }
            var hashedPassword = CryptoHelper.HashString(loginData.Password);
            var commandText = $"select * from userdata where " +
                $"name='{loginData.UserName}' and password='{hashedPassword}'";
            var updated = await ExecuteScalarAsync(commandText);
            return updated!=null;
        }

        private async Task<object?> ExecuteScalarAsync(string commandText)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new NpgsqlCommand(commandText, connection);
            return await command.ExecuteScalarAsync();
        }
        private async Task<int> ExecuteNonQueryAsync(string commandText)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new NpgsqlCommand(commandText, connection);
            return await command.ExecuteNonQueryAsync();
        }
    }
}
