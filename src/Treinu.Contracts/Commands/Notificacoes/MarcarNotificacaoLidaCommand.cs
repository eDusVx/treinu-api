using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Notificacoes;

public record MarcarNotificacaoLidaCommand(
    Guid NotificacaoId,
    Guid UsuarioId
) : IRequest<Result<object>>;
