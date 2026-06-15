using System.Text.Json.Serialization;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Dtos;

public record MedidaDto(
    ChaveMedidaEnum Chave,
    decimal Valor
);

public record AvaliacaoFisicaDto
{
    public DateTime Data { get; init; }
    public double Altura { get; init; }
    public double Peso { get; init; }
    public List<MedidaDto> Medidas { get; init; } = new();

    [JsonPropertyName("gorduraCorporal")]
    public double? GorduraCorporal { get; init; }

    [JsonPropertyName("gorduraCorpora")]
    public double? GorduraCorpora { get; init; }

    [JsonIgnore]
    public double? PercentualGordura => GorduraCorporal ?? GorduraCorpora;
}