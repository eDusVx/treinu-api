using FluentResults;
using Treinu.Domain.Entities;

namespace Treinu.Domain.Repositories;

public interface IExecucaoTreinoRepository
{
    Task<Result> AdicionarExecucaoAsync(ExecucaoTreino execucao);
    Task<Result> AtualizarExecucaoAsync(ExecucaoTreino execucao);
    Task<Result<ExecucaoTreino>> BuscarPorIdAsync(Guid id);
    Task<Result<List<ExecucaoTreino>>> BuscarPorAlunoAsync(Guid alunoId);
    Task<Result<ExecucaoTreino?>> BuscarExecucaoAtivaAsync(Guid alunoId, Guid treinoId);
}
