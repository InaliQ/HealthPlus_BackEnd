using Back_health.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back_health.Controllers.HealthUserMovil.AlertaController
{
    [ApiController]
    [Route("[controller]")]
    public class AlertaController : ControllerBase
    {
        private readonly HealtPlusContext _baseDatos;

        public AlertaController(HealtPlusContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpPost("AgregarAlerta")]
        public async Task<IActionResult> AgregarAlerta([FromBody] alertaRequets request)
        {
            try
            {
                var nuevaAlerta = new Alertum
                {
                    IdPaciente = request.IdPaciente,
                    FechaHora = request.FechaHora,
                    Descripcion = request.Descripcion,
                };

                _baseDatos.Alerta.Add(nuevaAlerta);
                await _baseDatos.SaveChangesAsync();

                return Ok(new { mensaje = "Alerta agregada correctamente", alerta = nuevaAlerta });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al agregar la alerta", error = ex.Message });
            }
        }
    }
    public class alertaRequets
    {
        public int IdPaciente { get; set; }
        public DateTime FechaHora { get; set; }
        public string Descripcion { get; set; }
    }
}
