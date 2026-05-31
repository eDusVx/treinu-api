using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Usuarios;

public record ConfigurarNotificacoesCommand(
    Guid UsuarioId,
    bool ReceberEmail,
    bool ReceberPush,
    bool AlertaVencimentoAvaliacao,
    bool AlertaVencimentoTreino,
    bool AlertaNovoTreino
) : IRequest<Result>;
