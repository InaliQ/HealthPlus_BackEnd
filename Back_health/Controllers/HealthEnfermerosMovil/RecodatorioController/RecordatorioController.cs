using Back_health.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Back_health.Controllers.HealthEnfermerosMovil.RecordatorioController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordatorioController : ControllerBase
    {
        private readonly HealtPlusContext _baseDatos;

        public RecordatorioController(HealtPlusContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        // Método para agregar un nuevo recordatorio
        [HttpPost("agregar")]
        public async Task<IActionResult> AgregarRecordatorio([FromBody] RecordatorioRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            // Crear un nuevo registro de recordatorio basado en la solicitud
            var nuevoRecordatorio = new Recordatorio
            {
                Medicamento = request.Medicamento,
                CantidadMedicamento = request.CantidadMedicamento,
                FechaInicio = request.FechaInicio,
                Estatus = request.Estatus,
                IdEnfermero = request.IdEnfermero,
                IdPaciente = request.IdPaciente
            };

            await _baseDatos.Set<Recordatorio>().AddAsync(nuevoRecordatorio);

            var resultado = await _baseDatos.SaveChangesAsync();

            if (resultado > 0)
            {
                return Ok(new { mensaje = "Recordatorio agregado exitosamente." });
            }

            return BadRequest(new { mensaje = "Error al guardar el recordatorio." });
        }

        // Método para eliminar un recordatorio por su ID
        [HttpDelete("EliminarRecordatorio/{id}")]
        public async Task<IActionResult> EliminarRecordatorio(int id)
        {
            var recordatorio = await _baseDatos.Set<Recordatorio>().FindAsync(id);

            if (recordatorio == null)
            {
                return NotFound(new { mensaje = "No se encontró el recordatorio a eliminar." });
            }

            _baseDatos.Set<Recordatorio>().Remove(recordatorio);

            var resultado = await _baseDatos.SaveChangesAsync();

            if (resultado > 0)
            {
                return Ok(new { mensaje = "Recordatorio eliminado exitosamente." });
            }

            return BadRequest(new { mensaje = "Error al eliminar el recordatorio." });
        }
    }

    // Clase para recibir la solicitud de nuevo recordatorio
    
}
