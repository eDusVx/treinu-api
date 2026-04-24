using FluentResults;
using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Infrastructure.Data.Repositories;

public class ExercicioRepository : IExercicioRepository
{
    private readonly AppDbContext _context;

    public ExercicioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Exercicio>> BuscarExercicioPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exercicio = await _context.Exercicios.FindAsync(new object[] { id }, cancellationToken);
        if (exercicio == null) return Result.Fail("Exercício não encontrado.");
        return Result.Ok(exercicio);
    }

    public async Task<Result<List<Exercicio>>> BuscarExerciciosPorTreinadorAsync(Guid treinadorId, string? tags, CancellationToken cancellationToken = default)
    {
        var query = _context.Exercicios.Where(e => e.TreinadorId == treinadorId);

        if (!string.IsNullOrWhiteSpace(tags))
        {
            var upperTags = tags.ToUpper();
            query = query.Where(e => e.Tags.ToUpper().Contains(upperTags));
        }

        var list = await query.OrderBy(e => e.Nome).ToListAsync(cancellationToken);
        return Result.Ok(list);
    }

    public async Task<Result> AdicionarExercicioAsync(Exercicio exercicio)
    {
        await _context.Exercicios.AddAsync(exercicio);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
}
