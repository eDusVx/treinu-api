using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Treinos;

public record ExcluirTreinoCommand(
    Guid TreinoId,
    Guid TreinadorId
) : IRequest<Result<object>>;
