using FluentResults;
using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;
using Treinu.Infrastructure.Data;

namespace Treinu.Infrastructure.Repositories;

public class TelemetriaRepository : ITelemetriaRepository
{
    private readonly AppDbContext _context;

    public TelemetriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> RegistrarEventoAsync(EventoTelemetria evento, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.EventosTelemetria.AddAsync(evento, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro ao registrar evento de telemetria: {ex.Message}");
        }
    }

    public async Task<Result<int>> ObterTotalUsuariosAtivosAsync(PerfilEnum perfil, CancellationToken cancellationToken = default)
    {
        try
        {
            var count = await _context.Usuarios
                .CountAsync(u => u.Ativo && u.Perfil == perfil, cancellationToken);
            return Result.Ok(count);
        }
        catch (Exception ex)
        {
            return Result.Fail<int>($"Erro ao obter total de usuários ativos: {ex.Message}");
        }
    }

    public async Task<Result<int>> ObterTotalNovosCadastrosAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        try
        {
            var inicioUtc = DateTime.SpecifyKind(inicio, DateTimeKind.Utc);
            var fimUtc = DateTime.SpecifyKind(fim, DateTimeKind.Utc);

            var count = await _context.Usuarios
                .CountAsync(u => u.DataCriacao >= inicioUtc && u.DataCriacao <= fimUtc, cancellationToken);
            return Result.Ok(count);
        }
        catch (Exception ex)
        {
            return Result.Fail<int>($"Erro ao obter total de novos cadastros: {ex.Message}");
        }
    }

    public async Task<Result<int>> ObterTotalTreinosCriadosAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        try
        {
            var inicioUtc = DateTime.SpecifyKind(inicio, DateTimeKind.Utc);
            var fimUtc = DateTime.SpecifyKind(fim, DateTimeKind.Utc);

            // Using DataInicio as the date when Treino was created/assigned
            var count = await _context.Treinos
                .CountAsync(t => t.DataInicio >= inicioUtc && t.DataInicio <= fimUtc, cancellationToken);
            return Result.Ok(count);
        }
        catch (Exception ex)
        {
            return Result.Fail<int>($"Erro ao obter total de treinos criados: {ex.Message}");
        }
    }

    public async Task<Result<int>> ObterTotalTreinosConcluidosAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        try
        {
            var inicioUtc = DateTime.SpecifyKind(inicio, DateTimeKind.Utc);
            var fimUtc = DateTime.SpecifyKind(fim, DateTimeKind.Utc);

            var count = await _context.ExecucoesTreino
                .CountAsync(e => e.Concluido && e.DataFim.HasValue && e.DataFim.Value >= inicioUtc && e.DataFim.Value <= fimUtc, cancellationToken);
            return Result.Ok(count);
        }
        catch (Exception ex)
        {
            return Result.Fail<int>($"Erro ao obter total de treinos concluídos: {ex.Message}");
        }
    }

    public async Task<Result<List<EventoTelemetria>>> ObterEventosInteracaoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        try
        {
            var inicioUtc = DateTime.SpecifyKind(inicio, DateTimeKind.Utc);
            var fimUtc = DateTime.SpecifyKind(fim, DateTimeKind.Utc);

            var eventos = await _context.EventosTelemetria
                .Where(e => e.DataOcorrencia >= inicioUtc && e.DataOcorrencia <= fimUtc)
                .ToListAsync(cancellationToken);
            return Result.Ok(eventos);
        }
        catch (Exception ex)
        {
            return Result.Fail<List<EventoTelemetria>>($"Erro ao obter eventos de interação: {ex.Message}");
        }
    }
}
