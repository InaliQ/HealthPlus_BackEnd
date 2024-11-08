using Back_health.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back_health.Controllers.HeathAdmin.PadecimientosController
{

    [Route("api/[controller]")]
    [ApiController]
    public class PadecimientosController : Controller
    {

       private readonly HealtPlusContext _baseDatos;

        // Constructor
        public PadecimientosController(HealtPlusContext baseDatos) { _baseDatos = baseDatos; }

        //METODO GET (OBTIENE PADECIMIENTOS)
        [HttpGet]
        [Route("ListarPadecimientos")]
        public async Task<IActionResult> ListarPadecimientos()
        {
            var padecimientos = from p in _baseDatos.Padecimientos
                                select new { idPadecimiento = p.IdPadecimiento, nombrePadecimiento = p.Nombre };

            return Ok(padecimientos.ToList());
        }

        //POST AGREGAR PADECIMIENTOS
        [HttpPost]
        [Route("AgregarPadecimientos")]
        public async Task<IActionResult> AgregarPadecimientos([FromBody] PadecimientoRequest request)
        {
            var padecimiento = new Padecimiento
            {
                Nombre = request.nombrePadecimiento
            };

            _baseDatos.Padecimientos.Add(padecimiento);
            await _baseDatos.SaveChangesAsync();

            return Ok(padecimiento);

        }

        //PUT MODIFICAR PADECIMIENTOS
        [HttpPut]
        [Route("ModificarPadecimiento/{id:int}")]
        public async Task<IActionResult> ModificarPadecimiento(int id, [FromBody] PadecimientoRequest request)
        {
            var padecimientoExistente = await _baseDatos.Padecimientos.FindAsync(id);
            if (padecimientoExistente == null)
            {
                return NotFound("Padecimiento no encontrado");
            }

            padecimientoExistente.Nombre = request.nombrePadecimiento;

            await _baseDatos.SaveChangesAsync();

            return Ok(padecimientoExistente);
        }


    }
}
