using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.ExecucoesTreino;
using Treinu.Domain.Repositories;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;

namespace Treinu.Application.Handlers.ExecucoesTreino;

public class ConcluirExecucaoTreinoHandler(
    IExecucaoTreinoRepository execucaoTreinoRepository,
    ITreinoRepository treinoRepository,
    ITelemetriaRepository telemetriaRepository)
    : IRequestHandler<ConcluirExecucaoTreinoCommand, Result>
{
    public async Task<Result> Handle(ConcluirExecucaoTreinoCommand request, CancellationToken cancellationToken)
    {
        var execucaoResult = await execucaoTreinoRepository.BuscarPorIdAsync(request.ExecucaoTreinoId);
        
        if (execucaoResult.IsFailed)
            return Result.Fail(execucaoResult.Errors);

        var execucao = execucaoResult.Value;

        var concluirResult = execucao.Concluir(request.NotaFeedback, request.ComentarioFeedback);

        if (concluirResult.IsFailed)
            return Result.Fail(concluirResult.Errors);

        var updateResult = await execucaoTreinoRepository.AtualizarExecucaoAsync(execucao);
        
        if (updateResult.IsFailed)
            return Result.Fail(updateResult.Errors);

        var treinoResult = await treinoRepository.BuscarTreinoPorIdAsync(execucao.TreinoId, cancellationToken);
        if (treinoResult.IsSuccess && treinoResult.Value != null)
        {
            var treino = treinoResult.Value;
            var concluirTreinoResult = treino.Concluir();
            if (concluirTreinoResult.IsSuccess)
            {
                await treinoRepository.AtualizarTreinoAsync(treino);
            }
        }

        // Log telemetry event
        var workoutEvent = EventoTelemetria.Criar(execucao.AlunoId, TipoInteracaoEnum.SUBMIT_EXECUCAO_TREINO);
        await telemetriaRepository.RegistrarEventoAsync(workoutEvent, cancellationToken);

        return Result.Ok();
    }
}
