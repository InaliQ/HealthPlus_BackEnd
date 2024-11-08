using Back_health.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Back_health.Controllers.HeathAdmin.DashboardsController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly HealtPlusContext _baseDatos;

        public DashboardController(HealtPlusContext baseDatos) {this._baseDatos = baseDatos;}

        /*//RITMO CARDIACO
        [HttpGet]
        [Route("RitmoDia/{idPaciente}/{fecha}")]
        public async Task<IActionResult> RitmoDia(int idPaciente, DateTime fecha)
        {
            var resultados = await _baseDatos.MonitoreoSaluds
                .Where(mo => mo.IdPaciente == idPaciente && EF.Functions.DateDiffDay(mo.FechaHora, fecha) == 0)
                .Select(mo => new
                {
                    mo.RitmoCardiaco,
                    mo.FechaHora
                })
                .ToListAsync(); // Ejecuta la consulta

            return Ok(resultados);
        }*/


        // GET NUM PACIENTES POR PADECIMIENTO
        [HttpGet]
        [Route("PacientesXPadecimiento")]
        public async Task<IActionResult> PacientesXPadecimiento()
        {
            var pacientes = from pa in _baseDatos.PacientePadecimientos
                            group pa by pa.IdPadecimiento into g
                            select new
                            {
                                PadecimientoId = g.Key,
                                PadecimientoNombre = (from p in _baseDatos.Padecimientos
                                                      where p.IdPadecimiento == g.Key
                                                      select p.Nombre).FirstOrDefault(),
                                CantidadPacientes = g.Count()
                            };

            return Ok(pacientes);

        }

        // GET PACIENTES POR EDAD
        [HttpGet]
        [Route("PacientesPorEdad")]
        public async Task<IActionResult> PacientesPorEdad()
        {
            var pacientes = from pa in _baseDatos.Pacientes
                            join pe in _baseDatos.Personas on pa.IdPersona equals pe.IdPersona
                            select new { fechaNacimiento = pe.FechaNacimiento };

            var pacientesList = await pacientes.ToListAsync();
            var pacientesPorEdad = pacientesList
                .GroupBy(p => CalcularEdad(p.fechaNacimiento))
                .Select(g => new
                {
                    Edad = g.Key,
                    CantidadPacientes = g.Count()
                })
                .ToList(); 

            return Ok(pacientesPorEdad);
        }


        // Método para calcular la edad
        private int CalcularEdad(string fechaNacimiento)
        {
            if (DateTime.TryParse(fechaNacimiento, out DateTime fechaNac))
            {
                var hoy = DateTime.Today;
                int edad = hoy.Year - fechaNac.Year;

                if (fechaNac.Date > hoy.AddYears(-edad)) edad--;

                return edad;
            }
            else
            {
                throw new ArgumentException("Fecha de nacimiento no es válida");
            }
        }

        // METODO GET (OBTIENE DASHBOARD DE RITMO CARDÍACO)
        [HttpGet]
        [Route("DashboardRitmo/{idPaciente}/{fecha}")]
        public async Task<IActionResult> DashboardRitmo(int idPaciente, DateTime fecha)
        {
            var ritmoDatos = await (from r in _baseDatos.Ritmos
                                    where r.IdPaciente == idPaciente
                                          && EF.Functions.DateDiffDay(r.FechaRegistro, fecha) == 0 // Compara las fechas
                                    orderby r.FechaRegistro descending // Ordena por fecha de registro
                                    select new
                                    {
                                        idRitmo = r.IdRitmo,
                                        max = r.Max,
                                        min = r.Min,
                                        medicion = r.Medicion,
                                        idPadecimiento = r.IdPadecimiento,
                                        fechaRegistro = r.FechaRegistro
                                    }).ToListAsync();

            return Ok(ritmoDatos);
        }


    }
}
