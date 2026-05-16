using FluentResults;
using Treinu.Domain.Entities;

namespace Treinu.Domain.Repositories;

public interface IPlataformaRepository
{
    Task<Result> AdicionarSugestaoAsync(Sugestao sugestao);
    Task<Result> AdicionarAvaliacaoPlataformaAsync(AvaliacaoPlataforma avaliacao);
    Task<Result<List<Treinu.Domain.Dtos.RankingAlunoDto>>> ObterRankingAlunosAsync(Guid? treinadorId);
}
