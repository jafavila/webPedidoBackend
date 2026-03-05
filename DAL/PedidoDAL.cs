using DTL.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DAL
{
    public class PedidoDAL
    {
        private readonly PostgresHelper _helper;

        public PedidoDAL(PostgresHelper helper)
        {
            _helper = helper;
        }

        public async Task<int> InsertarPedido(Pedido pedido)
        {
            // First insert header and get ID (Postgres SPs normally don't return via RETURN like SQL Server, but we can query it or use a trigger)
            // However, the provided spMttoTblPedido inserts. We might need to select the last ID or modify flow.
            // For now, let's follow the SP provided.
            
            var parameters = new NpgsqlParameter[]
            {
                new("p_idpedido", DBNull.Value),
                new("p_idcliente", pedido.IdCliente),
                new("p_fecha", pedido.Fecha),
                new("p_total", pedido.Total),
                new("p_estatus", pedido.Estatus),
                new("p_accion", 'i')
            };

            await _helper.ExecuteNonQueryStoredProcedureAsync("spMttoTblPedido", parameters);
            
            // Get the last inserted ID for this client (simplified for this exercise)
            var dt = await _helper.ExecuteStoredProcedureQueryAsync($"SELECT idPedido FROM tblPedido WHERE idCliente = {pedido.IdCliente} ORDER BY idPedido DESC LIMIT 1");
            return Convert.ToInt32(dt.Rows[0]["idPedido"]);
        }

    

        public async Task<bool> InsertarDetalle(PedidoDetalle detalle)
{
    var parameters = new NpgsqlParameter[]
    {
        new("p_idpedidodetalle", DBNull.Value),
        new("p_idpedido", detalle.IdPedido),
        new("p_idproducto", detalle.IdProducto),
        new("p_cantidad", detalle.Cantidad),
        new("p_total", detalle.Total),
        new("p_accion", 'i')
    };
    
    // Llamamos al nuevo SP
    await _helper.ExecuteNonQueryStoredProcedureAsync("spMttoTblPedidoDetalle", parameters);
    return true;
}


        public async Task<List<Pedido>> ObtenerPedidos(int? idCliente = null)
        {
            string filter = idCliente.HasValue ? $" WHERE p.idCliente = {idCliente.Value} " : "";
            var dt = await _helper.ExecuteStoredProcedureQueryAsync($@"
                SELECT p.*, c.nombre as nombreCliente 
                FROM tblPedido p 
                INNER JOIN catCliente c ON p.idCliente = c.idCliente 
                {filter}
                ORDER BY p.idPedido DESC");
            
            var lista = new List<Pedido>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Pedido
                {
                    IdPedido = Convert.ToInt32(row["idPedido"]),
                    IdCliente = Convert.ToInt32(row["idCliente"]),
                    Fecha = Convert.ToDateTime(row["fecha"]),
                    Total = Convert.ToDecimal(row["total"]),
                    Estatus = Convert.ToBoolean(row["estatus"]),
                    NombreCliente = row["nombreCliente"].ToString()!
                });
            }
            return lista;
        }

        public async Task<List<PedidoDetalle>> ObtenerDetalle(int idPedido)
        {
            var dt = await _helper.ExecuteStoredProcedureQueryAsync($@"
                SELECT d.*, p.producto as nombreProducto 
                FROM tblPedidoDetalle d 
                INNER JOIN catProducto p ON d.idProducto = p.idProducto 
                WHERE d.idPedido = {idPedido}");
            
            var lista = new List<PedidoDetalle>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new PedidoDetalle
                {
                    IdPedidoDetalle = Convert.ToInt32(row["idPedidoDetalle"]),
                    IdPedido = Convert.ToInt32(row["idPedido"]),
                    IdProducto = Convert.ToInt32(row["idProducto"]),
                    Producto = row["nombreProducto"].ToString()!,
                    Cantidad = Convert.ToInt32(row["cantidad"]),
                    Total = Convert.ToDecimal(row["total"])
                });
            }
            return lista;
        }
    }
}
