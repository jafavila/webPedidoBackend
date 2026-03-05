using BLL;
using DTL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ClienteBLL _clienteBll;
        private readonly IConfiguration _config;

        public AuthController(ClienteBLL clienteBll, IConfiguration config)
        {
            _clienteBll = clienteBll;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                bool success = await _clienteBll.Login(request.Correo, request.Password);
                if (success)
                {
                    var cliente = await _clienteBll.ObtenerClientePorCorreo(request.Correo);
                    if (cliente == null) return Unauthorized(new { message = "Error al obtener datos del cliente" });
                    if (cliente.Intento) return Unauthorized(new { message = "Usuario bloqueado" });

                    var token = GenerateJwtToken(cliente);
                    return Ok(new { token, cliente });
                }
                return Unauthorized(new { message = "Credenciales inválidas" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        private string GenerateJwtToken(Cliente cliente)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "EstaEsUnaLlaveSuperSecretaDe32Caracteres!"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.IdCliente.ToString()),
                new Claim(ClaimTypes.Email, cliente.Correo),
                new Claim(ClaimTypes.Role, cliente.IsAdmin ? "Admin" : "User"),
                new Claim("isAdmin", cliente.IsAdmin.ToString().ToLower())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
