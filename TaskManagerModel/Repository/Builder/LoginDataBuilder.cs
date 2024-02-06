using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Model.Builder
{
    public static class LoginDataBuilder
    {
        public static async Task<LoginData> BuildLoginDataByDataReaderAsync(NpgsqlDataReader reader)
        {
            var loginData = new LoginData();
            while (await reader.ReadAsync())
            {
                loginData.UserName = reader.GetString(0);
                loginData.Email = reader.GetString(1);
            }

            return loginData;
        }
    }
}
