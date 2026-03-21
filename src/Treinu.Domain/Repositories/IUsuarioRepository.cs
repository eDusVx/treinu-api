using Treinu.Domain.Entities;

namespace Treinu.Domain.Repositories;

public interface IUsuarioRepository
{
    Task VerificarExistenciaAsync(string email, string cpf);
    Task SalvarUsuarioAsync(Usuario usuario);
    Task<(int Total, IEnumerable<Usuario> Usuarios)> BuscarUsuariosPaginadoAsync(Treinu.Domain.Enums.PerfilEnum? tipoUsuario, int page, int limit, CancellationToken cancellationToken);
}

