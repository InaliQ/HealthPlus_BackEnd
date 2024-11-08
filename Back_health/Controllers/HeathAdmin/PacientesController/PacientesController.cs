using Microsoft.AspNetCore.Mvc;
using Back_health.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;
using Azure.Core;

namespace Back_health.Controllers.HealthAdmin
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly HealtPlusContext _baseDatos;

        // Constructor
        public PacientesController(HealtPlusContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        // MÉTODO GET: BUSCAR PACIENTE POR NOMBRE
        [HttpGet]
        [Route("BuscarPorNombre")]
        public async Task<IActionResult> BuscarPorNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest("El nombre no puede estar vacío.");
            }

            var pacientes = await (from pa in _baseDatos.Pacientes
                                   join pe in _baseDatos.Personas on pa.IdPersona equals pe.IdPersona
                                   where pe.Nombre.Contains(nombre)
                                   select new
                                   {
                                       idPaciente = pa.IdPaciente,
                                       nombre = pe.Nombre,
                                       primerApellido = pe.PrimerApellido,
                                       segundoApellido = pe.SegundoApellido,
                                       telefono = pe.Telefono,
                                       fecha_nac = pe.FechaNacimiento,
                                       calle = pe.Calle,
                                       numero = pe.Numero,
                                       colonia = pe.Colonia,
                                       codigoPostal = pe.CodigoPostal,
                                       numPaciente = pa.NumPaciente,
                                       altura = pa.Altura,
                                       peso = pa.Peso,
                                       tipoSangre = pa.TipoSangre,
                                       estatus = pa.Estatus,
                                       padecimientos = (from pad in _baseDatos.PacientePadecimientos
                                                        join pade in _baseDatos.Padecimientos on pad.IdPadecimiento equals pade.IdPadecimiento
                                                        where pad.IdPaciente == pa.IdPaciente
                                                        select new
                                                        {
                                                            idPadecimiento = pade.IdPadecimiento,
                                                            nombrePadecimiento = pade.Nombre
                                                        }).ToList() // Agrupar padecimientos
                                   }).ToListAsync();

            if (!pacientes.Any())
            {
                return NotFound("No se encontraron pacientes con ese nombre.");
            }

            return Ok(pacientes);
        }

        //METODO GET BUSCAR POR ESTATUS
        [HttpGet]
        [Route("BuscarXEstatus")]
        public async Task<IActionResult> BuscarXEstatus(bool estatus)
        {
            var pacientes = await (from pa in _baseDatos.Pacientes
                                   join pe in _baseDatos.Personas on pa.IdPersona equals pe.IdPersona
                                   where pa.Estatus == estatus
                                   select new
                                   {
                                       idPaciente = pa.IdPaciente,
                                       nombre = pe.Nombre,
                                       primerApellido = pe.PrimerApellido,
                                       segundoApellido = pe.SegundoApellido,
                                       telefono = pe.Telefono,
                                       fecha_nac = pe.FechaNacimiento,
                                       calle = pe.Calle,
                                       numero = pe.Numero,
                                       colonia = pe.Colonia,
                                       codigoPostal = pe.CodigoPostal,
                                       numPaciente = pa.NumPaciente,
                                       altura = pa.Altura,
                                       peso = pa.Peso,
                                       tipoSangre = pa.TipoSangre,
                                       estatus = pa.Estatus,
                                       padecimientos = (from pad in _baseDatos.PacientePadecimientos
                                                        join pade in _baseDatos.Padecimientos on pad.IdPadecimiento equals pade.IdPadecimiento
                                                        where pad.IdPaciente == pa.IdPaciente
                                                        select new
                                                        {
                                                            idPadecimiento = pade.IdPadecimiento,
                                                            nombrePadecimiento = pade.Nombre
                                                        }).ToList() // Agrupar padecimientos
                                   }).ToListAsync();

            if (!pacientes.Any())
            {
                return NotFound("No hay");
            }

            return Ok(pacientes);
        }

        [HttpGet]
        [Route("ListarPacientes")]
        public async Task<IActionResult> ListarPacientes()
        {
            var pacientes = from pa in _baseDatos.Pacientes
                             join pe in _baseDatos.Personas on pa.IdPersona equals pe.IdPersona
                             join us in _baseDatos.Usuarios on pa.IdUsuario equals us.IdUsuario
                             select new
                             {
                                 idPaciente = pa.IdPaciente,
                                 nombre = pe.Nombre,
                                 primerApellido = pe.PrimerApellido,
                                 segundoApellido = pe.SegundoApellido,
                                 telefono = pe.Telefono,
                                 fechaNacimiento = pe.FechaNacimiento,
                                 calle = pe.Calle,
                                 numero = pe.Numero,
                                 colonia = pe.Colonia,
                                 codigoPostal = pe.CodigoPostal,
                                 numPaciente = pa.NumPaciente,
                                 altura = pa.Altura,
                                 peso = pa.Peso,
                                 tipoSangre = pa.TipoSangre,
                                 estatus = pa.Estatus,
                                 usuario1 = us.Usuario1,
                                 contrasenia = us.Contrasenia,

                                 padecimientos = (from pad in _baseDatos.PacientePadecimientos
                                                  join pade in _baseDatos.Padecimientos on pad.IdPadecimiento equals pade.IdPadecimiento
                                                  where pad.IdPaciente == pa.IdPaciente
                                                  select new
                                                  {
                                                      idPadecimiento = pade.IdPadecimiento,
                                                      nombrePadecimiento = pade.Nombre
                                                  }).ToList()

                             };

            if (!pacientes.Any())
            {
                return NotFound("No se encontraron pacientes");
            }

            return Ok(pacientes);
        }


        // METODO GET (OBTIENE PACIENTES DISPONIBLES)
        [HttpGet]
        [Route("ListarPacientesDisponibles/{idEnfermero}")]
        public async Task<IActionResult> ListarPacientesDisponibles(int idEnfermero)
        {
            var pacientes = from pa in _baseDatos.Pacientes
                            join pe in _baseDatos.Personas on pa.IdPersona equals pe.IdPersona
                            where !_baseDatos.HistorialTurnos
                                      .Any(ht => ht.IdPaciente == pa.IdPaciente
                                                 && ht.IdEnfermero == idEnfermero
                                                 && ht.Activo == true) // Solo pacientes sin asignación activa con este enfermero
                            select new
                            {
                                idPaciente = pa.IdPaciente,
                                nombre = pe.Nombre,
                                primerApellido = pe.PrimerApellido,
                                segundoApellido = pe.SegundoApellido,
                                telefono = pe.Telefono,
                                fecha_nac = pe.FechaNacimiento,
                                calle = pe.Calle,
                                numero = pe.Numero,
                                colonia = pe.Colonia,
                                codigoPostal = pe.CodigoPostal,
                                numPaciente = pa.NumPaciente,
                                altura = pa.Altura,
                                peso = pa.Peso,
                                tipoSangre = pa.TipoSangre,
                                estatus = pa.Estatus,
                                padecimientos = (from pad in _baseDatos.PacientePadecimientos
                                                 join pade in _baseDatos.Padecimientos on pad.IdPadecimiento equals pade.IdPadecimiento
                                                 where pad.IdPaciente == pa.IdPaciente
                                                 select new
                                                 {
                                                     idPadecimiento = pade.IdPadecimiento,
                                                     nombrePadecimiento = pade.Nombre
                                                 }).ToList()
                            };

            return Ok(pacientes);
        }


        // VERIFICAR SI EL PACIENTE YA ESTÁ ASIGNADO
        [HttpGet]
        [Route("yaAsignado/{idPaciente}")]
        public async Task<IActionResult> yaAsignado(int idPaciente)
        {
            var pacientes = from hi in _baseDatos.HistorialTurnos
                            join en in _baseDatos.Enfermeros on hi.IdEnfermero equals en.IdEnfermero
                            join pe in _baseDatos.Personas on en.IdPersona equals pe.IdPersona
                            where hi.IdPaciente == idPaciente && hi.Activo == true
                            select new
                            {
                                idHistorialTurno = hi.IdHistorialTurno,
                                idEnfermero = hi.IdEnfermero,
                                idPaciente = hi.IdPaciente,
                                idEnfermeroTurno = hi.IdEnfermeroTurno,
                                fechaRegistro = hi.FechaRegistro,
                                numEnfermero = en.NumEnfermero,
                                Nombre = pe.Nombre + " " + pe.PrimerApellido + " " + pe.SegundoApellido
                            };
            if (pacientes == null)
            {
                return NotFound("El paciente no está asignado a ningún enfermero actualmente.");
            }

            return Ok(pacientes);
        }


        // PUT: Modificar Paciente
        [HttpPut]
        [Route("ModificarPaciente/{id}")]
        public async Task<IActionResult> ModificarPaciente(int id, [FromBody] PacienteRequest request)
        {
            if (request == null)
            {
                return BadRequest("La solicitud es nula.");
            }

            await using var transaction = await _baseDatos.Database.BeginTransactionAsync();
            try
            {
                var pacienteExistente = await _baseDatos.Pacientes
                        .Include(e => e.IdPersonaNavigation)
                        .Include(e => e.IdUsuarioNavigation)
                        .FirstOrDefaultAsync(e => e.IdPaciente == id);

                if (pacienteExistente == null)
                {
                    return NotFound("Paciente no encontrado.");
                }

                // Actualizar los datos de la persona
                var personaExistente = pacienteExistente.IdPersonaNavigation;
                personaExistente.Nombre = request.Nombre;
                personaExistente.PrimerApellido = request.PrimerApellido;
                personaExistente.SegundoApellido = request.SegundoApellido;
                personaExistente.Telefono = request.Telefono;
                personaExistente.FechaNacimiento = request.FechaNacimiento;
                personaExistente.Calle = request.Calle;
                personaExistente.Numero = request.Numero;
                personaExistente.CodigoPostal = request.CodigoPostal;
                personaExistente.Colonia = request.Colonia;

                // Actualizar los datos del paciente
                pacienteExistente.NumPaciente = request.NumPaciente;
                pacienteExistente.Altura = request.Altura;
                pacienteExistente.Peso = request.Peso;
                pacienteExistente.TipoSangre = request.TipoSangre;
                pacienteExistente.Estatus = request.Estatus;

                // Actualizando los datos del usuario
                var usuarioExistente = pacienteExistente.IdUsuarioNavigation;
                usuarioExistente.Usuario1 = request.Usuario1;
                usuarioExistente.Contrasenia = request.Contrasenia;

                              
                // Actualizar los padecimientos del paciente
                var pacientesPadecimientosExistentes = await _baseDatos.PacientePadecimientos
                    .Where(pp => pp.IdPaciente == pacienteExistente.IdPaciente).ToListAsync();

                // Eliminar los padecimientos existentes
                _baseDatos.PacientePadecimientos.RemoveRange(pacientesPadecimientosExistentes);

                // Agregar nuevos padecimientos
                foreach (var idPadecimiento in request.IdPadecimientos)
                {
                    var pacientePadecimiento = new PacientePadecimiento
                    {
                        IdPaciente = pacienteExistente.IdPaciente,
                        IdPadecimiento = idPadecimiento
                    };

                    _baseDatos.PacientePadecimientos.Add(pacientePadecimiento);
                }

                // Guardar todos los cambios en la base de datos
                await _baseDatos.SaveChangesAsync();

                // Confirmar la transacción
                await transaction.CommitAsync();

                return Ok(pacienteExistente);
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


        // PUT MODIFICAR ESTATUS PACIENTE
        [HttpPut]
        [Route("ModificarEstatusPaciente/{id:int}")]
        public async Task<IActionResult> ModificarEstatusPaciente(int id, [FromBody] PacienteRequest request)
        {

            using (var transaction = await _baseDatos.Database.BeginTransactionAsync())
            {
                try
                {
                    var pacienteExistente = await _baseDatos.Pacientes
                        .FirstOrDefaultAsync(p => p.IdPaciente == id);

                    if (pacienteExistente == null)
                    {
                        return NotFound("Paciente no encontrado.");
                    }

                    pacienteExistente.Estatus = request.Estatus;

                    await _baseDatos.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok(pacienteExistente);
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

        // POST: Agregar Paciente
        [HttpPost]
        [Route("AgregarPacienteConRitmo")]
        public async Task<IActionResult> AgregarPacienteConRitmo([FromBody] PacienteRequest request)
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

                var ritmo = new Ritmo
                {
                    Max = request.Max,
                    Min = request.Min,
                    IdPaciente = paciente.IdPaciente,
                    IdPadecimiento = (int)request.idPadecimientoRitmo,
                    FechaRegistro = DateTime.Now,
                };

                _baseDatos.Ritmos.Add(ritmo);
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

    }
}
