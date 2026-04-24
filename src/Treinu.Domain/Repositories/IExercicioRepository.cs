using FluentResults;
using Treinu.Domain.Entities;

namespace Treinu.Domain.Repositories;

public interface IExercicioRepository
{
    Task<Result<Exercicio>> BuscarExercicioPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<Exercicio>>> BuscarExerciciosPorTreinadorAsync(Guid treinadorId, string? tags, CancellationToken cancellationToken = default);
    Task<Result> AdicionarExercicioAsync(Exercicio exercicio);
}
