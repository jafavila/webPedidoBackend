using System;
using System.Collections.Generic;

namespace DTL.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public bool Estatus { get; set; }
        
        // Navigation / Auxiliary
        public string NombreCliente { get; set; } = string.Empty;
        public List<PedidoDetalle> Detalles { get; set; } = new();
    }
}
