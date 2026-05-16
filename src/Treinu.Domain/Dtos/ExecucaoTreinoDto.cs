namespace Treinu.Domain.Dtos;

public record ExecucaoTreinoDto(
    Guid Id,
    Guid TreinoId,
    Guid AlunoId,
    DateTime DataInicio,
    DateTime? DataFim,
    bool Concluido,
    int? NotaFeedback,
    string? ComentarioFeedback,
    List<ExecucaoExercicioDto> Exercicios
);

public record ExecucaoExercicioDto(
    Guid Id,
    Guid ItemTreinoId,
    int SeriesRealizadas,
    int RepeticoesRealizadas,
    decimal CargaUtilizada
);
