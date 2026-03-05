using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DAL
{
    public class PostgresHelper
    {
        private readonly string _connectionString;

        public PostgresHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? "Host=localhost;Database=webPedido;Username=usrWebPedido;Password=MiWebPedido";
        }

        public async Task<int> ExecuteNonQueryStoredProcedureAsync(string procName, NpgsqlParameter[] parameters)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand($"CALL {procName}({GetParamPlaceholders(parameters)})", connection))
                {
                    command.Parameters.AddRange(parameters);
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<DataTable> ExecuteStoredProcedureQueryAsync(string query, NpgsqlParameter[]? parameters = null)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (parameters != null) command.Parameters.AddRange(parameters);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
            }
        }

        private string GetParamPlaceholders(NpgsqlParameter[] parameters)
        {
            var placeholders = new List<string>();
            foreach (var p in parameters)
            {
                placeholders.Add(p.ParameterName.StartsWith(":") || p.ParameterName.StartsWith("@") ? p.ParameterName : "@" + p.ParameterName);
            }
            return string.Join(", ", placeholders);
        }
    }
}
