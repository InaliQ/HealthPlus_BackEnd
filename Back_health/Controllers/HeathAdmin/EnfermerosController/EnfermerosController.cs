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
            var enfermeros = await (from en in _baseDatos.Enfermeros
                                    join pe in _baseDatos.Personas on en.IdPersona equals pe.IdPersona
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
                                        numEnfermero = en.NumEnfermero
                                    }).ToListAsync(); 

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

    }
}
