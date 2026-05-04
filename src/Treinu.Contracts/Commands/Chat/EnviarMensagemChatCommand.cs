using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Chat;

public record EnviarMensagemChatCommand(
    Guid SalaId,
    Guid RemetenteId,
    string Conteudo,
    string NomeRemetente
) : IRequest<Result<object>>;
