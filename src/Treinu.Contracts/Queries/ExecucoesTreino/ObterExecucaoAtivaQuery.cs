using Treinu.Domain.Core.Mediator;
using FluentResults;
using Treinu.Domain.Dtos;

namespace Treinu.Contracts.Queries.ExecucoesTreino;

public record ObterExecucaoAtivaQuery(Guid AlunoId, Guid TreinoId) : IRequest<Result<ExecucaoTreinoDto>>;
