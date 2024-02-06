using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Model.Builder
{
    public static class UserNamesBuilder
    {
        public static async Task<string[]> BuildUserNamesByDataReaderAsync(NpgsqlDataReader reader)
        {
            var userNames = new List<string>();
            while (await reader.ReadAsync())
            {
                userNames.Add(reader.GetString(0));
            }

            return userNames.ToArray();
        }
    }
}
