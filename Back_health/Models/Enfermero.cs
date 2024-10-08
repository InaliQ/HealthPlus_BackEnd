using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class Enfermero
{
    public int IdEnfermero { get; set; }

    public string? Titulo { get; set; }

    public string? NumEnfermero { get; set; }

    public int IdPersona { get; set; }

    public int IdUsuario { get; set; }

    public virtual ICollection<EnfermeroTurno> EnfermeroTurnos { get; set; } = new List<EnfermeroTurno>();

    public virtual ICollection<HistorialTurno> HistorialTurnos { get; set; } = new List<HistorialTurno>();

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Recordatorio> Recordatorios { get; set; } = new List<Recordatorio>();
}
