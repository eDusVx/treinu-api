using FluentResults;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Repositories;

public interface ITelemetriaRepository
{
    Task<Result> RegistrarEventoAsync(EventoTelemetria evento, CancellationToken cancellationToken = default);
    Task<Result<int>> ObterTotalUsuariosAtivosAsync(PerfilEnum perfil, CancellationToken cancellationToken = default);
    Task<Result<int>> ObterTotalNovosCadastrosAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<Result<int>> ObterTotalTreinosCriadosAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<Result<int>> ObterTotalTreinosConcluidosAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<Result<List<EventoTelemetria>>> ObterEventosInteracaoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
}
