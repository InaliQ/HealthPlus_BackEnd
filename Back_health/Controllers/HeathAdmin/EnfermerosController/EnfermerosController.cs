using Back_health.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_health.Controllers.HeathAdmin.EnfermerosController
{

    [Route("api/[controller]")]
    [ApiController]
    public class EnfermerosController : Controller
    {
        //Variable de contexto de base de datos
        private readonly HealtPlusContext _baseDatos;

        ///GENERAMOS CONSTRUCTOR
        public EnfermerosController(HealtPlusContext baseDatos) { this._baseDatos = baseDatos; }

        //METODO GET BUSCAR ENFERMERO POR NOMBRE
        [HttpGet]
        [Route("BuscarEPorNombre")]
        public async Task<IActionResult> BuscarEPorNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest("vacia");
            }

            var enfermeros = from en in _baseDatos.Enfermeros
                             join pe in _baseDatos.Personas on en.IdPersona equals pe.IdPersona
                             where pe.Nombre.Contains(nombre)
                             select new
                             {
                                 idEnfermero = en.IdEnfermero,
                                 Nombre = pe.Nombre,
                                 primerApellido = pe.PrimerApellido,
                                 segundoApellido = pe.SegundoApellido,
                                 telefono = pe.Telefono,
                                 fechaNacimiento = pe.FechaNacimiento,
                                 calle = pe.Calle,
                                 numero = pe.Numero,
                                 colonia = pe.Colonia,
                                 codigoPostal = pe.CodigoPostal,
                                 titulo = en.Titulo,
                                 numEnfermero = en.NumEnfermero
                             };

            if (!enfermeros.Any())
            {
                return NotFound("No hay.");
            }

            return Ok(enfermeros);
        }

        //METODO GET (OBTIENE ENFERMEROS)
        [HttpGet]
        [Route("ListarEnfermeros")]
        public async Task<IActionResult> ListarEnfermeros()
        {
            var enfermeros = from en in _baseDatos.Enfermeros
                                   join pe in _baseDatos.Personas on en.IdPersona equals pe.IdPersona
                                   select new {
                                       idEnfermero = en.IdEnfermero,
                                       nombre = pe.Nombre,
                                       primerApellido = pe.PrimerApellido,
                                       segundoApellido = pe.SegundoApellido,
                                       telefono = pe.Telefono,
                                       fechaNacimiento = pe.FechaNacimiento,
                                       calle = pe.Calle,
                                       numero = pe.Numero,
                                       colonia = pe.Colonia,
                                       codigoPostal = pe.CodigoPostal,
                                       titulo = en.Titulo,
                                       numEnfermero = en.NumEnfermero
                                   };

            return Ok(enfermeros); 
        }

        //POST AGREGAR ENFERMEROS
        [HttpPost]
        [Route("AgregarEnfermero")]
        public async Task<IActionResult> AgregarEnfermero([FromBody] EnfermeroRequest request)
        {
            using (var transaction = _baseDatos.Database.BeginTransaction())
            {
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
                        Rol = "Enfermero"
                    };

                    _baseDatos.Usuarios.Add(usuario);
                    await _baseDatos.SaveChangesAsync();

                    var enfermero = new Enfermero
                    {
                        Titulo = request.Titulo,
                        NumEnfermero = request.NumEnfermero,
                        IdPersona = persona.IdPersona,
                        IdUsuario = usuario.IdUsuario
                    };

                    _baseDatos.Enfermeros.Add(enfermero);
                    await _baseDatos.SaveChangesAsync();

                    var enfermero_turno = new EnfermeroTurno
                    {
                        IdEnfermero = enfermero.IdEnfermero,
                        Turno = request.Turno
                    };

                    _baseDatos.EnfermeroTurnos.Add(enfermero_turno);
                    await _baseDatos.SaveChangesAsync();

                    transaction.Commit();
                    return Ok(enfermero);
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

        // ASIGNAR PACIENTES A ENFERMEROS
        [HttpPost]
        [Route("AsignarPaciente")]
        public async Task<IActionResult> AsignarPaciente([FromBody] EnfermeroPacienteRequest request)
        {
            if (request == null)
            {
                return BadRequest("Datos inválidos");
            }

            using (var transaction = await _baseDatos.Database.BeginTransactionAsync())
            {
                try
                {
                    // Validar que los datos no sean nulos
                    if (request.IdEnfermero == 0 || request.IdPaciente == 0 || request.IdEnfermeroTurno == 0)
                    {
                        return BadRequest("Identificadores inválidos");
                    }

                    var enfermeroExiste = await _baseDatos.Enfermeros.AnyAsync(e => e.IdEnfermero == request.IdEnfermero);
                    var pacienteExiste = await _baseDatos.Pacientes.AnyAsync(p => p.IdPaciente == request.IdPaciente);
                    var turnoExiste = await _baseDatos.EnfermeroTurnos.AnyAsync(et => et.IdEnfermeroTurno == request.IdEnfermeroTurno);

                    if (!enfermeroExiste || !pacienteExiste || !turnoExiste)
                    {
                        return NotFound("Enfermero, paciente o turno no encontrado");
                    }

                    var enfermeroPaciente = new HistorialTurno
                    {
                        IdEnfermero = request.IdEnfermero,
                        IdPaciente = request.IdPaciente,
                        IdEnfermeroTurno = request.IdEnfermeroTurno
                    };

                    _baseDatos.HistorialTurnos.Add(enfermeroPaciente);
                    await _baseDatos.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return Ok("Paciente asignado correctamente");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    return StatusCode(500, $"Error interno del servidor: {ex.Message}");
                }
            }
        }



        // PUT MODIFICAR ENFERMERO
        [HttpPut]
        [Route("ModificarEnfermero/{id:int}")]
        public async Task<IActionResult> ModificarEnfermero(int id, [FromBody] EnfermeroRequest request)
        {
            if (request == null)
            {
                return BadRequest("Datos inválidos.");
            }

            using (var transaction = await _baseDatos.Database.BeginTransactionAsync())
            {
                try
                {
                    var enfermeroExistente = await _baseDatos.Enfermeros
                        .Include(e => e.IdPersonaNavigation)
                        .Include(e => e.IdUsuarioNavigation)
                        .FirstOrDefaultAsync(e => e.IdEnfermero == id);

                    if (enfermeroExistente == null)
                    {
                        return NotFound("Enfermero no encontrado.");
                    }

                    var personaExistente = enfermeroExistente.IdPersonaNavigation;
                    personaExistente.Nombre = request.Nombre;
                    personaExistente.PrimerApellido = request.PrimerApellido;
                    personaExistente.SegundoApellido = request.SegundoApellido;
                    personaExistente.Telefono = request.Telefono;
                    personaExistente.FechaNacimiento = request.FechaNacimiento;
                    personaExistente.Calle = request.Calle;
                    personaExistente.Numero = request.Numero;
                    personaExistente.CodigoPostal = request.CodigoPostal;
                    personaExistente.Colonia = request.Colonia;

                    enfermeroExistente.NumEnfermero = request.NumEnfermero;
                    enfermeroExistente.Titulo = request.Titulo;

                    var usuarioExistente = enfermeroExistente.IdUsuarioNavigation;

                    usuarioExistente.Usuario1 = request.Usuario1;
                    usuarioExistente.Contrasenia = request.Contrasenia;

                    await _baseDatos.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok(enfermeroExistente);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    return StatusCode(500, $"Error interno del servidor: {ex.Message}");
                }
            }
        }

        //METODO GET BUSCAR ENFERMERO POR NOMBRE
        [HttpGet]
        [Route("MisPacientes")]
        public async Task<IActionResult> MisPacientes(int id)
        {
            if (id == 0)
            {
                return BadRequest("ID de enfermero no proporcionado.");
            }

            var pacientes = from hi in _baseDatos.HistorialTurnos
                                   join et in _baseDatos.EnfermeroTurnos on hi.IdEnfermero equals et.IdEnfermero
                                   join pa in _baseDatos.Pacientes on hi.IdPaciente equals pa.IdPaciente
                                   join pe in _baseDatos.Personas on pa.IdPersona equals pe.IdPersona
                                   where et.IdEnfermero == id
                                   select new
                                   {
                                       IdHistorialTurno = hi.IdHistorialTurno,
                                       IdEnfermero = hi.IdEnfermero,
                                       IdPaciente = hi.IdPaciente,
                                       IdEnfermeroTurno = hi.IdEnfermeroTurno,
                                       EnfermeroId = et.IdEnfermero,
                                       Nombre = pe.Nombre,
                                       PrimerApellido = pe.PrimerApellido,
                                       SegundoApellido = pe.SegundoApellido
                                   };

            if (!pacientes.Any())
            {
                return NotFound("No se encontraron pacientes asignados.");
            }

            return Ok(pacientes);
        }




    }
}
