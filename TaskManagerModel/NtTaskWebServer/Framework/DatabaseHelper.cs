using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework
{
    public static class DatabaseHelper
    {
        private static readonly NttaskDatabaseContext databaseContext = new();
        public static async Task<bool> WriteLoginDataAsync(LoginData? loginData)
        {
            return await databaseContext.WriteLoginDataAsync(loginData);
        }
    }
}
