using FluentResults;
using System;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Queries.Metas;

public record ObterMetasQuery(Guid AlunoId) : IRequest<Result<object>>;
