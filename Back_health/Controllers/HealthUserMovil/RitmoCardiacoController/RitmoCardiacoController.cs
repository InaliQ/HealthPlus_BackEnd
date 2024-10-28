using Back_health.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_health.Controllers.HealthUserMovil.RitmoCardiacoController
{
    [ApiController]
    [Route("api/[controller]")]
    public class RitmoCardiacoController : ControllerBase
    {
        private readonly HealtPlusContext _baseDatos;

        public RitmoCardiacoController(HealtPlusContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpGet("ObtenerMaxMinRitmo/{idPaciente}")]
        public async Task<IActionResult> ObtenerMaxMinRitmo(int idPaciente)
        {
            try
            {
                // Consulta el máximo y mínimo ritmo del paciente especificado
                var ritmoPaciente = await _baseDatos.Ritmos
                    .Where(r => r.IdPaciente == idPaciente)
                    .Select(r => new { Maximo = r.Max, Minimo = r.Min })
                    .FirstOrDefaultAsync();

                if (ritmoPaciente == null)
                {
                    return NotFound(new { mensaje = "No se encontraron datos de ritmo para el paciente especificado." });
                }

                return Ok(ritmoPaciente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener los datos de ritmo", error = ex.Message });
            }
        }

        [HttpPost("GuardarRitmoCardiaco")]
        public async Task<IActionResult> GuardarRitmoCardiaco([FromBody] RitmoRequest ritmoRequest)
        {
            try
            {
                var ultimoRitmoPaciente = await _baseDatos.Ritmos
                    .Where(r => r.IdPaciente == ritmoRequest.idPaciente)
                    .OrderByDescending(r => r.IdRitmo)
                    .FirstOrDefaultAsync();

                int maxRitmo = ultimoRitmoPaciente?.Max ?? 90;
                int minRitmo = ultimoRitmoPaciente?.Min ?? 40;
                int idPadecimiento = ultimoRitmoPaciente?.IdPadecimiento ?? 1;

                // Crear el nuevo registro de ritmo cardíaco
                var nuevoRitmoCardiaco = new Ritmo
                {
                    IdPaciente = ritmoRequest.idPaciente,
                    Medicion = ritmoRequest.Medicion,
                    Max = maxRitmo,
                    Min = minRitmo,
                    IdPadecimiento = idPadecimiento
                };

                _baseDatos.Ritmos.Add(nuevoRitmoCardiaco);
                await _baseDatos.SaveChangesAsync();

                return Ok(new { mensaje = "Ritmo cardíaco guardado correctamente", ritmoCardiaco = nuevoRitmoCardiaco });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new { mensaje = "Error al guardar el ritmo cardíaco", error = innerException });
            }
        }


        public class RitmoRequestt
        {
            public int idPaciente { get; set; }
            public int Medicion { get; set; }
        }

    }
}
