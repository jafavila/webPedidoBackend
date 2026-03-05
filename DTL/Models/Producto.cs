namespace DTL.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int Existencia { get; set; }
        public decimal Precio { get; set; }
    }
}
