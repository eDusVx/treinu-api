using FluentResults;
using Treinu.Contracts.Responses;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Queries.Autenticacao;

namespace Treinu.Contracts.Queries.Autenticacao;

public record AutenticarUsuarioLocalQuery(
    string Email,
    string Senha
) : IRequest<Result<TokenDto>>;
