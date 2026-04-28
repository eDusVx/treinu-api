using FluentResults;
using Treinu.Contracts.Commands.Notificacoes;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Notificacoes;

public class MarcarNotificacaoLidaHandler(
    INotificacaoRepository notificacaoRepository) : IRequestHandler<MarcarNotificacaoLidaCommand, Result<object>>
{
    public async Task<Result<object>> Handle(MarcarNotificacaoLidaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var notificacaoResult = await notificacaoRepository.BuscarNotificacaoPorIdAsync(request.NotificacaoId, cancellationToken);
            if (notificacaoResult.IsFailed) return Result.Fail<object>(notificacaoResult.Errors);

            var notificacao = notificacaoResult.Value;

            if (notificacao.UsuarioId != request.UsuarioId)
                return Result.Fail<object>("Notificação não pertence ao usuário.");

            var marcarResult = notificacao.MarcarComoLida();
            if (marcarResult.IsFailed) return Result.Fail<object>(marcarResult.Errors);

            var saveResult = await notificacaoRepository.AtualizarNotificacaoAsync(notificacao);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok((object)notificacao.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao marcar notificação como lida: {ex.Message}");
        }
    }
}
