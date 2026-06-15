using FluentResults;
using System;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Queries.Alunos;

public record ObterEvolucaoFisicaQuery(
    Guid AlunoId,
    DateTime? DataInicio,
    DateTime? DataFim
) : IRequest<Result<object>>;
