using FluentResults;
using Treinu.Domain.Entities;

namespace Treinu.Domain.Repositories;

public interface IContatoRepository
{
    Task<Result> AdicionarContatoAsync(Contato contato, Guid usuarioId, CancellationToken cancellationToken = default);
    Task<Result> RemoverContatoAsync(Guid contatoId, Guid usuarioId, CancellationToken cancellationToken = default);
}
