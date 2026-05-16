namespace Treinu.Domain.Dtos;

public record RankingAlunoDto(
    Guid AlunoId,
    string NomeAluno,
    int TreinosConcluidos,
    int PontuacaoTotal
);
