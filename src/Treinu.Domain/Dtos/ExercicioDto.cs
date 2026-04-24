namespace Treinu.Domain.Dtos;

public record ExercicioDto(
    Guid Id,
    string Nome,
    string Descricao,
    string Tags,
    string? ArquivoDemonstracao,
    Guid TreinadorId,
    DateTime CriadoEm
);
