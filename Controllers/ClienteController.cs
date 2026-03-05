using BLL;
using DTL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteBLL _bll;

        public ClienteController(ClienteBLL bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _bll.ObtenerClientes());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente)
        {
            await _bll.MttoCliente(cliente, 'i');
            return Ok(new { success = true });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Cliente cliente)
        {
            await _bll.MttoCliente(cliente, 'u');
            return Ok(new { success = true });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bll.MttoCliente(new Cliente { IdCliente = id }, 'd');
            return Ok(new { success = true });
        }
    }
}
