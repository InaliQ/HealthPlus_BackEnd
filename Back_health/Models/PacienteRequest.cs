namespace Back_health.Models
{
    public class PacienteRequest
    {
        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Telefono { get; set; }
        public string? FechaNacimiento { get; set; }
        public string? Calle { get; set; }
        public string? Numero { get; set; }
        public string? CodigoPostal { get; set; }
        public string? Colonia { get; set; }
        public string? NumPaciente { get; set; }
        public string? Altura { get; set; }
        public string? Peso { get; set; }
        public string? TipoSangre { get; set; }
        public bool Estatus { get; set; }
        public string? Usuario1 { get; set; }
        public string? Contrasenia { get; set; }
        public int IdPaciente { get; set; }
        public int IdPadecimiento { get; set; }
    }
}
