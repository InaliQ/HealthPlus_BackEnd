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
                             join et in _baseDatos.EnfermeroTurnos on en.IdEnfermero equals et.IdEnfermero
                             join us in _baseDatos.Usuarios on en.IdUsuario equals us.IdUsuario
                             select new
                             {
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
                                 numEnfermero = en.NumEnfermero,
                                 idEnfermeroTurno = et.IdEnfermeroTurno,
                                 turno = et.Turno,
                                 usuario1 = us.Usuario1,
                                 contrasenia = us.Contrasenia
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
                        Turno = request.Turno,
                        FechaCambio = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
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

                    // Actualizando los datos de la persona
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

                    // Actualizando los datos del enfermero
                    enfermeroExistente.NumEnfermero = request.NumEnfermero;
                    enfermeroExistente.Titulo = request.Titulo;

                    // Actualizando los datos del usuario
                    var usuarioExistente = enfermeroExistente.IdUsuarioNavigation;
                    usuarioExistente.Usuario1 = request.Usuario1;
                    usuarioExistente.Contrasenia = request.Contrasenia;

                    // Guardamos todos los cambios
                    await _baseDatos.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok(enfermeroExistente);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(409, $"Conflicto de actualización: {ex.Message}");
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






        [HttpGet]
        [Route("MisPacientes")]
        public async Task<IActionResult> MisPacientes(int id)
        {
            if (id == 0)
            {
                return BadRequest("ID de enfermero no proporcionado.");
            }

            var pacientes = from hi in _baseDatos.HistorialTurnos
                            join et in _baseDatos.Enfermeros on hi.IdEnfermero equals et.IdEnfermero
                            join pa in _baseDatos.Pacientes on hi.IdPaciente equals pa.IdPaciente
                            join pe in _baseDatos.Personas on pa.IdPersona equals pe.IdPersona
                            where et.IdEnfermero == id && hi.Activo == true
                            select new
                            {
                                IdHistorialTurno = hi.IdHistorialTurno,
                                IdEnfermero = hi.IdEnfermero,
                                IdPaciente = hi.IdPaciente,
                                IdEnfermeroTurno = hi.IdEnfermeroTurno,
                                EnfermeroId = et.IdEnfermero,
                                Nombre = pe.Nombre,
                                PrimerApellido = pe.PrimerApellido,
                                SegundoApellido = pe.SegundoApellido,
                                tipoSangre = pa.TipoSangre,
                                peso = pa.Peso,
                                altura = pa.Altura,
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
                return NotFound("No se encontraron pacientes asignados.");
            }

            return Ok(pacientes);
        }


        // ASIGNAR PACIENTES A ENFERMEROS
        [HttpPost]
        [Route("AsignarPaciente")]
        public async Task<IActionResult> AsignarPaciente([FromBody] EnfermeroPacienteRequest request)
        {
            if (request == null || request.IdEnfermero == 0 || request.IdPaciente == 0 || request.IdEnfermeroTurno == 0)
            {
                return BadRequest("Datos inválidos");
            }

            using (var transaction = await _baseDatos.Database.BeginTransactionAsync())
            {
                try
                {
                    // Verifica que el enfermero, paciente y turno existen
                    var enfermeroExiste = await _baseDatos.Enfermeros.AnyAsync(e => e.IdEnfermero == request.IdEnfermero);
                    var pacienteExiste = await _baseDatos.Pacientes.AnyAsync(p => p.IdPaciente == request.IdPaciente);
                    var turnoExiste = await _baseDatos.EnfermeroTurnos.AnyAsync(et => et.IdEnfermeroTurno == request.IdEnfermeroTurno);

                    if (!enfermeroExiste || !pacienteExiste || !turnoExiste)
                    {
                        return NotFound("Enfermero, paciente o turno no encontrado");
                    }

                    // Busca cualquier asignación activa existente para el paciente y la desactiva
                    var asignacionAnterior = await _baseDatos.HistorialTurnos
                        .FirstOrDefaultAsync(ht => ht.IdPaciente == request.IdPaciente && ht.Activo == true);

                    if (asignacionAnterior != null)
                    {
                        asignacionAnterior.Activo = false;
                        _baseDatos.HistorialTurnos.Update(asignacionAnterior);
                        await _baseDatos.SaveChangesAsync();
                    }

                    // Crea la nueva asignación para el enfermero actual
                    var nuevaAsignacion = new HistorialTurno
                    {
                        IdEnfermero = request.IdEnfermero,
                        IdPaciente = request.IdPaciente,
                        IdEnfermeroTurno = request.IdEnfermeroTurno,
                        FechaRegistro = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        Activo = true
                    };

                    _baseDatos.HistorialTurnos.Add(nuevaAsignacion);
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

        //HISTORIAL DE TURNOS
        [HttpGet("ObtenerTurnosEnfermero")]
        public async Task<IActionResult> ObtenerTurnosEnfermero(int idEnfermero)
        {
            var turnosEnfermero = from et in _baseDatos.EnfermeroTurnos
                                  join e in _baseDatos.Enfermeros on et.IdEnfermero equals e.IdEnfermero
                                  where et.IdEnfermero == idEnfermero
                                  orderby et.IdEnfermeroTurno descending
                                  select new {
                                      idEnfermero = et.IdEnfermero,
                                      numEnfermero = e.NumEnfermero,
                                      turno = et.Turno,
                                      fechaCambio = et.FechaCambio
                                  };

            if (turnosEnfermero == null || !turnosEnfermero.Any())
            {
                return NotFound("No se encontraron turnos para el enfermero especificado.");
            }

            return Ok(turnosEnfermero);
        }


        //POST CAMBIAR DE TURNO ENFERMERO
        [HttpPost]
        [Route("CambiarTurnoEnfermero")]
        public async Task<IActionResult> CambiarTurnoEnfermero([FromBody] CambiarTurnoRequest request)
        {
            if (request == null || request.IdEnfermero == 0 || string.IsNullOrEmpty(request.NuevoTurno))
            {
                return BadRequest("Datos inválidos");
            }

            using (var transaction = await _baseDatos.Database.BeginTransactionAsync())
            {
                try
                {
                    var enfermero = await _baseDatos.Enfermeros.FindAsync(request.IdEnfermero);
                    if (enfermero == null)
                    {
                        return NotFound("Enfermero no encontrado");
                    }

                    // Registrar el cambio de turno
                    var nuevoTurno = new EnfermeroTurno
                    {
                        IdEnfermero = enfermero.IdEnfermero,
                        Turno = request.NuevoTurno,
                        FechaCambio = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    };

                    await _baseDatos.EnfermeroTurnos.AddAsync(nuevoTurno);
                    await _baseDatos.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return Ok("Turno del enfermero actualizado correctamente");
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



    }
}
