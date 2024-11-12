using Microsoft.AspNetCore.Mvc;

namespace Back_health.Models
{
    public class RecordatorioRequest
    {
        public string Medicamento { get; set; }
        public string CantidadMedicamento { get; set; }
        public DateTime ?FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool Estatus { get; set; }
        public int IdEnfermero { get; set; }
        public int IdPaciente { get; set; }
    }
}
