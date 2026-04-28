using FluentResults;
using Treinu.Domain.Entities.AvaliacaoFisica;

namespace Treinu.Domain.Repositories;

public interface IAvaliacaoFisicaRepository
{
    Task<Result> AdicionarAvaliacaoFisicaAsync(AvaliacaoFisica avaliacao, Guid alunoId, CancellationToken cancellationToken = default);
    Task<Result> RemoverAvaliacaoFisicaAsync(Guid avaliacaoId, Guid alunoId, CancellationToken cancellationToken = default);
}
