namespace Treinu.Domain.Dtos;

public record ItemTreinoDto(
    Guid Id,
    Guid ExercicioId,
    ExercicioDto? Exercicio,
    int Series,
    string Repeticoes,
    string Carga,
    string Pausa,
    string Observacoes,
    int Ordem
);
