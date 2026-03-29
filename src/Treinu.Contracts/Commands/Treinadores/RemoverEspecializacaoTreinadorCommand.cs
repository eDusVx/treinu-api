using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Treinadores;

namespace Treinu.Contracts.Commands.Treinadores;

public record RemoverEspecializacaoTreinadorCommand(
    Guid TreinadorId,
    string Especializacao
) : IRequest<Result<object>>;
