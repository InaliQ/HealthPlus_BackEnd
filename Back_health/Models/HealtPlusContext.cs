using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Back_health.Models;

public partial class HealtPlusContext : DbContext
{
    public HealtPlusContext()
    {
    }

    public HealtPlusContext(DbContextOptions<HealtPlusContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alertum> Alerta { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Enfermero> Enfermeros { get; set; }

    public virtual DbSet<EnfermeroTurno> EnfermeroTurnos { get; set; }

    public virtual DbSet<HistorialTurno> HistorialTurnos { get; set; }

    public virtual DbSet<MonitoreoSalud> MonitoreoSaluds { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<PacientePadecimiento> PacientePadecimientos { get; set; }

    public virtual DbSet<Padecimiento> Padecimientos { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Recordatorio> Recordatorios { get; set; }

    public virtual DbSet<Ritmo> Ritmos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ADAGAKIAKILAP; Initial Catalog=healt_plus; user id=Inali; password=InaliQ01;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alertum>(entity =>
        {
            entity.HasKey(e => e.IdAlerta).HasName("PK__alerta__D099542711105225");

            entity.ToTable("alerta");

            entity.Property(e => e.IdAlerta).HasColumnName("idAlerta");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaHora)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora");
        }); 

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.IdDoctor).HasName("PK__doctor__418956C34DEE8F98");

            entity.ToTable("doctor");

            entity.Property(e => e.IdDoctor).HasColumnName("idDoctor");
            entity.Property(e => e.Cedula)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cedula");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.NumDoctor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("num_doctor");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__doctor__idUsuari__5DCAEF64");
        });

        modelBuilder.Entity<Enfermero>(entity =>
        {
            entity.HasKey(e => e.IdEnfermero).HasName("PK__enfermer__A823C61843AE720E");

            entity.ToTable("enfermero");

            entity.Property(e => e.IdEnfermero).HasColumnName("idEnfermero");
            entity.Property(e => e.IdPersona).HasColumnName("idPersona");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.NumEnfermero)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("num_enfermero");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Enfermeros)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__enfermero__idPer__3F466844");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Enfermeros)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__enfermero__idUsu__403A8C7D");
        });

        modelBuilder.Entity<EnfermeroTurno>(entity =>
        {
            entity.HasKey(e => e.IdEnfermeroTurno).HasName("PK__enfermer__FDBDD7AC042D8A29");

            entity.ToTable("enfermero_turno");

            entity.Property(e => e.IdEnfermeroTurno).HasColumnName("idEnfermero_turno");
            entity.Property(e => e.IdEnfermero).HasColumnName("idEnfermero");
            entity.Property(e => e.IdTurno).HasColumnName("idTurno");

            entity.HasOne(d => d.IdEnfermeroNavigation).WithMany(p => p.EnfermeroTurnos)
                .HasForeignKey(d => d.IdEnfermero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__enfermero__idTur__4F7CD00D");

            entity.HasOne(d => d.IdTurnoNavigation).WithMany(p => p.EnfermeroTurnos)
                .HasForeignKey(d => d.IdTurno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__enfermero__idTur__5070F446");
        });

        modelBuilder.Entity<HistorialTurno>(entity =>
        {
            entity.HasKey(e => e.IdTurno).HasName("PK__historia__AA068B012147D026");

            entity.ToTable("historial_turno");

            entity.Property(e => e.IdTurno).HasColumnName("idTurno");
            entity.Property(e => e.IdEnfermero).HasColumnName("idEnfermero");
            entity.Property(e => e.IdPaciente).HasColumnName("idPaciente");
            entity.Property(e => e.Turno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("turno");

            entity.HasOne(d => d.IdEnfermeroNavigation).WithMany(p => p.HistorialTurnos)
                .HasForeignKey(d => d.IdEnfermero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__historial__idEnf__4BAC3F29");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.HistorialTurnos)
                .HasForeignKey(d => d.IdPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__historial__idPac__4CA06362");
        });

        modelBuilder.Entity<MonitoreoSalud>(entity =>
        {
            entity.HasKey(e => e.IdMonitoreo).HasName("PK__monitore__69E8E0BFD48DF962");

            entity.ToTable("monitoreo_salud");

            entity.Property(e => e.IdMonitoreo).HasColumnName("idMonitoreo");
            entity.Property(e => e.FechaHora)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora");
            entity.Property(e => e.IdPadecimiento).HasColumnName("idPadecimiento");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo");

            entity.HasOne(d => d.IdPadecimientoNavigation).WithMany(p => p.MonitoreoSaluds)
                .HasForeignKey(d => d.IdPadecimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__monitoreo__idPad__571DF1D5");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.IdPaciente).HasName("PK__paciente__F48A08F280942C64");

            entity.ToTable("paciente");

            entity.Property(e => e.IdPaciente).HasColumnName("idPaciente");
            entity.Property(e => e.Altura)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("altura");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.IdAlerta).HasColumnName("idAlerta");
            entity.Property(e => e.IdPersona).HasColumnName("idPersona");
            entity.Property(e => e.IdRecordatorio).HasColumnName("idRecordatorio");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.NumPaciente)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("num_paciente");
            entity.Property(e => e.Peso)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("peso");
            entity.Property(e => e.TipoSangre)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipo_sangre");

            entity.HasOne(d => d.IdAlertaNavigation).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.IdAlerta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__paciente__idAler__46E78A0C");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__paciente__idUsua__45F365D3");

            entity.HasOne(d => d.IdRecordatorioNavigation).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.IdRecordatorio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__paciente__idReco__47DBAE45");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__paciente__idUsua__48CFD27E");
        });

        modelBuilder.Entity<PacientePadecimiento>(entity =>
        {
            entity.HasKey(e => e.IdPacientePadecimiento).HasName("PK__paciente__3AE7E254DC45448E");

            entity.ToTable("paciente_padecimiento");

            entity.Property(e => e.IdPacientePadecimiento).HasColumnName("idPacientePadecimiento");
            entity.Property(e => e.IdPaciente).HasColumnName("idPaciente");
            entity.Property(e => e.IdPadecimiento).HasColumnName("idPadecimiento");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.PacientePadecimientos)
                .HasForeignKey(d => d.IdPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__paciente___idPac__59FA5E80");

            entity.HasOne(d => d.IdPadecimientoNavigation).WithMany(p => p.PacientePadecimientos)
                .HasForeignKey(d => d.IdPadecimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__paciente___idPad__5AEE82B9");
        });

        modelBuilder.Entity<Padecimiento>(entity =>
        {
            entity.HasKey(e => e.IdPadecimiento).HasName("PK__padecimi__D21C343147D1C7AE");

            entity.ToTable("padecimiento");

            entity.Property(e => e.IdPadecimiento).HasColumnName("idPadecimiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("PK__persona__A47881415AC454CC");

            entity.ToTable("persona");

            entity.Property(e => e.IdPersona).HasColumnName("idPersona");
            entity.Property(e => e.Calle)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("calle");
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("codigo_postal");
            entity.Property(e => e.Colonia)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("colonia");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("numero");
            entity.Property(e => e.PrimerApellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("primer_apellido");
            entity.Property(e => e.SegundoApellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("segundo_apellido");
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Recordatorio>(entity =>
        {
            entity.HasKey(e => e.IdRecordatorio).HasName("PK__recordat__D132AA42FED1DCB3");

            entity.ToTable("recordatorio");

            entity.Property(e => e.IdRecordatorio).HasColumnName("idRecordatorio");
            entity.Property(e => e.CantidadMedicamento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cantidad_medicamento");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.IdEnfermero).HasColumnName("idEnfermero");
            entity.Property(e => e.Medicamento)
                .HasColumnType("text")
                .HasColumnName("medicamento");

            entity.HasOne(d => d.IdEnfermeroNavigation).WithMany(p => p.Recordatorios)
                .HasForeignKey(d => d.IdEnfermero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recordato__idEnf__4316F928");
        });

        modelBuilder.Entity<Ritmo>(entity =>
        {
            entity.HasKey(e => e.IdRitmo).HasName("PK__ritmo__5D4E3198D31B2151");

            entity.ToTable("ritmo");

            entity.Property(e => e.IdRitmo).HasColumnName("id_ritmo");
            entity.Property(e => e.IdPaciente).HasColumnName("idPaciente");
            entity.Property(e => e.IdPadecimiento).HasColumnName("idPadecimiento");
            entity.Property(e => e.Max).HasColumnName("max");
            entity.Property(e => e.Medicion).HasColumnName("medicion");
            entity.Property(e => e.Min).HasColumnName("min");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Ritmos)
                .HasForeignKey(d => d.IdPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ritmo__idPacient__534D60F1");

            entity.HasOne(d => d.IdPadecimientoNavigation).WithMany(p => p.Ritmos)
                .HasForeignKey(d => d.IdPadecimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ritmo__idPadecim__5441852A");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuario__645723A64355D1CF");

            entity.ToTable("usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contrasenia");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("rol");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
