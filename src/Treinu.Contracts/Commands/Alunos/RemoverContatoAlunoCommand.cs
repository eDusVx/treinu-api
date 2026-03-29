using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Alunos;

namespace Treinu.Contracts.Commands.Alunos;

public record RemoverContatoAlunoCommand(
    Guid AlunoId,
    Guid ContatoId
) : IRequest<Result<object>>;
