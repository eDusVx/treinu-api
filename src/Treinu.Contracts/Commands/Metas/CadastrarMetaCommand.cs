using FluentResults;
using System;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;

namespace Treinu.Contracts.Commands.Metas;

public record CadastrarMetaCommand(
    Guid AlunoId,
    TipoMetaEnum Tipo,
    decimal ValorAlvo,
    DateTime DataLimite
) : IRequest<Result<object>>;
