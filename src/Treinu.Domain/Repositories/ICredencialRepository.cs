using Treinu.Domain.Entities;

namespace Treinu.Domain.Repositories;

public interface ICredencialRepository
{
    Task SalvarCredencialAsync(Credencial credencial);
    Task<Credencial?> BuscarCredencialPorEmailAsync(string email);
    Task<Credencial?> BuscarCredencialPorRefreshTokenAsync(string refreshToken);
    Task AtualizarCredencialAsync(Credencial credencial);
}
