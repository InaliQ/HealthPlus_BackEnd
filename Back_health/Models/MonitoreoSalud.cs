using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class MonitoreoSalud
{
    public int IdMonitoreo { get; set; }

    public DateTime? FechaHora { get; set; }

    public string? Tipo { get; set; }

    public int IdPadecimiento { get; set; }

    public virtual Padecimiento IdPadecimientoNavigation { get; set; } = null!;
}
