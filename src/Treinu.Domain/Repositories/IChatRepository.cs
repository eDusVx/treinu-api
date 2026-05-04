using FluentResults;
using Treinu.Domain.Entities.Chat;

namespace Treinu.Domain.Repositories;

public interface IChatRepository
{
    Task<Result<SalaChat>> BuscarSalaPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<SalaChat>>> BuscarSalasDoUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<Result<List<MensagemChat>>> BuscarMensagensDaSalaAsync(Guid salaId, int page, int limit, CancellationToken cancellationToken = default);
    Task<Result<SalaChat>> BuscarSalaDiretaAsync(Guid usuario1Id, Guid usuario2Id, CancellationToken cancellationToken = default);
    Task<Result> AdicionarSalaAsync(SalaChat sala);
    Task<Result> AtualizarSalaAsync(SalaChat sala);
    Task<Result> AdicionarMensagemAsync(MensagemChat mensagem);
}
