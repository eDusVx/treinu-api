using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Queries.ExecucoesTreino;
using Treinu.Domain.Dtos;
using Treinu.Domain.Repositories;
using System.Linq;

namespace Treinu.Application.Handlers.ExecucoesTreino;

public class ObterHistoricoExecucoesHandler(IExecucaoTreinoRepository repository)
    : IRequestHandler<ObterHistoricoExecucoesQuery, Result<List<ExecucaoTreinoDto>>>
{
    public async Task<Result<List<ExecucaoTreinoDto>>> Handle(ObterHistoricoExecucoesQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.BuscarPorAlunoAsync(request.AlunoId);
        
        if (result.IsFailed)
            return Result.Fail<List<ExecucaoTreinoDto>>(result.Errors);

        var dtos = result.Value.Select(e => new ExecucaoTreinoDto(
            e.Id,
            e.TreinoId,
            e.AlunoId,
            e.DataInicio,
            e.DataFim,
            e.Concluido,
            e.NotaFeedback,
            e.ComentarioFeedback,
            e.Exercicios.Select(ex => new ExecucaoExercicioDto(
                ex.Id, ex.ItemTreinoId, ex.SeriesRealizadas, ex.RepeticoesRealizadas, ex.CargaUtilizada
            )).ToList()
        )).OrderByDescending(e => e.DataInicio).ToList();

        return Result.Ok(dtos);
    }
}
