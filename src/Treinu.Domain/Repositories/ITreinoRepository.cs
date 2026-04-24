using FluentResults;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Repositories;

public interface ITreinoRepository
{
    Task<Result<Treino>> BuscarTreinoPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<Treino>>> BuscarTreinosPorAlunoAsync(Guid alunoId, TreinoStatusEnum? status, CancellationToken cancellationToken = default);
    Task<Result<List<Treino>>> BuscarTreinosPorTreinadorAsync(Guid treinadorId, TreinoStatusEnum? status, CancellationToken cancellationToken = default);
    Task<Result<List<Treino>>> BuscarTreinosVencidosAtivosAsync(DateTime dataVencimento, CancellationToken cancellationToken = default);
    Task<Result> AdicionarTreinoAsync(Treino treino);
    Task<Result> AtualizarTreinoAsync(Treino treino);
}
