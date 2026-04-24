using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Events;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Notificacoes;

// Handler para TreinoAtribuidoEvent
public class NotificarTreinoAtribuidoHandler(INotificacaoRepository notificacaoRepository)
    : INotificationHandler<TreinoAtribuidoEvent>
{
    public async Task Handle(TreinoAtribuidoEvent notification, CancellationToken cancellationToken)
    {
        var props = new CriarNotificacaoProps(
            notification.AlunoId,
            "Novo Treino Atribuído",
            $"O treinador designou um novo treino para você: {notification.NomeTreino}"
        );

        var notificacaoResult = Notificacao.Criar(props);
        if (notificacaoResult.IsSuccess)
        {
            await notificacaoRepository.AdicionarNotificacaoAsync(notificacaoResult.Value);
        }
    }
}

// Handler para TreinoVencidoEvent
public class NotificarTreinoVencidoHandler(INotificacaoRepository notificacaoRepository)
    : INotificationHandler<TreinoVencidoEvent>
{
    public async Task Handle(TreinoVencidoEvent notification, CancellationToken cancellationToken)
    {
        var props = new CriarNotificacaoProps(
            notification.AlunoId,
            "Treino Vencido",
            $"O treino '{notification.NomeTreino}' atingiu a data de vencimento."
        );

        var notificacaoResult = Notificacao.Criar(props);
        if (notificacaoResult.IsSuccess)
        {
            await notificacaoRepository.AdicionarNotificacaoAsync(notificacaoResult.Value);
        }
    }
}

// Handler para AvaliacaoFisicaEnviadaEvent
public class NotificarAvaliacaoEnviadaHandler(INotificacaoRepository notificacaoRepository)
    : INotificationHandler<AvaliacaoFisicaEnviadaEvent>
{
    public async Task Handle(AvaliacaoFisicaEnviadaEvent notification, CancellationToken cancellationToken)
    {
        if (!notification.TreinadorId.HasValue) return;

        var props = new CriarNotificacaoProps(
            notification.TreinadorId.Value,
            "Nova Avaliação Física",
            $"O aluno enviou uma nova avaliação física que já está disponível no painel."
        );

        var notificacaoResult = Notificacao.Criar(props);
        if (notificacaoResult.IsSuccess)
        {
            await notificacaoRepository.AdicionarNotificacaoAsync(notificacaoResult.Value);
        }
    }
}
