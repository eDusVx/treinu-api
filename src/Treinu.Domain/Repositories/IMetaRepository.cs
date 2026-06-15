using FluentResults;
using Treinu.Domain.Entities;

namespace Treinu.Domain.Repositories;

public interface IMetaRepository
{
    Task<Result> SalvarMetaAsync(Meta meta, CancellationToken cancellationToken = default);
    Task<List<Meta>> BuscarMetasPorAlunoAsync(Guid alunoId, CancellationToken cancellationToken = default);
    Task<Meta?> BuscarPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> AtualizarMetaAsync(Meta meta, CancellationToken cancellationToken = default);
}
