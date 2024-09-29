using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class EnfermeroTurno
{
    public int IdEnfermeroTurno { get; set; }

    public int IdEnfermero { get; set; }

    public int IdTurno { get; set; }

    public virtual Enfermero IdEnfermeroNavigation { get; set; } = null!;

    public virtual HistorialTurno IdTurnoNavigation { get; set; } = null!;
}
