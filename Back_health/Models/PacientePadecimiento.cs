using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class PacientePadecimiento
{
    public int IdPacientePadecimiento { get; set; }

    public int IdPaciente { get; set; }

    public int IdPadecimiento { get; set; }

    public virtual Paciente IdPacienteNavigation { get; set; } = null!;

    public virtual Padecimiento IdPadecimientoNavigation { get; set; } = null!;
}
