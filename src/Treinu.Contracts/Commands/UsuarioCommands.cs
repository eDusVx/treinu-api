using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;

namespace Treinu.Contracts.Commands;

public record RegistrarTreinadorCommand(
    string NomeCompleto,
    string Email,
    string Senha,
    DateTime DataNascimento,
    GeneroEnum Genero,
    string Cpf,
    bool AceiteTermoAdesao,
    List<CertificadoDto> Certificados
) : IRequest<Result<object>>;

public record CertificadoDto(string Nome, string ArquivoPdf);

public record RegistrarAlunoCommand(
    string NomeCompleto,
    string Email,
    string Senha,
    DateTime DataNascimento,
    GeneroEnum Genero,
    string Cpf,
    bool AceiteTermoAdesao,
    ObjetivoEnum Objetivo,
    Guid TokenConvite
) : IRequest<Result<object>>;