using Treinu.Domain.Entities;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Dtos;

public record TreinadorDto(
    Guid Id,
    string NomeCompleto,
    string Email,
    DateTime DataNascimento,
    GeneroEnum Genero,
    IReadOnlyCollection<ContatoDto> Contato,
    string Cpf,
    PerfilEnum Perfil,
    bool Ativo,
    bool AceiteTermoAdesao,
    string Cref,
    IReadOnlyCollection<string> Especializacoes
);