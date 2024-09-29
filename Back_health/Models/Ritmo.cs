using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class Ritmo
{
    public int IdRitmo { get; set; }

    public int? Max { get; set; }

    public int? Min { get; set; }

    public int? Medicion { get; set; }

    public int IdPadecimiento { get; set; }

    public int IdPaciente { get; set; }

    public virtual Paciente IdPacienteNavigation { get; set; } = null!;

    public virtual Padecimiento IdPadecimientoNavigation { get; set; } = null!;
}
