using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Usuario1 { get; set; }

    public string? Contrasenia { get; set; }

    public bool? Estatus { get; set; }

    public string? Rol { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual ICollection<Enfermero> Enfermeros { get; set; } = new List<Enfermero>();

    public virtual ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
}
