using Back_health.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Back_health.Controllers.HealthEnfermerosMovil.PadecimietosController
{
    [ApiController]
    [Route("[controller]")]
    public class PadeciminetosControllerMovil : ControllerBase
    {
        private readonly HealtPlusContext _baseDatos;

        public PadeciminetosControllerMovil(HealtPlusContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        // POST: Agregar un nuevo registro de ritmo para un paciente si tiene el padecimiento con ID = 1
        [HttpPost]
        [Route("AgregarRitmo")]
        public async Task<IActionResult> AgregarRitmo(int idPaciente, [FromBody] RitmoRequest request)
        {
            // Verificar si el paciente tiene el padecimiento con ID = 1
            var tienePadecimiento = await _baseDatos.PacientePadecimientos
                .AnyAsync(pp => pp.IdPaciente == idPaciente && pp.IdPadecimiento == 1);

            if (!tienePadecimiento)
            {
                return BadRequest("El paciente no tiene el padecimiento requerido.");
            }

            // Crear el nuevo registro de ritmo
            var nuevoRitmo = new Ritmo
            {
                Max = request.Max,
                Min = request.Min,
                Medicion = request.Medicion,
                IdPadecimiento = 1,
                IdPaciente = idPaciente
            };

            _baseDatos.Ritmos.Add(nuevoRitmo);
            await _baseDatos.SaveChangesAsync();

            return Ok("Ritmo agregado exitosamente.");
        }
    }
}
