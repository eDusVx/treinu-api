using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.ExecucoesTreino;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.ExecucoesTreino;

public class IniciarExecucaoTreinoHandler(IExecucaoTreinoRepository execucaoTreinoRepository)
    : IRequestHandler<IniciarExecucaoTreinoCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(IniciarExecucaoTreinoCommand request, CancellationToken cancellationToken)
    {
        var execucaoResult = ExecucaoTreino.Iniciar(request.TreinoId, request.AlunoId);
        
        if (execucaoResult.IsFailed)
            return Result.Fail<Guid>(execucaoResult.Errors);

        var addResult = await execucaoTreinoRepository.AdicionarExecucaoAsync(execucaoResult.Value);
        
        if (addResult.IsFailed)
            return Result.Fail<Guid>(addResult.Errors);

        return Result.Ok(execucaoResult.Value.Id);
    }
}
