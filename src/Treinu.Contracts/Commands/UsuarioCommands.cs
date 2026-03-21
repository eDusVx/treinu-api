using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Dtos;
using Treinu.Domain.Enums;

namespace Treinu.Contracts.Commands;

public record RegistrarUsuarioCommand(
    string NomeCompleto,
    string Email,
    string Senha,
    DateTime DataNascimento,
    GeneroEnum Genero,
    string Cpf,
    bool Ativo,
    bool AceiteTermoAdesao,
    PerfilEnum TipoUsuario,
    List<ContatoDto> Contatos,
    ObjetivoEnum? Objetivo = null,
    List<AvaliacaoFisicaDto>? AvaliacoesFisicas = null,
    List<CertificadoDto>? Certificados = null,
    List<string>? Especializacoes = null
) : IRequest<Result<object>>;