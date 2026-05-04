using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Chat;

public record CriarSalaChatCommand(
    Guid UsuarioCriadorId,
    string Nome,
    List<Guid> ParticipantesIds,
    bool EhGrupo
) : IRequest<Result<object>>;
