namespace Treinu.Domain.Dtos;

public record TreinoDto(
    Guid Id,
    string Nome,
    string Descricao,
    DateTime DataInicio,
    DateTime DataFim,
    string Status,
    Guid TreinadorId,
    Guid AlunoId,
    List<ItemTreinoDto> Itens
);
