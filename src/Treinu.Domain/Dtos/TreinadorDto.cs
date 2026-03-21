using Treinu.Domain.Enums;

namespace Treinu.Domain.Dtos;

public record TreinadorDto(
    Guid Id,
    string NomeCompleto,
    string Email,
    DateTime DataNascimento,
    GeneroEnum Genero,
    IReadOnlyCollection<Entities.Contato> Contato,
    string Cpf,
    PerfilEnum Perfil,
    bool Ativo,
    bool AceiteTermoAdesao,
    IReadOnlyCollection<Entities.Certificado> Certificados,
    IReadOnlyCollection<string> Especializacoes
);
