using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Dtos;
using Treinu.Contracts.Commands.Alunos;

namespace Treinu.Contracts.Commands.Alunos;

public record AdicionarAvaliacaoFisicaAlunoCommand(
    Guid AlunoId,
    AvaliacaoFisicaDto AvaliacaoFisica
) : IRequest<Result<object>>;
