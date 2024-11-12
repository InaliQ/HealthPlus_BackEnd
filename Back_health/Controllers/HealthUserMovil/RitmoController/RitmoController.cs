using Back_health.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Back_health.Controllers.HealthUserMovil.RitmoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RitmoController : ControllerBase
    {
        private readonly HealtPlusContext _baseDatos;

        public DateTime FechaRegistro { get; private set; }

        public RitmoController(HealtPlusContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpPost("guardar")]
        public async Task<IActionResult> GuardarMedicion([FromBody] RitmoRequest request)
        {
            // Validar que el request no sea nulo
            if (request == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            // Consultar la tabla de ritmo para obtener los valores de max, min e idPadecimiento del paciente
            var ritmoExistente = await _baseDatos.Set<Ritmo>()
                .FirstOrDefaultAsync(r => r.IdPaciente == request.IdPaciente);

            int max, min, idPadecimiento;

            if (ritmoExistente != null)
            {
                // Si existe un registro, usar sus valores de max, min y idPadecimiento
                max = (int)ritmoExistente.Max;
                min = (int)ritmoExistente.Min;
                idPadecimiento = ritmoExistente.IdPadecimiento;
                FechaRegistro = DateTime.Now;
            }
            else
            {
                // Si no existe un registro, definir valores por defecto
                max = 90; // Puedes ajustar este valor según sea necesario
                min = 40; // Puedes ajustar este valor según sea necesario
                idPadecimiento = 1; // Ajusta este valor según sea necesario
            }

            // Crear un nuevo registro con los valores obtenidos
            var nuevoRitmo = new Ritmo
            {
                Max = max,
                Min = min,
                Medicion = request.Medicion, // Usar el valor de Medicion del request
                IdPadecimiento = idPadecimiento,
                IdPaciente = request.IdPaciente, // Usar el IdPaciente del request
                FechaRegistro = request.FechaRegistro
            };

            await _baseDatos.Set<Ritmo>().AddAsync(nuevoRitmo);

            // Guardar los cambios en la base de datos
            var resultado = await _baseDatos.SaveChangesAsync();

            if (resultado > 0)
            {
                return Ok(new { mensaje = "Medición guardada exitosamente." });
            }

            return BadRequest(new { mensaje = "Error al guardar la medición." });
        }

        [HttpGet("ObtenerMaxMinRitmo/{idPaciente}")]
        public async Task<IActionResult> ObtenerMaxMinRitmo(int idPaciente)
        {
            var ritmo = await _baseDatos.Set<Ritmo>()
                                        .FirstOrDefaultAsync(r => r.IdPaciente == idPaciente);

            if (ritmo == null)
            {
                return NotFound(new { mensaje = "No se encontró ritmo para el paciente." });
            }

            var response = new RitmoMaxMinResponse
            {
                Max = (int)ritmo.Max,
                Min = (int)ritmo.Min,
                Medicion = (int)ritmo.Medicion,
                IdPadecimiento = ritmo.IdPadecimiento
            };

            return Ok(response);
        }


        [HttpGet("ObtenerRitmos/{idPaciente}")]
        public async Task<IActionResult> ObtenerRitmos(int idPaciente)
        {
            // Obtener todos los registros de ritmo para el paciente con el ID especificado
            var ritmos = await _baseDatos.Set<Ritmo>()
                                          .Where(r => r.IdPaciente == idPaciente)
                                          .ToListAsync();

            // Verificar si existen registros de ritmo para el paciente
            if (ritmos == null || !ritmos.Any())
            {
                return NotFound(new { mensaje = "No se encontraron registros de ritmo para el paciente." });
            }

            // Retornar la lista de registros de ritmo
            return Ok(ritmos);
        }

        [HttpDelete("EliminarRitmo/{id}")]
        public async Task<IActionResult> EliminarRitmo(int id)
        {
            // Buscar el registro de ritmo por su ID
            var ritmo = await _baseDatos.Set<Ritmo>().FindAsync(id);

            // Verificar si el registro existe
            if (ritmo == null)
            {
                return NotFound(new { mensaje = "No se encontró el registro de ritmo a eliminar." });
            }

            // Eliminar el registro
            _baseDatos.Set<Ritmo>().Remove(ritmo);

            // Guardar los cambios en la base de datos
            var resultado = await _baseDatos.SaveChangesAsync();

            if (resultado > 0)
            {
                return Ok(new { mensaje = "Registro de ritmo eliminado exitosamente." });
            }

            return BadRequest(new { mensaje = "Error al eliminar el registro de ritmo." });
        }


    }







    public class RitmoMaxMinResponse
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public int Medicion { get; set; }
        public int IdPadecimiento { get; set; }
    }

}
