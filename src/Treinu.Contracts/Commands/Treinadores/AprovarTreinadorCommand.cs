using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Treinadores;

namespace Treinu.Contracts.Commands.Treinadores;

public record AprovarTreinadorCommand(Guid TreinadorId) : IRequest<Result<Result>>;
