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
    Guid TokenConvite
) : IRequest<Result<object>>;

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