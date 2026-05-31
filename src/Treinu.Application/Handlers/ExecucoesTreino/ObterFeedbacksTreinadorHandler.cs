using Treinu.Domain.Core.Mediator;
using FluentResults;
using Treinu.Contracts.Queries.ExecucoesTreino;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.ExecucoesTreino;

public class ObterFeedbacksTreinadorHandler(IExecucaoTreinoRepository execucaoTreinoRepository)
    : IRequestHandler<ObterFeedbacksTreinadorQuery, Result<List<Treinu.Domain.Dtos.FeedbackTreinoDto>>>
{
    public async Task<Result<List<Treinu.Domain.Dtos.FeedbackTreinoDto>>> Handle(ObterFeedbacksTreinadorQuery request, CancellationToken cancellationToken)
    {
        return await execucaoTreinoRepository.BuscarFeedbacksPorTreinadorAsync(request.TreinadorId);
    }
}
