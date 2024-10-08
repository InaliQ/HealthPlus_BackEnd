using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class HistorialTurno
{
    public int IdTurno { get; set; }

    public int IdEnfermero { get; set; }

    public int IdPaciente { get; set; }

    public string? Turno { get; set; }

    public virtual ICollection<EnfermeroTurno> EnfermeroTurnos { get; set; } = new List<EnfermeroTurno>();

    public virtual Enfermero IdEnfermeroNavigation { get; set; } = null!;

    public virtual Paciente IdPacienteNavigation { get; set; } = null!;
}
