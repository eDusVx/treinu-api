using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Queries.ExecucoesTreino;
using Treinu.Domain.Dtos;
using Treinu.Domain.Repositories;
using System.Linq;

namespace Treinu.Application.Handlers.ExecucoesTreino;

public class ObterExecucaoAtivaHandler(IExecucaoTreinoRepository repository)
    : IRequestHandler<ObterExecucaoAtivaQuery, Result<ExecucaoTreinoDto>>
{
    public async Task<Result<ExecucaoTreinoDto>> Handle(ObterExecucaoAtivaQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.BuscarExecucaoAtivaAsync(request.AlunoId, request.TreinoId);
        
        if (result.IsFailed)
            return Result.Fail<ExecucaoTreinoDto>(result.Errors);

        if (result.Value == null)
            return Result.Fail<ExecucaoTreinoDto>("Nenhuma execução ativa encontrada para este treino.");

        var e = result.Value;
        var dto = new ExecucaoTreinoDto(
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
        );

        return Result.Ok(dto);
    }
}
