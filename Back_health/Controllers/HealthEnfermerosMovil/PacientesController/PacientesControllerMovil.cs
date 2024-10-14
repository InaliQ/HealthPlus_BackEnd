using Back_health.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Back_health.Controllers.HealthEnfermerosMovil.PacientesController
{
    [ApiController]
    [Route("[controller]")]
    public class PacientesControllerMovil : Controller
    {
        private readonly HealtPlusContext _baseDatos;

        public PacientesControllerMovil(HealtPlusContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        // POST: Agregar Paciente
        [HttpPost]
        [Route("AgregarPaciente")]
        public async Task<IActionResult> AgregarPaciente([FromBody] PacienteRequest request)
        {
            if (request == null)
            {
                return BadRequest("La solicitud es nula.");
            }

            await using var transaction = await _baseDatos.Database.BeginTransactionAsync();
            try
            {
                var persona = new Persona
                {
                    Nombre = request.Nombre,
                    PrimerApellido = request.PrimerApellido,
                    SegundoApellido = request.SegundoApellido,
                    Telefono = request.Telefono,
                    FechaNacimiento = request.FechaNacimiento,
                    Calle = request.Calle,
                    Numero = request.Numero,
                    CodigoPostal = request.CodigoPostal,
                    Colonia = request.Colonia
                };

                _baseDatos.Personas.Add(persona);
                await _baseDatos.SaveChangesAsync();

                var usuario = new Usuario
                {
                    Usuario1 = request.Usuario1,
                    Contrasenia = request.Contrasenia,
                    Estatus = true,
                    Rol = "Paciente"
                };

                _baseDatos.Usuarios.Add(usuario);
                await _baseDatos.SaveChangesAsync();

                var paciente = new Paciente
                {
                    NumPaciente = request.NumPaciente,
                    Altura = request.Altura,
                    Peso = request.Peso,
                    TipoSangre = request.TipoSangre,
                    Estatus = request.Estatus,
                    IdPersona = persona.IdPersona,
                    IdAlerta = null,
                    IdRecordatorio = null,
                    IdUsuario = usuario.IdUsuario
                };

                _baseDatos.Pacientes.Add(paciente);
                await _baseDatos.SaveChangesAsync();

                foreach (var idPadecimiento in request.IdPadecimientos)
                {
                    var pacientePadecimiento = new PacientePadecimiento
                    {
                        IdPaciente = paciente.IdPaciente,
                        IdPadecimiento = idPadecimiento
                    };

                    _baseDatos.PacientePadecimientos.Add(pacientePadecimiento);
                }

                await _baseDatos.SaveChangesAsync();

                // Confirmar la transacción
                await transaction.CommitAsync();

                return Ok(paciente);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                return StatusCode(500, "Error interno del servidor.");
            }
        }



        // PUT: Actualizar Paciente
        [HttpPut]
        [Route("ActualizarPaciente/{id}")]
        public async Task<IActionResult> ActualizarPaciente(int id, [FromBody] PacienteRequest request)
        {
            if (request == null)
            {
                return BadRequest("La solicitud es nula.");
            }

            var paciente = await _baseDatos.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound("Paciente no encontrado.");
            }

            await using var transaction = await _baseDatos.Database.BeginTransactionAsync();
            try
            {
                // Actualizar los datos de la persona asociada
                var persona = await _baseDatos.Personas.FindAsync(paciente.IdPersona);
                if (persona != null)
                {
                    persona.Nombre = request.Nombre;
                    persona.PrimerApellido = request.PrimerApellido;
                    persona.SegundoApellido = request.SegundoApellido;
                    persona.Telefono = request.Telefono;
                    persona.FechaNacimiento = request.FechaNacimiento;
                    persona.Calle = request.Calle;
                    persona.Numero = request.Numero;
                    persona.CodigoPostal = request.CodigoPostal;
                    persona.Colonia = request.Colonia;

                    _baseDatos.Personas.Update(persona);
                }

                // Actualizar los datos del usuario asociado
                var usuario = await _baseDatos.Usuarios.FindAsync(paciente.IdUsuario);
                if (usuario != null)
                {
                    usuario.Usuario1 = request.Usuario1;
                    usuario.Contrasenia = request.Contrasenia;
                    usuario.Estatus = true;

                    _baseDatos.Usuarios.Update(usuario);
                }

                // Actualizar los datos del paciente
                paciente.NumPaciente = request.NumPaciente;
                paciente.Altura = request.Altura;
                paciente.Peso = request.Peso;
                paciente.TipoSangre = request.TipoSangre;
                paciente.Estatus = request.Estatus;

                _baseDatos.Pacientes.Update(paciente);
                await _baseDatos.SaveChangesAsync();

                return Ok("Paciente actualizado con éxito.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}