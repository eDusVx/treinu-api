using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Queries.Chat;

public record ListarSalasChatQuery(Guid UsuarioId) : IRequest<Result<object>>;
