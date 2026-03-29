using FluentResults;
using Treinu.Contracts.Responses;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;
using Treinu.Contracts.Queries.Usuarios;

namespace Treinu.Contracts.Queries.Usuarios;

public record BuscarUsuariosQuery(
    PerfilEnum? TipoUsuario = null,
    int Page = 1,
    int Limit = 10
) : IRequest<Result<PaginationResponse<object>>>;
