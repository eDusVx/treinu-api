using MediatR;
using Treinu.Contracts.Responses;

namespace Treinu.Contracts.Queries;

public record AutenticarUsuarioLocalQuery(
    string Email,
    string Senha
) : IRequest<TokenDto>;

public record AutenticarUsuarioGoogleQuery(
    string Token
) : IRequest<TokenDto>;
