using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Treinos;

public record EditarTreinoCommand(
    Guid TreinoId,
    string Nome,
    string Descricao,
    DateTime DataInicio,
    DateTime DataFim,
    Guid TreinadorId
) : IRequest<Result<object>>;
