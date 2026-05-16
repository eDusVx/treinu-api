using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.ExecucoesTreino;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.ExecucoesTreino;

public class RegistrarExercicioExecutadoHandler(IExecucaoTreinoRepository execucaoTreinoRepository)
    : IRequestHandler<RegistrarExercicioExecutadoCommand, Result>
{
    public async Task<Result> Handle(RegistrarExercicioExecutadoCommand request, CancellationToken cancellationToken)
    {
        var execucaoResult = await execucaoTreinoRepository.BuscarPorIdAsync(request.ExecucaoTreinoId);
        
        if (execucaoResult.IsFailed)
            return Result.Fail(execucaoResult.Errors);

        var execucao = execucaoResult.Value;

        var registrarResult = execucao.RegistrarExercicio(
            request.ItemTreinoId, 
            request.SeriesRealizadas, 
            request.RepeticoesRealizadas, 
            request.CargaUtilizada);

        if (registrarResult.IsFailed)
            return Result.Fail(registrarResult.Errors);

        var updateResult = await execucaoTreinoRepository.AtualizarExecucaoAsync(execucao);
        
        if (updateResult.IsFailed)
            return Result.Fail(updateResult.Errors);

        return Result.Ok();
    }
}
