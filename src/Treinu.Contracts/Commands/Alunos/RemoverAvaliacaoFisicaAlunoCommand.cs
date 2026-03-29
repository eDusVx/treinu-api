using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Alunos;

namespace Treinu.Contracts.Commands.Alunos;

public record RemoverAvaliacaoFisicaAlunoCommand(
    Guid AlunoId,
    Guid AvaliacaoFisicaId
) : IRequest<Result<object>>;
