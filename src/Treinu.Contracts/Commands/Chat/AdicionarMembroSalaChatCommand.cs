using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Chat;

public record AdicionarMembroSalaChatCommand(
    Guid SalaId,
    Guid UsuarioLogadoId,
    Guid NovoMembroId
) : IRequest<Result<object>>;
