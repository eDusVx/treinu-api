using FluentResults;
using Treinu.Domain.Entities;

namespace Treinu.Domain.Repositories;

public interface IConviteRepository
{
    Task<Result> SalvarConviteAsync(Convite convite);
    Task<Result<Convite>> BuscarPorTokenAsync(Guid token);
    Task<Result> AtualizarConviteAsync(Convite convite);
    Task<bool> ExisteConvitePendenteParaEmailAsync(string email);
}
