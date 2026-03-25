using FluentResults;
using Treinu.Contracts.Queries;
using Treinu.Contracts.Responses;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
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
        var (total, usuariosList) = await _usuarioRepository.BuscarUsuariosPaginadoAsync(
            request.TipoUsuario,
            request.Page,
            request.Limit,
            cancellationToken
        );

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
}