using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Responses;

namespace Treinu.Contracts.Queries;

public record AutenticarUsuarioLocalQuery(
    string Email,
    string Senha
) : IRequest<Result<TokenDto>>;

public record RenovarTokenQuery(
    string RefreshToken
) : IRequest<Result<TokenDto>>;
