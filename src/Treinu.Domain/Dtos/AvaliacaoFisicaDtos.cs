using System.Text.Json.Serialization;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Dtos;

public record MedidaDto(
    ChaveMedidaEnum Chave,
    decimal Valor
);

[JsonPolymorphic(TypeDiscriminatorPropertyName = "tipoAvaliacao")]
[JsonDerivedType(typeof(DocumentoDto), "DOCUMENTO")]
[JsonDerivedType(typeof(QuestionarioDto), "QUESTIONARIO")]
public abstract record AvaliacaoFisicaDto
{
    [JsonIgnore]
    public abstract TipoAvaliacaoEnum ContextoAvaliacao { get; }
    public DateTime Data { get; init; }
}

public record DocumentoDto : AvaliacaoFisicaDto
{
    [JsonIgnore]
    public override TipoAvaliacaoEnum ContextoAvaliacao => TipoAvaliacaoEnum.DOCUMENTO;
    public string Nome { get; init; } = string.Empty;
    public string Arquivo { get; init; } = string.Empty;
}

public record QuestionarioDto : AvaliacaoFisicaDto
{
    [JsonIgnore]
    public override TipoAvaliacaoEnum ContextoAvaliacao => TipoAvaliacaoEnum.QUESTIONARIO;
    public double Altura { get; init; }
    public double Peso { get; init; }
    public List<MedidaDto> Medidas { get; init; } = new();
}

