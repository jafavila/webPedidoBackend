using DTL.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductoDAL
    {
        private readonly PostgresHelper _helper;

        public ProductoDAL(PostgresHelper helper)
        {
            _helper = helper;
        }

        public async Task<bool> MttoProducto(Producto producto, char accion)
        {
            var parameters = new NpgsqlParameter[]
            {
                new("p_idproducto", producto.IdProducto == 0 ? DBNull.Value : producto.IdProducto),
                new("p_producto", (object?)producto.Nombre ?? DBNull.Value),
                new("p_descripcion", (object?)producto.Descripcion ?? DBNull.Value),
                new("p_existencia", producto.Existencia),
                new("p_precio", producto.Precio),
                new("p_accion", accion)
            };

            await _helper.ExecuteNonQueryStoredProcedureAsync("spMttoCatProducto", parameters);
            return true;
        }

        public async Task<List<Producto>> ObtenerProductos()
        {
            var dt = await _helper.ExecuteStoredProcedureQueryAsync("SELECT * FROM catProducto ORDER BY idProducto");
            var lista = new List<Producto>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Producto
                {
                    IdProducto = Convert.ToInt32(row["idProducto"]),
                    Nombre = row["producto"].ToString()!,
                    Descripcion = row["descripcion"].ToString()!,
                    Existencia = Convert.ToInt32(row["existencia"]),
                    Precio = Convert.ToDecimal(row["precio"])
                });
            }
            return lista;
        }
    }
}
