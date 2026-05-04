using Treinu.Domain.Entities.Chat;

namespace Treinu.Application.Interfaces;

public interface IRealTimeChatService
{
    Task NotificarNovaMensagemAsync(MensagemChat mensagem, string nomeRemetente, CancellationToken cancellationToken = default);
    Task NotificarMensagemNaoLidaAsync(Guid usuarioId, Guid salaId, int quantidade, CancellationToken cancellationToken = default);
}
