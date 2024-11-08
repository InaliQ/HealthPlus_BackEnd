using System;
using System.Collections.Generic;

namespace Back_health.Models;

public partial class Recordatorio
{
    public int IdRecordatorio { get; set; }

    public string? Medicamento { get; set; }

    public string? CantidadMedicamento { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public bool? Estatus { get; set; }

    public int IdEnfermero { get; set; }

    public int? IdPaciente { get; set; }

    public virtual Enfermero IdEnfermeroNavigation { get; set; } = null!;

    public virtual ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
}
