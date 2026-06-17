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
    List<ItemTreinoDto> Itens,
    string? NomeDivisaoA,
    string? NomeDivisaoB,
    string? NomeDivisaoC,
    string? NomeDivisaoD,
    string? DivisaoSegunda,
    string? DivisaoTerca,
    string? DivisaoQuarta,
    string? DivisaoQuinta,
    string? DivisaoSexta,
    string? DivisaoSabado,
    string? DivisaoDomingo
);
