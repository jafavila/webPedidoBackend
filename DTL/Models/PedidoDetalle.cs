namespace DTL.Models
{
    public class PedidoDetalle
    {
        public int IdPedidoDetalle { get; set; }
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}
