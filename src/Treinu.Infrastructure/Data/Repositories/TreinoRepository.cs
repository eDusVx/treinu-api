using FluentResults;
using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;

namespace Treinu.Infrastructure.Data.Repositories;

public class TreinoRepository : ITreinoRepository
{
    private readonly AppDbContext _context;

    public TreinoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Treino>> BuscarTreinoPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var treino = await _context.Treinos
            .Include(t => t.Itens)
                .ThenInclude(i => i.Exercicio)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (treino == null) return Result.Fail("Treino não encontrado.");
        return Result.Ok(treino);
    }

    public async Task<Result<List<Treino>>> BuscarTreinosPorAlunoAsync(Guid alunoId, TreinoStatusEnum? status, CancellationToken cancellationToken = default)
    {
        var query = _context.Treinos
            .Include(t => t.Itens)
                .ThenInclude(i => i.Exercicio)
            .Where(t => t.AlunoId == alunoId);

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        var list = await query.OrderByDescending(t => t.DataInicio).ToListAsync(cancellationToken);
        return Result.Ok(list);
    }

    public async Task<Result<List<Treino>>> BuscarTreinosPorTreinadorAsync(Guid treinadorId, TreinoStatusEnum? status, CancellationToken cancellationToken = default)
    {
        var query = _context.Treinos
            .Include(t => t.Itens)
                .ThenInclude(i => i.Exercicio)
            .Where(t => t.TreinadorId == treinadorId);

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        var list = await query.OrderByDescending(t => t.DataInicio).ToListAsync(cancellationToken);
        return Result.Ok(list);
    }

    public async Task<Result<List<Treino>>> BuscarTreinosVencidosAtivosAsync(DateTime dataReferencia, CancellationToken cancellationToken = default)
    {
        var dateOnly = dataReferencia.Date;
        var list = await _context.Treinos
            .Where(t => t.DataFim.Date < dateOnly && t.Status != TreinoStatusEnum.VENCIDO)
            .ToListAsync(cancellationToken);

        return Result.Ok(list);
    }

    public async Task<Result> AdicionarTreinoAsync(Treino treino)
    {
        await _context.Treinos.AddAsync(treino);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> AtualizarTreinoAsync(Treino treino)
    {
        _context.Treinos.Update(treino);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
}
