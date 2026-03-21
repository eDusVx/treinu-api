using FluentResults;
using Treinu.Contracts.Responses;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Queries;

public record AutenticarUsuarioLocalQuery(
    string Email,
    string Senha
) : IRequest<Result<TokenDto>>;

public record RenovarTokenQuery(
    string RefreshToken
) : IRequest<Result<TokenDto>>;