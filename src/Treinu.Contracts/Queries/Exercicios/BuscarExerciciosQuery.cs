using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Queries.Exercicios;

public record BuscarExerciciosQuery(
    Guid TreinadorId,
    string? Tags
) : IRequest<Result<object>>;
