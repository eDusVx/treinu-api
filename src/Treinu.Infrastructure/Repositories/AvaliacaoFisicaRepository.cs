using FluentResults;
using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities.AvaliacaoFisica;
using Treinu.Domain.Errors;
using Treinu.Domain.Repositories;
using Treinu.Infrastructure.Data;

namespace Treinu.Infrastructure.Repositories;

public class AvaliacaoFisicaRepository : IAvaliacaoFisicaRepository
{
    private readonly AppDbContext _context;

    public AvaliacaoFisicaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> AdicionarAvaliacaoFisicaAsync(AvaliacaoFisica avaliacao, Guid alunoId, CancellationToken cancellationToken = default)
    {
        try
        {
            var alunoExiste = await _context.Alunos.AnyAsync(a => a.Id == alunoId, cancellationToken);
            if (!alunoExiste)
                return Result.Fail(DomainErrors.Usuario.AlunoNaoEncontrado);

            _context.Entry(avaliacao).Property("AlunoId").CurrentValue = alunoId;
            await _context.AvaliacoesFisicas.AddAsync(avaliacao, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro inesperado ao salvar avaliação física: {ex.Message}");
        }
    }

    public async Task<Result> RemoverAvaliacaoFisicaAsync(Guid avaliacaoId, Guid alunoId, CancellationToken cancellationToken = default)
    {
        try
        {
            var avaliacao = await _context.AvaliacoesFisicas
                .Include(a => a.Medidas)
                .FirstOrDefaultAsync(a => a.Id == avaliacaoId && EF.Property<Guid?>(a, "AlunoId") == alunoId, cancellationToken);

            if (avaliacao == null)
                return Result.Fail(DomainErrors.Usuario.AvaliacaoFisicaNaoEncontrada);

            _context.AvaliacoesFisicas.Remove(avaliacao);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro inesperado ao remover avaliação física: {ex.Message}");
        }
    }
}
