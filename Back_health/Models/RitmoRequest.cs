namespace Back_health.Models
{
    public class RitmoRequest
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public int Medicion { get; set; }
        public int IdPaciente { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }

}
