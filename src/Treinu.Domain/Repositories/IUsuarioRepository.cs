using FluentResults;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Repositories;

public interface IUsuarioRepository
{
    Task<Result> VerificarExistenciaAsync(string email, string cpf);
    Task<Result> SalvarUsuarioAsync(Usuario usuario);

    Task<Result<(int Total, IEnumerable<Usuario> Usuarios)>> BuscarUsuariosPaginadoAsync(PerfilEnum? tipoUsuario, int page,
        int limit, CancellationToken cancellationToken);
}