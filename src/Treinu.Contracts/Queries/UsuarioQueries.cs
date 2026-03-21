using FluentResults;
using Treinu.Contracts.Responses;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;

namespace Treinu.Contracts.Queries;

public record BuscarUsuariosQuery(
    PerfilEnum? TipoUsuario = null,
    int Page = 1,
    int Limit = 10
) : IRequest<Result<PaginationResponse<object>>>;