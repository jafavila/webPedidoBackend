using DTL.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DAL
{
    public class ClienteDAL
    {
        private readonly PostgresHelper _helper;

        public ClienteDAL(PostgresHelper helper)
        {
            _helper = helper;
        }

        public async Task<bool> MttoCliente(Cliente cliente, char accion)
        {
            var parameters = new NpgsqlParameter[]
            {
                new("p_idcliente", cliente.IdCliente == 0 ? DBNull.Value : cliente.IdCliente),
                new("p_nombre", (object?)cliente.Nombre ?? DBNull.Value),
                new("p_correo", (object?)cliente.Correo ?? DBNull.Value),
                new("p_password", (object?)cliente.Password ?? DBNull.Value),
                new("p_isadmin", cliente.IsAdmin),
                new("p_intento", cliente.Intento),
                new("p_accion", accion)
            };

            await _helper.ExecuteNonQueryStoredProcedureAsync("spMttoCatCliente", parameters);
            return true;
        }

        public async Task<List<Cliente>> ObtenerClientes()
        {
            var dt = await _helper.ExecuteStoredProcedureQueryAsync("SELECT * FROM catCliente ORDER BY idCliente");
            var lista = new List<Cliente>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Cliente
                {
                    IdCliente = Convert.ToInt32(row["idCliente"]),
                    Nombre = row["nombre"].ToString()!,
                    Correo = row["correo"].ToString()!,
                    IsAdmin = Convert.ToBoolean(row["isAdmin"]),
                    Intento = Convert.ToBoolean(row["intento"]),
                    ConteoIntentos = Convert.ToInt32(row["conteo_intentos"])
                });
            }
            return lista;
        }

        public async Task<bool> Login(string correo, string password)
        {
            var parameters = new NpgsqlParameter[]
            {
                new("p_correo", correo),
                new("p_password", password)
            };
            
            // spLogin uses RAISE EXCEPTION for errors, so we handle success/fail
            try 
            {
                await _helper.ExecuteNonQueryStoredProcedureAsync("spLogin", parameters);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<Cliente?> ObtenerClientePorCorreo(string correo)
        {
            var parameters = new NpgsqlParameter[] { new("p_correo", correo) };
            var dt = await _helper.ExecuteStoredProcedureQueryAsync("SELECT * FROM catCliente WHERE correo = @p_correo", parameters);
            if (dt.Rows.Count == 0) return null;
            var row = dt.Rows[0];
            return new Cliente
            {
                IdCliente = Convert.ToInt32(row["idCliente"]),
                Nombre = row["nombre"].ToString()!,
                Correo = row["correo"].ToString()!,
                IsAdmin = Convert.ToBoolean(row["isAdmin"]),
                Intento = Convert.ToBoolean(row["intento"]),
                ConteoIntentos = Convert.ToInt32(row["conteo_intentos"])
            };
        }
    }
}
