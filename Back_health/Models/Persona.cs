using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class Persona
{
    public int IdPersona { get; set; }

    public string? Nombre { get; set; }

    public string? PrimerApellido { get; set; }

    public string? SegundoApellido { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string? Telefono { get; set; }

    public string? Calle { get; set; }

    public string? Numero { get; set; }

    public string? CodigoPostal { get; set; }

    public string? Colonia { get; set; }

    public virtual ICollection<Enfermero> Enfermeros { get; set; } = new List<Enfermero>();

    public virtual ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
}
