using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;

namespace Treinu.Contracts.Queries.Treinos;

public record FiltrarTreinosQuery(
    Guid? AlunoId,
    Guid? TreinadorId,
    TreinoStatusEnum? Status
) : IRequest<Result<object>>;
