using FluentResults;
using Treinu.Contracts.Queries.Treinos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinos;

public class FiltrarTreinosHandler(ITreinoRepository treinoRepository) : IRequestHandler<FiltrarTreinosQuery, Result<object>>
{
    public async Task<Result<object>> Handle(FiltrarTreinosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.AlunoId.HasValue)
            {
                var result = await treinoRepository.BuscarTreinosPorAlunoAsync(request.AlunoId.Value, request.Status, cancellationToken);
                if (result.IsFailed) return Result.Fail<object>(result.Errors);
                return Result.Ok((object)result.Value.Select(t => t.ToDto()).ToList());
            }

            if (request.TreinadorId.HasValue)
            {
                var result = await treinoRepository.BuscarTreinosPorTreinadorAsync(request.TreinadorId.Value, request.Status, cancellationToken);
                if (result.IsFailed) return Result.Fail<object>(result.Errors);
                return Result.Ok((object)result.Value.Select(t => t.ToDto()).ToList());
            }

            return Result.Fail<object>("Informe o AlunoId ou TreinadorId para buscar os treinos.");
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao buscar treinos: {ex.Message}");
        }
    }
}
