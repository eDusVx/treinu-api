using MediatR;
using Treinu.Contracts.Queries;
using Treinu.Contracts.Responses;
using Treinu.Domain.Repositories;
using Treinu.Domain.Entities;
using Treinu.Domain.Dtos;

namespace Treinu.Application.Handlers.Usuarios;

public class BuscarUsuariosQueryHandler : IRequestHandler<BuscarUsuariosQuery, PaginationResponse<object>>
{
    private readonly IUsuarioRepository _usuarioRepository;
    
    public BuscarUsuariosQueryHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<PaginationResponse<object>> Handle(BuscarUsuariosQuery request, CancellationToken cancellationToken)
    {
        var (total, usuariosList) = await _usuarioRepository.BuscarUsuariosPaginadoAsync(
            request.TipoUsuario, 
            request.Page, 
            request.Limit, 
            cancellationToken
        );

        var data = usuariosList.Select(u => 
        {
            if (u is Aluno a) return (object)a.ToDto();
            if (u is Treinador t) return (object)t.ToDto();
            throw new InvalidOperationException("Unrecognized kind of user.");
        }).ToList();

        var totalPages = (int)Math.Ceiling(total / (double)request.Limit);

        return new PaginationResponse<object>(
            data,
            total,
            request.Page,
            request.Limit,
            totalPages
        );
    }
}


