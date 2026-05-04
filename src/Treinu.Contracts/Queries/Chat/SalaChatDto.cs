namespace Treinu.Contracts.Queries.Chat;

public record SalaChatDto(
    Guid Id,
    string Nome,
    string Tipo,
    Guid? CriadorId,
    int NaoLidas,
    int TotalParticipantes
);
