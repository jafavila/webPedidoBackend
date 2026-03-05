using BLL;
using DTL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoBLL _bll;

        public PedidoController(PedidoBLL bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            
            if (userRole == "Admin")
            {
                return Ok(await _bll.ObtenerPedidos()); // All orders for Admin
            }
            else if (userIdClaim != null)
            {
                int idCliente = int.Parse(userIdClaim.Value);
                return Ok(await _bll.ObtenerPedidos(idCliente)); // Only own orders for User
            }
            
            return Unauthorized();
        }

        [HttpGet("{id}/detalle")]
        public async Task<IActionResult> GetDetalle(int id)
        {
            return Ok(await _bll.ObtenerDetalle(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pedido pedido)
        {
            // Set idCliente from claims
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim != null) pedido.IdCliente = int.Parse(claim.Value);
            pedido.Fecha = DateTime.Now;
            pedido.Estatus = true; // Activo/Procesado
            
            int idPedido = await _bll.GuardarPedido(pedido);
            return Ok(new { success = true, idPedido });
        }
    }
}
