using FluentResults;
using Treinu.Domain.Entities;

namespace Treinu.Domain.Repositories;

public interface INotificacaoRepository
{
    Task<Result<List<Notificacao>>> BuscarNotificacoesPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<Result<Notificacao>> BuscarNotificacaoPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> AdicionarNotificacaoAsync(Notificacao notificacao);
    Task<Result> AtualizarNotificacaoAsync(Notificacao notificacao);
}
