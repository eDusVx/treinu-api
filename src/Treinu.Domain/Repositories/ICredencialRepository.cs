using FluentResults;
using Treinu.Domain.Entities;

namespace Treinu.Domain.Repositories;

public interface ICredencialRepository
{
    Task<Result> SalvarCredencialAsync(Credencial credencial);
    Task<Result<Credencial?>> BuscarCredencialPorEmailAsync(string email);
    Task<Result<Credencial?>> BuscarCredencialPorRefreshTokenAsync(string refreshToken);
    Task<Result<Credencial?>> BuscarCredencialPorTokenRecuperacaoAsync(string token);
    Task<Result> AtualizarCredencialAsync(Credencial credencial);
}