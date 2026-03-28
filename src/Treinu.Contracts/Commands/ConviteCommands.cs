using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands;

public record ConvidarTreinadorCommand(string Email) : IRequest<Result>;

public record ConvidarAlunoCommand(string Email, Guid TreinadorId) : IRequest<Result>;