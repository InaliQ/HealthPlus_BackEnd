using Back_health.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back_health.Controllers.HealthUserMovil.AlertaController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertaController : ControllerBase
    {
        private readonly HealtPlusContext _baseDatos;

        public AlertaController(HealtPlusContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        // Acción para crear una nueva alerta
        [HttpPost("agregar")]
        public async Task<IActionResult> CrearAlerta([FromBody] AlertaRequest alertaRequest)
        {
            if (alertaRequest == null)
            {
                return BadRequest("La alerta es inválida.");
            }

            try
            {
                // Creamos una nueva instancia de alerta usando los datos del request
                var nuevaAlerta = new Alertum
                {
                    FechaHora = DateTime.Now,
                    Descripcion = alertaRequest.Descripcion,
                    IdPaciente = alertaRequest.IdPaciente
                };

                // Guardamos la nueva alerta en la base de datos
                _baseDatos.Alerta.Add(nuevaAlerta);
                await _baseDatos.SaveChangesAsync();

                // Retornamos una respuesta exitosa con los detalles de la alerta creada
                return CreatedAtAction(nameof(CrearAlerta), new { id = nuevaAlerta.IdAlerta }, nuevaAlerta);
            }
            catch (Exception ex)
            {
                // Si ocurre algún error, respondemos con un error interno del servidor
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }

    public class AlertaRequest
    {
        public string Descripcion { get; set; }
        public int? IdPaciente { get; set; }
    }

}
