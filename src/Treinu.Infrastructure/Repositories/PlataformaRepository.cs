using FluentResults;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;
using Treinu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Treinu.Infrastructure.Repositories;

public class PlataformaRepository : IPlataformaRepository
{
    private readonly AppDbContext _context;

    public PlataformaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> AdicionarSugestaoAsync(Sugestao sugestao)
    {
        try
        {
            await _context.Sugestoes.AddAsync(sugestao);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro ao adicionar sugestão: {ex.Message}");
        }
    }

    public async Task<Result> AdicionarAvaliacaoPlataformaAsync(AvaliacaoPlataforma avaliacao)
    {
        try
        {
            await _context.AvaliacoesPlataforma.AddAsync(avaliacao);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro ao adicionar avaliação: {ex.Message}");
        }
    }

    public async Task<Result<List<Treinu.Domain.Dtos.RankingAlunoDto>>> ObterRankingAlunosAsync(Guid? treinadorId)
    {
        var query = _context.Alunos.AsQueryable();

        if (treinadorId.HasValue)
        {
            query = query.Where(a => a.TreinadorId == treinadorId.Value);
        }

        var rankingQuery = query
            .Select(a => new
            {
                a.Id,
                a.NomeCompleto,
                TreinosConcluidos = _context.ExecucoesTreino.Count(e => e.AlunoId == a.Id && e.Concluido)
            })
            .OrderByDescending(x => x.TreinosConcluidos)
            .Take(50);

        var rankingList = await rankingQuery.ToListAsync();

        var ranking = rankingList
            .Select(x => new Treinu.Domain.Dtos.RankingAlunoDto(
                x.Id,
                x.NomeCompleto,
                x.TreinosConcluidos,
                x.TreinosConcluidos * 10
            ))
            .ToList();

        return Result.Ok(ranking);
    }
}
