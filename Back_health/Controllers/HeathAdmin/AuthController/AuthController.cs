using Microsoft.AspNetCore.Mvc;
using Back_health.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Back_health.Controllers.HeathAdmin.AuthController
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        //Variable de contexto de base de datos
        private readonly HealtPlusContext _baseDatos;

        ///GENERAMOS CONSTRUCTOR
        public AuthController(HealtPlusContext baseDatos) { this._baseDatos = baseDatos; }

        //---------------------------------------- AUTH -------------------------------------------

        //METODO GET (LOGIIN)
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLogin request)
        {
            var usuario = await _baseDatos.Usuarios
                .FirstOrDefaultAsync(u => u.Usuario1 == request.user && u.Contrasenia == request.contrasenia);

            if (usuario == null){
                return Unauthorized(new { isSuccess = false, message = "Usuario o contraseña incorrectos" });
            }

            if (usuario.Rol != "Doctor"){
                return Unauthorized(new { isSuccess = false, message = "No es doctor" });
            }

            var userName = usuario.Usuario1;
            var token = GenerateJwtToken(usuario);
            return Ok(new { isSuccess = true, token, userName });
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("esta_es_mi_secret_key_yes_mucho_mas_grande");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", usuario.IdUsuario.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
