using DAL;
using DTL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class PedidoBLL
    {
        private readonly PedidoDAL _dal;

        public PedidoBLL(PedidoDAL dal)
        {
            _dal = dal;
        }

        public async Task<int> GuardarPedido(Pedido pedido)
        {
            int idPedido = await _dal.InsertarPedido(pedido);
            foreach (var detalle in pedido.Detalles)
            {
                detalle.IdPedido = idPedido;
                await _dal.InsertarDetalle(detalle);
            }
            return idPedido;
        }

        public Task<List<Pedido>> ObtenerPedidos(int? idCliente = null) => _dal.ObtenerPedidos(idCliente);
        public Task<List<PedidoDetalle>> ObtenerDetalle(int idPedido) => _dal.ObtenerDetalle(idPedido);
    }
}
