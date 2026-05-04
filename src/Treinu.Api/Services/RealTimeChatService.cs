using Microsoft.AspNetCore.SignalR;
using Treinu.Api.Hubs;
using Treinu.Application.Interfaces;
using Treinu.Domain.Entities.Chat;

namespace Treinu.Api.Services;

public class RealTimeChatService : IRealTimeChatService
{
    private readonly IHubContext<ChatHub> _hubContext;

    public RealTimeChatService(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotificarNovaMensagemAsync(MensagemChat mensagem, string nomeRemetente, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.Group($"Sala_{mensagem.SalaChatId}").SendAsync("NovaMensagem", new 
        {
            Id = mensagem.Id,
            SalaId = mensagem.SalaChatId,
            RemetenteId = mensagem.RemetenteId,
            NomeRemetente = nomeRemetente,
            Conteudo = mensagem.Conteudo,
            DataEnvio = mensagem.DataEnvio,
            Tipo = mensagem.Tipo.ToString()
        }, cancellationToken);
    }

    public async Task NotificarMensagemNaoLidaAsync(Guid usuarioId, Guid salaId, int quantidade, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.Group($"User_{usuarioId}").SendAsync("NovaNotificacaoChat", new 
        {
            SalaId = salaId,
            MensagensNaoLidas = quantidade
        }, cancellationToken);
    }
}
