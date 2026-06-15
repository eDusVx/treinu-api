using FluentResults;
using Treinu.Application.Interfaces;
using Treinu.Contracts.Commands.Chat;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities.Chat;
using Treinu.Domain.Repositories;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;

namespace Treinu.Application.Handlers.Chat;

public class EnviarMensagemChatHandler(
    IChatRepository chatRepository,
    IRealTimeChatService realTimeChatService,
    ITelemetriaRepository telemetriaRepository) 
    : IRequestHandler<EnviarMensagemChatCommand, Result<object>>
{
    public async Task<Result<object>> Handle(EnviarMensagemChatCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var salaResult = await chatRepository.BuscarSalaPorIdAsync(request.SalaId, cancellationToken);
            if (salaResult.IsFailed) return Result.Fail<object>("Sala não encontrada.");

            var sala = salaResult.Value;

            var mensagemResult = sala.AdicionarMensagem(request.RemetenteId, request.Conteudo, TipoMensagem.Texto);
            if (mensagemResult.IsFailed) return Result.Fail<object>(mensagemResult.Errors);

            var saveResult = await chatRepository.AtualizarSalaAsync(sala);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            var mensagem = mensagemResult.Value;

            // Broadcast via SignalR abstrato
            await realTimeChatService.NotificarNovaMensagemAsync(mensagem, request.NomeRemetente, cancellationToken);

            // Notificar usuários para atualizar contador de notificações globais
            foreach (var participante in sala.Participantes.Where(p => p.UsuarioId != request.RemetenteId))
            {
                await realTimeChatService.NotificarMensagemNaoLidaAsync(participante.UsuarioId, sala.Id, participante.MensagensNaoLidas, cancellationToken);
            }

            // Log telemetry event
            var msgEvent = EventoTelemetria.Criar(request.RemetenteId, TipoInteracaoEnum.MENSAGEM_CHAT);
            await telemetriaRepository.RegistrarEventoAsync(msgEvent, cancellationToken);

            return Result.Ok((object)new { MensagemId = mensagem.Id });
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao enviar mensagem: {ex.Message}");
        }
    }
}
