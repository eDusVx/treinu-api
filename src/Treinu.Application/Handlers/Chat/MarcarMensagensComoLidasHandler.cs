using FluentResults;
using Treinu.Application.Interfaces;
using Treinu.Contracts.Commands.Chat;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Chat;

public class MarcarMensagensComoLidasHandler(
    IChatRepository chatRepository,
    IRealTimeChatService realTimeChatService) 
    : IRequestHandler<MarcarMensagensComoLidasCommand, Result<object>>
{
    public async Task<Result<object>> Handle(MarcarMensagensComoLidasCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var salaResult = await chatRepository.BuscarSalaPorIdAsync(request.SalaId, cancellationToken);
            if (salaResult.IsFailed) return Result.Fail<object>("Sala não encontrada.");

            var sala = salaResult.Value;

            var marcarResult = sala.MarcarMensagensComoLidas(request.UsuarioId);
            if (marcarResult.IsFailed) return Result.Fail<object>(marcarResult.Errors);

            var saveResult = await chatRepository.AtualizarSalaAsync(sala);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            // Notificar que leu as mensagens para zerar o contador global
            await realTimeChatService.NotificarMensagemNaoLidaAsync(request.UsuarioId, sala.Id, 0, cancellationToken);

            return Result.Ok((object)new { Sucesso = true });
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao marcar mensagens como lidas: {ex.Message}");
        }
    }
}
