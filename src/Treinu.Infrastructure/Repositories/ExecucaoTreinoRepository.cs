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

    public async Task<Result<List<Treinu.Domain.Dtos.FeedbackTreinoDto>>> BuscarFeedbacksPorTreinadorAsync(Guid treinadorId)
    {
        var feedbacks = await _context.ExecucoesTreino
            .Where(e => e.Concluido && (e.NotaFeedback != null || !string.IsNullOrEmpty(e.ComentarioFeedback)))
            .Join(_context.Treinos, e => e.TreinoId, t => t.Id, (e, t) => new { e, t })
            .Where(x => x.t.TreinadorId == treinadorId)
            .OrderByDescending(x => x.e.DataFim)
            .Select(x => new Treinu.Domain.Dtos.FeedbackTreinoDto(
                x.e.Id,
                x.e.AlunoId,
                x.t.Aluno!.NomeCompleto,
                x.t.Nome,
                x.e.NotaFeedback,
                x.e.ComentarioFeedback,
                x.e.DataFim.Value
            ))
            .ToListAsync();

        return Result.Ok(feedbacks);
    }

    public async Task<Result<List<ExecucaoTreino>>> BuscarExecucoesFiltradoAsync(Guid? treinadorId, Guid? alunoId, DateTime? inicio, DateTime? fim, CancellationToken cancellationToken = default)
    {
        var query = _context.ExecucoesTreino.AsQueryable();

        if (alunoId.HasValue)
        {
            query = query.Where(e => e.AlunoId == alunoId.Value);
        }
        else if (treinadorId.HasValue)
        {
            query = query.Where(e => _context.Alunos.Any(a => a.Id == e.AlunoId && a.TreinadorId == treinadorId.Value));
        }

        if (inicio.HasValue)
        {
            var inicioUtc = DateTime.SpecifyKind(inicio.Value, DateTimeKind.Utc);
            query = query.Where(e => e.DataInicio >= inicioUtc);
        }

        if (fim.HasValue)
        {
            var fimUtc = DateTime.SpecifyKind(fim.Value, DateTimeKind.Utc);
            query = query.Where(e => e.DataInicio <= fimUtc);
        }

        var list = await query.ToListAsync(cancellationToken);
        return Result.Ok(list);
    }
}
