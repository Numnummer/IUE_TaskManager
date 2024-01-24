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
        protected async Task<object?> ExecuteScalarAsync(string commandText, NpgsqlParameter[]? parameters = null)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new NpgsqlCommand(commandText, connection);
            if (parameters != null) command.Parameters.AddRange(parameters);
            return await command.ExecuteScalarAsync();
        }
        protected async Task<int> ExecuteNonQueryAsync(string commandText, NpgsqlParameter[]? parameters = null)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new NpgsqlCommand(commandText, connection);
            if (parameters != null) command.Parameters.AddRange(parameters);
            return await command.ExecuteNonQueryAsync();
        }

        protected async Task<NpgsqlDataReader> ExecuteReaderAsync(NpgsqlConnection connection, string commandText, NpgsqlParameter[]? parameters = null)
        {
            await connection.OpenAsync();
            using var command = new NpgsqlCommand(commandText, connection);
            if (parameters != null) command.Parameters.AddRange(parameters);
            return await command.ExecuteReaderAsync();
        }

        protected async Task<bool> ExecuteTransactionAsync(string[] commands, NpgsqlParameter[][] parameters)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            var transaction = await connection.BeginTransactionAsync();
            try
            {
                var commandList = new List<NpgsqlCommand>();
                var paramIndex = 0;
                foreach (var command in commands)
                {
                    var commandInstance = new NpgsqlCommand(command, connection);
                    commandInstance.Parameters.AddRange(parameters[paramIndex]);
                    commandList.Add(commandInstance);
                    paramIndex++;
                }
                foreach (var command in commandList)
                {
                    if (await command.ExecuteNonQueryAsync()==0)
                    {
                        throw new Exception();
                    }
                }
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
            return true;
        }

        protected NpgsqlParameter[] MakeParameters(params (string?, object?)[] parametrs)
        {
            var result = new List<NpgsqlParameter>();
            foreach (var param in parametrs)
            {
                result.Add(new NpgsqlParameter(param.Item1, param.Item2));
            }
            return result.ToArray();
        }
    }
}
