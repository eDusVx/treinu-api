using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Queries.Alunos;

public record ObterDashboardEvolucaoQuery(
    Guid AlunoId
) : IRequest<Result<object>>;
