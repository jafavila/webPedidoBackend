using BLL;
using DTL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoBLL _bll;

        public ProductoController(ProductoBLL bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _bll.ObtenerProductos());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] Producto producto)
        {
            await _bll.MttoProducto(producto, 'i');
            return Ok(new { success = true });
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put([FromBody] Producto producto)
        {
            await _bll.MttoProducto(producto, 'u');
            return Ok(new { success = true });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bll.MttoProducto(new Producto { IdProducto = id }, 'd');
            return Ok(new { success = true });
        }
    }
}
