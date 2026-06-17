using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Treinos;

public record AdicionarItemTreinoCommand(
    Guid TreinoId,
    Guid TreinadorId,
    Guid ExercicioId,
    int Series,
    string Repeticoes,
    string Carga,
    string Pausa,
    string Observacoes,
    int Ordem,
    string Divisao
) : IRequest<Result<object>>;
