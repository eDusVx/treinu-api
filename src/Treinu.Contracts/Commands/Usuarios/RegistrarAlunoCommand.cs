using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;
using Treinu.Contracts.Commands.Usuarios;

namespace Treinu.Contracts.Commands.Usuarios;

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
