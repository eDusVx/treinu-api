using Treinu.Domain.Core.Mediator;
using FluentResults;
using Treinu.Domain.Dtos;

namespace Treinu.Contracts.Queries.Plataforma;

public record ObterRankingAlunosQuery(Guid? TreinadorId) : IRequest<Result<List<RankingAlunoDto>>>;
