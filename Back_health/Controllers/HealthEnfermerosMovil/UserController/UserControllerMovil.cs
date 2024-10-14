using Back_health.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Back_health.Controllers.HealthEnfermerosMovil.UserController
{
    [ApiController]
    [Route("[controller]")]
    public class UserControllerMovil : ControllerBase
    {
        private readonly HealtPlusContext _baseDatos;

        public UserControllerMovil(HealtPlusContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        // POST: Agregar un recordatorio para un paciente
        [HttpPost]
        [Route("AgregarRecordatorio")]
        public async Task<IActionResult> AgregarRecordatorio(int idPaciente, [FromBody] RecordatorioRequest request)
        {
            if (request == null)
            {
                return BadRequest("La solicitud es nula.");
            }

            // Crear un nuevo recordatorio
            var nuevoRecordatorio = new Recordatorio
            {
                Medicamento = request.Medicamento,
                CantidadMedicamento = request.CantidadMedicamento,
                FechaInicio = DateTime.Now,
                FechaFin = null,
                Estatus = true,
                IdEnfermero = request.IdEnfermero,
                IdPaciente = idPaciente
            };

            _baseDatos.Recordatorios.Add(nuevoRecordatorio);
            await _baseDatos.SaveChangesAsync();

            return Ok("Recordatorio agregado exitosamente.");
        }

        // DELETE: Eliminar un recordatorio
        [HttpDelete]
        [Route("EliminarRecordatorio/{idRecordatorio}")]
        public async Task<IActionResult> EliminarRecordatorio(int idRecordatorio)
        {
            var recordatorio = await _baseDatos.Recordatorios.FindAsync(idRecordatorio);

            if (recordatorio == null)
            {
                return NotFound("Recordatorio no encontrado.");
            }

            _baseDatos.Recordatorios.Remove(recordatorio);
            await _baseDatos.SaveChangesAsync();

            return Ok("Recordatorio eliminado exitosamente.");
        }
    }
}
