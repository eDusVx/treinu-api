using FluentResults;
using System;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Queries.Alunos;

public record ObterComparativoPerformanceAlunosQuery(
    Guid? TreinadorId,
    DateTime? DataInicio,
    DateTime? DataFim,
    string? Agrupamento // "semana" or "mes"
) : IRequest<Result<object>>;
