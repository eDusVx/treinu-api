using Treinu.Domain.Core.Mediator;
using FluentResults;
using Treinu.Domain.Dtos;

namespace Treinu.Contracts.Queries.ExecucoesTreino;

public record ObterDetalhesExecucaoQuery(Guid ExecucaoTreinoId) : IRequest<Result<ExecucaoTreinoDto>>;
