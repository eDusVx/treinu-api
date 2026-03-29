using FluentResults;
using Treinu.Domain.Core.Mediator;
using Treinu.Contracts.Commands.Convites;

namespace Treinu.Contracts.Commands.Convites;

public record ConvidarTreinadorCommand(string Email) : IRequest<Result>;
