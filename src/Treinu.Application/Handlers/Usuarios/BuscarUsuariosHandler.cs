using FluentResults;
using Treinu.Contracts.Queries.Usuarios;
using Treinu.Contracts.Responses;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Usuarios;

public class BuscarUsuariosHandler : IRequestHandler<BuscarUsuariosQuery, Result<PaginationResponse<object>>>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public BuscarUsuariosHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Result<PaginationResponse<object>>> Handle(BuscarUsuariosQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var pagedResult = await _usuarioRepository.BuscarUsuariosPaginadoAsync(
                request.TipoUsuario,
                request.Page,
                request.Limit,
                cancellationToken
            );

            if (pagedResult.IsFailed) return Result.Fail<PaginationResponse<object>>(pagedResult.Errors);

            var (total, usuariosList) = pagedResult.Value;

            var data = usuariosList.Select(u => u.ToDto()).ToList();

            var totalPages = (int)Math.Ceiling(total / (double)request.Limit);

            return Result.Ok(new PaginationResponse<object>(
                data!,
                total,
                request.Page,
                request.Limit,
                totalPages
            ));
        }
        catch (Exception ex)
        {
            return Result.Fail<PaginationResponse<object>>($"Erro inesperado ao buscar usuários: {ex.Message}");
        }
    }
}