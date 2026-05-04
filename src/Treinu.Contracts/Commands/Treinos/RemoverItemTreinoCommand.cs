using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Treinos;

public record RemoverItemTreinoCommand(
    Guid TreinoId,
    Guid ItemId,
    Guid TreinadorId
) : IRequest<Result<object>>;
