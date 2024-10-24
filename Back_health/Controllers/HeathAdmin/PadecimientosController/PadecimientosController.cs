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
                                select new { idPadecimiento = p.IdPadecimiento, nombre = p.Nombre };

            return Ok(padecimientos);
        }

    }
}
