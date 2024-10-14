using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_health.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alerta",
                columns: table => new
                {
                    idAlerta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha_hora = table.Column<DateTime>(type: "datetime", nullable: true),
                    descripcion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__alerta__D099542711105225", x => x.idAlerta);
                });

            migrationBuilder.CreateTable(
                name: "padecimiento",
                columns: table => new
                {
                    idPadecimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__padecimi__D21C343147D1C7AE", x => x.idPadecimiento);
                });

            migrationBuilder.CreateTable(
                name: "persona",
                columns: table => new
                {
                    idPersona = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    primer_apellido = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    segundo_apellido = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fecha_nacimiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    telefono = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    calle = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    numero = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    codigo_postal = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    colonia = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__persona__A47881415AC454CC", x => x.idPersona);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    contrasenia = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    estatus = table.Column<bool>(type: "bit", nullable: true),
                    rol = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__usuario__645723A64355D1CF", x => x.idUsuario);
                });

            migrationBuilder.CreateTable(
                name: "monitoreo_salud",
                columns: table => new
                {
                    idMonitoreo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha_hora = table.Column<DateTime>(type: "datetime", nullable: true),
                    tipo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    idPadecimiento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__monitore__69E8E0BFD48DF962", x => x.idMonitoreo);
                    table.ForeignKey(
                        name: "FK__monitoreo__idPad__571DF1D5",
                        column: x => x.idPadecimiento,
                        principalTable: "padecimiento",
                        principalColumn: "idPadecimiento");
                });

            migrationBuilder.CreateTable(
                name: "doctor",
                columns: table => new
                {
                    idDoctor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cedula = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    num_doctor = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__doctor__418956C34DEE8F98", x => x.idDoctor);
                    table.ForeignKey(
                        name: "FK__doctor__idUsuari__5DCAEF64",
                        column: x => x.idUsuario,
                        principalTable: "usuario",
                        principalColumn: "idUsuario");
                });

            migrationBuilder.CreateTable(
                name: "enfermero",
                columns: table => new
                {
                    idEnfermero = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    num_enfermero = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    idPersona = table.Column<int>(type: "int", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__enfermer__A823C61843AE720E", x => x.idEnfermero);
                    table.ForeignKey(
                        name: "FK__enfermero__idPer__3F466844",
                        column: x => x.idPersona,
                        principalTable: "persona",
                        principalColumn: "idPersona");
                    table.ForeignKey(
                        name: "FK__enfermero__idUsu__403A8C7D",
                        column: x => x.idUsuario,
                        principalTable: "usuario",
                        principalColumn: "idUsuario");
                });

            migrationBuilder.CreateTable(
                name: "recordatorio",
                columns: table => new
                {
                    idRecordatorio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    medicamento = table.Column<string>(type: "text", nullable: true),
                    cantidad_medicamento = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fecha_inicio = table.Column<DateTime>(type: "datetime", nullable: true),
                    fecha_fin = table.Column<DateTime>(type: "datetime", nullable: true),
                    estatus = table.Column<bool>(type: "bit", nullable: true),
                    idEnfermero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__recordat__D132AA42FED1DCB3", x => x.idRecordatorio);
                    table.ForeignKey(
                        name: "FK__recordato__idEnf__4316F928",
                        column: x => x.idEnfermero,
                        principalTable: "enfermero",
                        principalColumn: "idEnfermero");
                });

            migrationBuilder.CreateTable(
                name: "paciente",
                columns: table => new
                {
                    idPaciente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    num_paciente = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    altura = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    peso = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    tipo_sangre = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    estatus = table.Column<bool>(type: "bit", nullable: true),
                    idPersona = table.Column<int>(type: "int", nullable: false),
                    idAlerta = table.Column<int>(type: "int", nullable: true),
                    idRecordatorio = table.Column<int>(type: "int", nullable: true),
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__paciente__F48A08F280942C64", x => x.idPaciente);
                    table.ForeignKey(
                        name: "FK__paciente__idAler__46E78A0C",
                        column: x => x.idAlerta,
                        principalTable: "alerta",
                        principalColumn: "idAlerta");
                    table.ForeignKey(
                        name: "FK__paciente__idReco__47DBAE45",
                        column: x => x.idRecordatorio,
                        principalTable: "recordatorio",
                        principalColumn: "idRecordatorio");
                    table.ForeignKey(
                        name: "FK__paciente__idUsua__45F365D3",
                        column: x => x.idPersona,
                        principalTable: "persona",
                        principalColumn: "idPersona");
                    table.ForeignKey(
                        name: "FK__paciente__idUsua__48CFD27E",
                        column: x => x.idUsuario,
                        principalTable: "usuario",
                        principalColumn: "idUsuario");
                });

            migrationBuilder.CreateTable(
                name: "historial_turno",
                columns: table => new
                {
                    idTurno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idEnfermero = table.Column<int>(type: "int", nullable: false),
                    idPaciente = table.Column<int>(type: "int", nullable: false),
                    turno = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__historia__AA068B012147D026", x => x.idTurno);
                    table.ForeignKey(
                        name: "FK__historial__idEnf__4BAC3F29",
                        column: x => x.idEnfermero,
                        principalTable: "enfermero",
                        principalColumn: "idEnfermero");
                    table.ForeignKey(
                        name: "FK__historial__idPac__4CA06362",
                        column: x => x.idPaciente,
                        principalTable: "paciente",
                        principalColumn: "idPaciente");
                });

            migrationBuilder.CreateTable(
                name: "paciente_padecimiento",
                columns: table => new
                {
                    idPacientePadecimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idPaciente = table.Column<int>(type: "int", nullable: false),
                    idPadecimiento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__paciente__3AE7E254DC45448E", x => x.idPacientePadecimiento);
                    table.ForeignKey(
                        name: "FK__paciente___idPac__59FA5E80",
                        column: x => x.idPaciente,
                        principalTable: "paciente",
                        principalColumn: "idPaciente");
                    table.ForeignKey(
                        name: "FK__paciente___idPad__5AEE82B9",
                        column: x => x.idPadecimiento,
                        principalTable: "padecimiento",
                        principalColumn: "idPadecimiento");
                });

            migrationBuilder.CreateTable(
                name: "ritmo",
                columns: table => new
                {
                    id_ritmo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    max = table.Column<int>(type: "int", nullable: true),
                    min = table.Column<int>(type: "int", nullable: true),
                    medicion = table.Column<int>(type: "int", nullable: true),
                    idPadecimiento = table.Column<int>(type: "int", nullable: false),
                    idPaciente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ritmo__5D4E3198D31B2151", x => x.id_ritmo);
                    table.ForeignKey(
                        name: "FK__ritmo__idPacient__534D60F1",
                        column: x => x.idPaciente,
                        principalTable: "paciente",
                        principalColumn: "idPaciente");
                    table.ForeignKey(
                        name: "FK__ritmo__idPadecim__5441852A",
                        column: x => x.idPadecimiento,
                        principalTable: "padecimiento",
                        principalColumn: "idPadecimiento");
                });

            migrationBuilder.CreateTable(
                name: "enfermero_turno",
                columns: table => new
                {
                    idEnfermero_turno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idEnfermero = table.Column<int>(type: "int", nullable: false),
                    idTurno = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__enfermer__FDBDD7AC042D8A29", x => x.idEnfermero_turno);
                    table.ForeignKey(
                        name: "FK__enfermero__idTur__4F7CD00D",
                        column: x => x.idEnfermero,
                        principalTable: "enfermero",
                        principalColumn: "idEnfermero");
                    table.ForeignKey(
                        name: "FK__enfermero__idTur__5070F446",
                        column: x => x.idTurno,
                        principalTable: "historial_turno",
                        principalColumn: "idTurno");
                });

            migrationBuilder.CreateIndex(
                name: "IX_doctor_idUsuario",
                table: "doctor",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_enfermero_idPersona",
                table: "enfermero",
                column: "idPersona");

            migrationBuilder.CreateIndex(
                name: "IX_enfermero_idUsuario",
                table: "enfermero",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_enfermero_turno_idEnfermero",
                table: "enfermero_turno",
                column: "idEnfermero");

            migrationBuilder.CreateIndex(
                name: "IX_enfermero_turno_idTurno",
                table: "enfermero_turno",
                column: "idTurno");

            migrationBuilder.CreateIndex(
                name: "IX_historial_turno_idEnfermero",
                table: "historial_turno",
                column: "idEnfermero");

            migrationBuilder.CreateIndex(
                name: "IX_historial_turno_idPaciente",
                table: "historial_turno",
                column: "idPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_monitoreo_salud_idPadecimiento",
                table: "monitoreo_salud",
                column: "idPadecimiento");

            migrationBuilder.CreateIndex(
                name: "IX_paciente_idAlerta",
                table: "paciente",
                column: "idAlerta");

            migrationBuilder.CreateIndex(
                name: "IX_paciente_idPersona",
                table: "paciente",
                column: "idPersona");

            migrationBuilder.CreateIndex(
                name: "IX_paciente_idRecordatorio",
                table: "paciente",
                column: "idRecordatorio");

            migrationBuilder.CreateIndex(
                name: "IX_paciente_idUsuario",
                table: "paciente",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_paciente_padecimiento_idPaciente",
                table: "paciente_padecimiento",
                column: "idPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_paciente_padecimiento_idPadecimiento",
                table: "paciente_padecimiento",
                column: "idPadecimiento");

            migrationBuilder.CreateIndex(
                name: "IX_recordatorio_idEnfermero",
                table: "recordatorio",
                column: "idEnfermero");

            migrationBuilder.CreateIndex(
                name: "IX_ritmo_idPaciente",
                table: "ritmo",
                column: "idPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_ritmo_idPadecimiento",
                table: "ritmo",
                column: "idPadecimiento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "doctor");

            migrationBuilder.DropTable(
                name: "enfermero_turno");

            migrationBuilder.DropTable(
                name: "monitoreo_salud");

            migrationBuilder.DropTable(
                name: "paciente_padecimiento");

            migrationBuilder.DropTable(
                name: "ritmo");

            migrationBuilder.DropTable(
                name: "historial_turno");

            migrationBuilder.DropTable(
                name: "padecimiento");

            migrationBuilder.DropTable(
                name: "paciente");

            migrationBuilder.DropTable(
                name: "alerta");

            migrationBuilder.DropTable(
                name: "recordatorio");

            migrationBuilder.DropTable(
                name: "enfermero");

            migrationBuilder.DropTable(
                name: "persona");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
