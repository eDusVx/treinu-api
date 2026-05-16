using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Queries.Plataforma;
using Treinu.Domain.Dtos;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Plataforma;

public class ObterRankingAlunosHandler(IPlataformaRepository plataformaRepository)
    : IRequestHandler<ObterRankingAlunosQuery, Result<List<RankingAlunoDto>>>
{
    public async Task<Result<List<RankingAlunoDto>>> Handle(ObterRankingAlunosQuery request, CancellationToken cancellationToken)
    {
        return await plataformaRepository.ObterRankingAlunosAsync(request.TreinadorId);
    }
}
