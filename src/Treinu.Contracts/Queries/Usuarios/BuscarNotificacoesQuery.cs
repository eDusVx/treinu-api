using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Queries.Usuarios;

public record BuscarNotificacoesQuery(
    Guid UsuarioId
) : IRequest<Result<object>>;
