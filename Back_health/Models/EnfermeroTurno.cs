using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class EnfermeroTurno
{
    public int IdEnfermeroTurno { get; set; }

    public int IdEnfermero { get; set; }

    public string Turno { get; set; } = null!;

    public string? FechaCambio { get; set; }

    public virtual Enfermero IdEnfermeroNavigation { get; set; } = null!;
}
