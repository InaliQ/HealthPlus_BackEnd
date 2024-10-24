using Back_health.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back_health.Controllers.HealthUserMovil.AuthController
{
    [Route("api/users/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HealtPlusContext _context;

        public AuthController(HealtPlusContext context)
        {
            _context = context;
        }

        [HttpPost("loginUser")]
        public IActionResult Login([FromBody] UsuarioUserRequest request)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Usuario1 == request.NombreUsuario && u.Contrasenia == request.Contrasenia);

            if (usuario != null && usuario.Rol == "Paciente")
            {
                var paciente = _context.Pacientes
                    .FirstOrDefault(p => p.IdUsuario == usuario.IdUsuario);

                if (paciente != null)
                {
                    return Ok(new { message = "Inicio de sesión exitoso", idPaciente = paciente.IdPaciente });
                }
            }

            return Unauthorized(new { message = "Credenciales incorrectas o rol no autorizado" });
        }
    }

    public class UsuarioUserRequest
    {
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
    }
}
