using FluentResults;
using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;
using Treinu.Infrastructure.Data;

namespace Treinu.Infrastructure.Repositories;

public class ExecucaoTreinoRepository : IExecucaoTreinoRepository
{
    private readonly AppDbContext _context;

    public ExecucaoTreinoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> AdicionarExecucaoAsync(ExecucaoTreino execucao)
    {
        try
        {
            await _context.ExecucoesTreino.AddAsync(execucao);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro ao adicionar execução de treino: {ex.Message}");
        }
    }

    public async Task<Result> AtualizarExecucaoAsync(ExecucaoTreino execucao)
    {
        try
        {
            _context.ExecucoesTreino.Update(execucao);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro ao atualizar execução de treino: {ex.Message}");
        }
    }

    public async Task<Result<ExecucaoTreino>> BuscarPorIdAsync(Guid id)
    {
        var execucao = await _context.ExecucoesTreino
            .Include(x => x.Exercicios)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (execucao == null)
            return Result.Fail("Execução de treino não encontrada.");

        return Result.Ok(execucao);
    }

    public async Task<Result<List<ExecucaoTreino>>> BuscarPorAlunoAsync(Guid alunoId)
    {
        var execucoes = await _context.ExecucoesTreino
            .Include(x => x.Exercicios)
            .Where(x => x.AlunoId == alunoId)
            .ToListAsync();

        return Result.Ok(execucoes);
    }

    public async Task<Result<ExecucaoTreino?>> BuscarExecucaoAtivaAsync(Guid alunoId, Guid treinoId)
    {
        var execucao = await _context.ExecucoesTreino
            .Include(x => x.Exercicios)
            .Where(x => x.AlunoId == alunoId && x.TreinoId == treinoId && !x.Concluido)
            .OrderByDescending(x => x.DataInicio)
            .FirstOrDefaultAsync();

        return Result.Ok(execucao);
    }
}
