using FluentResults;
using Treinu.Contracts.Commands.Alunos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Dtos;

namespace Treinu.Contracts.Commands.Alunos;

public record AdicionarAvaliacaoFisicaProprioAlunoCommand(
    Guid AlunoId,
    AvaliacaoFisicaDto AvaliacaoFisica
) : IRequest<Result<object>>;
