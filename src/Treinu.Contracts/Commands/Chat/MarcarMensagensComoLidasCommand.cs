using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Chat;

public record MarcarMensagensComoLidasCommand(
    Guid SalaId,
    Guid UsuarioId
) : IRequest<Result<object>>;
