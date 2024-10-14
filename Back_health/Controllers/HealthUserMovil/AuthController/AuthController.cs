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
                // Buscar el usuario en la base de datos
                var usuario = _context.Usuarios
                    .FirstOrDefault(u => u.Usuario1 == request.NombreUsuario && u.Contrasenia == request.Contrasenia);

                // Verificar si el usuario existe y tiene el rol de "Enfermero"
                if (usuario != null && usuario.Rol == "Paciente")
                {
                    // Iniciar sesión correctamente
                    return Ok(new { message = "Inicio de sesión exitoso", usuario = usuario.IdUsuario });
                }

                // Usuario no encontrado o no es enfermero
                return Unauthorized(new { message = "Credenciales incorrectas o rol no autorizado" });
            }
        }

        public class UsuarioUserRequest
        {
            public string NombreUsuario { get; set; }
            public string Contrasenia { get; set; }
        }
    }

