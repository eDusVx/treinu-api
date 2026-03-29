using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Treinadores;

namespace Treinu.Contracts.Commands.Treinadores;

public record AdicionarEspecializacaoTreinadorCommand(
    Guid TreinadorId,
    string Especializacao
) : IRequest<Result<object>>;
