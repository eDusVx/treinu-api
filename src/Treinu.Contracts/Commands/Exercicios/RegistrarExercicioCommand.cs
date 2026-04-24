using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Exercicios;

public record RegistrarExercicioCommand(
    string Nome,
    string Descricao,
    string Tags,
    string? ArquivoDemonstracao,
    Guid TreinadorId
) : IRequest<Result<object>>;
