using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Treinadores;

namespace Treinu.Contracts.Commands.Treinadores;

public record RemoverContatoTreinadorCommand(
    Guid TreinadorId,
    Guid ContatoId
) : IRequest<Result<object>>;
