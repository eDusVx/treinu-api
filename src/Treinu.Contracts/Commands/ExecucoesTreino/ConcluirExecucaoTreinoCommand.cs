using Treinu.Domain.Core.Mediator;
using FluentResults;

namespace Treinu.Contracts.Commands.ExecucoesTreino;

public record ConcluirExecucaoTreinoCommand(Guid ExecucaoTreinoId, int? NotaFeedback, string? ComentarioFeedback) : IRequest<Result>;
