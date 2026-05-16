using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.ExecucoesTreino;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.ExecucoesTreino;

public class ConcluirExecucaoTreinoHandler(IExecucaoTreinoRepository execucaoTreinoRepository)
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

        return Result.Ok();
    }
}
