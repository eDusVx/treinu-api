using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Events;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinos;

public class NotificarTreinoEventHandlers(INotificacaoRepository notificacaoRepository)
    : INotificationHandler<TreinoAtribuidoEvent>,
      INotificationHandler<TreinoAtualizadoEvent>
{
    public async Task Handle(TreinoAtribuidoEvent notification, CancellationToken cancellationToken)
    {
        var notificacao = Notificacao.Criar(new CriarNotificacaoProps(
            notification.AlunoId,
            "Novo Treino",
            $"O treino '{notification.NomeTreino}' foi criado e atribuído a você."
        ));
        
        if (notificacao.IsSuccess)
        {
            await notificacaoRepository.AdicionarNotificacaoAsync(notificacao.Value);
        }
    }

    public async Task Handle(TreinoAtualizadoEvent notification, CancellationToken cancellationToken)
    {
        var notificacao = Notificacao.Criar(new CriarNotificacaoProps(
            notification.AlunoId,
            "Treino Atualizado",
            $"O treino '{notification.NomeTreino}' foi atualizado pelo seu treinador."
        ));
        
        if (notificacao.IsSuccess)
        {
            await notificacaoRepository.AdicionarNotificacaoAsync(notificacao.Value);
        }
    }
}
