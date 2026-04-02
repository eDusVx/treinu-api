using Treinu.Domain.Enums;

namespace Treinu.Domain.Dtos;

public record ContatoDto(
    TipoContatoEnum Tipo,
    string Valor,
    string? Descricao,
    bool Principal,
    PlataformaRedeSocialEnum? Plataforma,
    string? NomeExibicao
);