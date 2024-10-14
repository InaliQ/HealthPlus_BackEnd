using Back_health.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back_health.Controllers.HealthUserMovil.RecordatoriosController
{
    [ApiController]
    [Route("[controller]")]
    public class RecordatorioController : Controller
    {
        private readonly HealtPlusContext _baseDatos;

        public RecordatorioController(HealtPlusContext baseDatos)
        {
            _baseDatos = baseDatos;
        }
        // PUT: Actualizar el estado de un recordatorio
        [HttpPut]
        [Route("CompletarRecordatorio/{idRecordatorio}")]
        public async Task<IActionResult> CompletarRecordatorio(int idRecordatorio)
        {
            var recordatorio = await _baseDatos.Recordatorios.FindAsync(idRecordatorio);

            if (recordatorio == null)
            {
                return NotFound("Recordatorio no encontrado.");
            }

            // Actualizar la fecha_fin y el estatus
            recordatorio.FechaFin = DateTime.Now;
            recordatorio.Estatus = true;

            await _baseDatos.SaveChangesAsync();

            return Ok("Recordatorio actualizado exitosamente.");
        }
    }
}
