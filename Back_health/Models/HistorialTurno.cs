using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class HistorialTurno
{
    public int IdHistorialTurno { get; set; }

    public int IdEnfermero { get; set; }

    public int IdPaciente { get; set; }

    public int IdEnfermeroTurno { get; set; }

    public string? FechaRegistro { get; set; }

    public bool? Activo { get; set; }

    public virtual Enfermero IdEnfermeroNavigation { get; set; } = null!;

    public virtual Paciente IdPacienteNavigation { get; set; } = null!;
}
