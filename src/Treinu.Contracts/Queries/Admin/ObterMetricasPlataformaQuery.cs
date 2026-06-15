using FluentResults;
using System;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Queries.Admin;

public record ObterMetricasPlataformaQuery(
    DateTime DataInicio,
    DateTime DataFim
) : IRequest<Result<object>>;
