namespace Treinu.Domain.Dtos;

public record FeedbackTreinoDto(
    Guid ExecucaoTreinoId,
    Guid AlunoId,
    string NomeAluno,
    string NomeTreino,
    int? Nota,
    string? Comentario,
    DateTime DataFim
);
