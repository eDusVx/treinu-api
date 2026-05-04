using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Chat;

public record RemoverMembroSalaChatCommand(
    Guid SalaId,
    Guid UsuarioLogadoId,
    Guid MembroId
) : IRequest<Result<object>>;
