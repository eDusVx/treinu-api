using FluentResults;
using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;
using Treinu.Infrastructure.Data;

namespace Treinu.Infrastructure.Repositories;

public class MetaRepository : IMetaRepository
{
    private readonly AppDbContext _context;

    public MetaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> SalvarMetaAsync(Meta meta, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Metas.AddAsync(meta, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro ao salvar meta: {ex.Message}");
        }
    }

    public async Task<List<Meta>> BuscarMetasPorAlunoAsync(Guid alunoId, CancellationToken cancellationToken = default)
    {
        return await _context.Metas
            .Where(m => m.AlunoId == alunoId)
            .OrderByDescending(m => m.DataCriacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<Meta?> BuscarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Metas.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Result> AtualizarMetaAsync(Meta meta, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Metas.Update(meta);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro ao atualizar meta: {ex.Message}");
        }
    }
}
