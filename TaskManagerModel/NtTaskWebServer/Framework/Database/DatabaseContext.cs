using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Database
{
    public abstract class DatabaseContext
    {
        protected readonly string _connectionString;
        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected async Task<object?> ExecuteScalarAsync(string commandText)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new NpgsqlCommand(commandText, connection);
            return await command.ExecuteScalarAsync();
        }
        protected async Task<int> ExecuteNonQueryAsync(string commandText)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new NpgsqlCommand(commandText, connection);
            return await command.ExecuteNonQueryAsync();
        }

        protected async Task<NpgsqlDataReader> ExecuteReaderAsync(NpgsqlConnection connection, string commandText)
        {
            await connection.OpenAsync();
            using var command = new NpgsqlCommand(commandText, connection);
            return await command.ExecuteReaderAsync();
        }

    }
}
