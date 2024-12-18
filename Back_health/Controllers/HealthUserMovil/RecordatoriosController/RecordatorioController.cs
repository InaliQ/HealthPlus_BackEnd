﻿using Back_health.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_health.Controllers.HealthUserMovil.RecordatoriosController
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecordatorioController : Controller
    {
        private readonly HealtPlusContext _baseDatos;

        public RecordatorioController(HealtPlusContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpPut]
        [Route("CompletarRecordatorio/{idRecordatorio}")]
        public async Task<IActionResult> CompletarRecordatorio(int idRecordatorio)
        {
            var recordatorio = await _baseDatos.Recordatorios.FindAsync(idRecordatorio);

            if (recordatorio == null)
            {
                return NotFound("Recordatorio no encontrado.");
            }

            recordatorio.FechaFin = DateTime.Now;
            recordatorio.Estatus = true;

            await _baseDatos.SaveChangesAsync();

            return Ok("Recordatorio actualizado exitosamente.");
        }

        [HttpGet("UltimaAlertaRecordatorio/{idPaciente}")]
        public IActionResult GetUltimaAlertaYRecordatorio(int idPaciente)
        {
            var ultimaAlerta = _baseDatos.Alerta
                .Where(a => a.IdPaciente == idPaciente)
                .OrderByDescending(a => a.FechaHora)
                .FirstOrDefault();

            var ultimoRecordatorio = _baseDatos.Recordatorios
                .Where(r => r.IdPaciente == idPaciente && r.Estatus == false)
                .OrderByDescending(r => r.FechaFin)
                .FirstOrDefault();

            if (ultimoRecordatorio == null)
            {
                ultimoRecordatorio = _baseDatos.Recordatorios
                    .Where(r => r.IdPaciente == idPaciente && r.Estatus == true)
                    .OrderByDescending(r => r.FechaFin)
                    .FirstOrDefault();
            }

            if (ultimaAlerta == null && ultimoRecordatorio == null)
            {
                return NotFound(new { message = "No se encontraron datos para este paciente." });
            }

            return Ok(new
            {
                UltimaAlerta = ultimaAlerta,
                UltimoRecordatorio = ultimoRecordatorio
            });
        }

        // Nueva petición GET para obtener todos los recordatorios del paciente
        [HttpGet("TodosRecordatorios/{idPaciente}")]
        public IActionResult GetTodosRecordatorios(int idPaciente)
        {
            var recordatorios = _baseDatos.Recordatorios
                .Where(r => r.IdPaciente == idPaciente)
                .OrderBy(r => r.FechaInicio)
                .ToList();

            if (recordatorios == null || !recordatorios.Any())
            {
                return NotFound(new { message = "No se encontraron recordatorios para este paciente." });
            }

            return Ok(recordatorios);
        }
    }
}
