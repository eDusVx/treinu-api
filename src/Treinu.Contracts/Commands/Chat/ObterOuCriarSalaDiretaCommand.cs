using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Chat;

public record ObterOuCriarSalaDiretaCommand(
    Guid UsuarioLogadoId,
    Guid OutroUsuarioId
) : IRequest<Result<object>>;
