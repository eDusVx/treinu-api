using Treinu.Domain.Entities;
using Treinu.Domain.Entities.AvaliacaoFisica;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Dtos;

public record AlunoDto(
    Guid Id,
    string NomeCompleto,
    string Email,
    DateTime DataNascimento,
    GeneroEnum Genero,
    IReadOnlyCollection<Contato> Contato,
    string Cpf,
    PerfilEnum Perfil,
    bool Ativo,
    bool AceiteTermoAdesao,
    ObjetivoEnum Objetivo,
    IReadOnlyCollection<Treinu.Domain.Entities.AvaliacaoFisica.AvaliacaoFisica> AvaliacaoFisica
);