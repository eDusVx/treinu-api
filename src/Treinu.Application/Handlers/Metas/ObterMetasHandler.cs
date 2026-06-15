using FluentResults;
using System;
using System.Threading;
using System.Threading.Tasks;
using Treinu.Contracts.Queries.Metas;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Metas;

public class ObterMetasHandler(IMetaRepository metaRepository)
    : IRequestHandler<ObterMetasQuery, Result<object>>
{
    public async Task<Result<object>> Handle(ObterMetasQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var metas = await metaRepository.BuscarMetasPorAlunoAsync(request.AlunoId, cancellationToken);
            return Result.Ok((object)metas);
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao buscar metas: {ex.Message}");
        }
    }
}
