using Microsoft.AspNetCore.Mvc;
using Back_health.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_health.Controllers.HeathAdmin.PacientesController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : Controller
    {
        private readonly HealtPlusContext _baseDatos;

        ///GENERAMOS CONSTRUCTOR
        public PacientesController(HealtPlusContext baseDatos) { this._baseDatos = baseDatos; }

        //POST AGREGAR PACIENTES
        [HttpPost]
        [Route("AgregarPaciente")]
        public async Task<IActionResult> AgregarPaciente([FromBody] PacienteRequest request)
        {
            using (var transaction = _baseDatos.Database.BeginTransaction())
            {
                try
                {
                    // Agregar persona
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
                        Rol = "Enfermero"
                    };

                    _baseDatos.Usuarios.Add(usuario);
                    await _baseDatos.SaveChangesAsync();

                    // Agregar paciente
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
                        IdUsuario = usuario.IdUsuario,
                    };

                    _baseDatos.Pacientes.Add(paciente);
                    await _baseDatos.SaveChangesAsync();

                    // Agregar PacientePadecimiento
                    var pacientePadecimiento = new PacientePadecimiento
                    { IdPaciente = paciente.IdPaciente, IdPadecimiento = request.IdPadecimiento };

                    _baseDatos.PacientePadecimientos.Add(pacientePadecimiento);
                    await _baseDatos.SaveChangesAsync();

                    transaction.Commit();

                    return Ok(paciente);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    Console.WriteLine($"Error: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    return StatusCode(500, $"Error interno del servidor: {ex.Message}");
                }
            }
        }


    }
}
