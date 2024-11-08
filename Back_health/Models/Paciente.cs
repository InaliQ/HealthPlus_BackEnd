using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class Paciente
{
    public int IdPaciente { get; set; }

    public string? NumPaciente { get; set; }

    public string? Altura { get; set; }

    public string? Peso { get; set; }

    public string? TipoSangre { get; set; }

    public bool? Estatus { get; set; }

    public int IdPersona { get; set; }

    public int? IdAlerta { get; set; }

    public int? IdRecordatorio { get; set; }

    public int IdUsuario { get; set; }

    public virtual ICollection<HistorialTurno> HistorialTurnos { get; set; } = new List<HistorialTurno>();

    public virtual Alertum? IdAlertaNavigation { get; set; }

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    public virtual Recordatorio? IdRecordatorioNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<PacientePadecimiento> PacientePadecimientos { get; set; } = new List<PacientePadecimiento>();

    public virtual ICollection<Ritmo> Ritmos { get; set; } = new List<Ritmo>();
}
