using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class Alertum
{
    public int IdAlerta { get; set; }

    public DateTime? FechaHora { get; set; }

    public string? Descripcion { get; set; }

    public int? IdPaciente { get; set; }

    public virtual ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
}
