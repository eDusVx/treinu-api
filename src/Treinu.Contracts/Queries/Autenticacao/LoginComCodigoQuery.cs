using FluentResults;
using Treinu.Contracts.Responses;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Queries.Autenticacao;

namespace Treinu.Contracts.Queries.Autenticacao;

public record LoginComCodigoQuery(
    string Email,
    string Codigo
) : IRequest<Result<TokenDto>>;
