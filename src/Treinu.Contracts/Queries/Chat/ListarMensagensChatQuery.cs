using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Queries.Chat;

public record ListarMensagensChatQuery(
    Guid SalaId,
    Guid UsuarioId,
    int Page = 1,
    int Limit = 50
) : IRequest<Result<object>>;
