using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class Doctor
{
    public int IdDoctor { get; set; }

    public string? Cedula { get; set; }

    public string? NumDoctor { get; set; }

    public int IdUsuario { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
