using DAL;
using DTL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class ProductoBLL
    {
        private readonly ProductoDAL _dal;

        public ProductoBLL(ProductoDAL dal)
        {
            _dal = dal;
        }

        public Task<bool> MttoProducto(Producto producto, char accion) => _dal.MttoProducto(producto, accion);
        public Task<List<Producto>> ObtenerProductos() => _dal.ObtenerProductos();
    }
}
