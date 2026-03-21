using MediatR;
using Treinu.Contracts.Responses;

namespace Treinu.Contracts.Queries;

public record AutenticarUsuarioLocalQuery(
    string Email,
    string Senha
) : IRequest<TokenDto>;

public record RenovarTokenQuery(
    string RefreshToken
) : IRequest<TokenDto>;
