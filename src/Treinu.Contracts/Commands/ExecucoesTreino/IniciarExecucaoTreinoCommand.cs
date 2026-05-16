using Treinu.Domain.Core.Mediator;
using FluentResults;

namespace Treinu.Contracts.Commands.ExecucoesTreino;

public record IniciarExecucaoTreinoCommand(Guid TreinoId, Guid AlunoId) : IRequest<Result<Guid>>;
