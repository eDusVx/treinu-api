using Treinu.Domain.Core.Mediator;
using FluentResults;

namespace Treinu.Contracts.Queries.ExecucoesTreino;

public record ObterFeedbacksTreinadorQuery(Guid TreinadorId) : IRequest<Result<List<Treinu.Domain.Dtos.FeedbackTreinoDto>>>;
