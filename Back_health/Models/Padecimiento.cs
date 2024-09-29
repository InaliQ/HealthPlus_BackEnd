using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class Padecimiento
{
    public int IdPadecimiento { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<MonitoreoSalud> MonitoreoSaluds { get; set; } = new List<MonitoreoSalud>();

    public virtual ICollection<PacientePadecimiento> PacientePadecimientos { get; set; } = new List<PacientePadecimiento>();

    public virtual ICollection<Ritmo> Ritmos { get; set; } = new List<Ritmo>();
}
