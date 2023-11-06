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
            if (loginData==null || !ValidationHelper.IsValidLoginData(loginData))
            {
                return false;
            }
            var hashedPassword = CryptoHelper.HashString(loginData.Password);
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            var commandText = "insert into userdata(name,email,login,password) " +
                $"values('{loginData.UserName}','{loginData.Email}','{loginData.Login}','{hashedPassword}')";
            using var command = new NpgsqlCommand(commandText, connection);
            var updated = await command.ExecuteNonQueryAsync();
            return updated>0;
        }
    }
}
