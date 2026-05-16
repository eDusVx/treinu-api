using Treinu.Domain.Core.Mediator;
using FluentResults;

namespace Treinu.Contracts.Commands.ExecucoesTreino;

public record RegistrarExercicioExecutadoCommand(Guid ExecucaoTreinoId, Guid ItemTreinoId, int SeriesRealizadas, int RepeticoesRealizadas, decimal CargaUtilizada) : IRequest<Result>;
