using Back_health.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back_health.Controllers.HeathAdmin.CheckController
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : Controller
    {
        
        private readonly HealtPlusContext _baseDatos;

            public HealthController(HealtPlusContext context)
            {
                _baseDatos = context;
            }

            // Endpoint GET /api/healthcheck
            [HttpGet("healthcheck")]
            public async Task<IActionResult> HealthCheck()
            {
                // Verifica la disponibilidad del sistema en general
                var dbAvailable = await _baseDatos.Database.CanConnectAsync();

                if (dbAvailable)
                {
                    return Ok(new { status = "Bien", database = "Conectada" });
                }
                else
                {
                    return StatusCode(500, new { status = "Mal", database = "Not Conectada" });
                }
            }
        }
    }