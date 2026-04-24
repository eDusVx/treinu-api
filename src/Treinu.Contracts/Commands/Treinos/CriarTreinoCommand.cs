using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Treinos;

public record CriarItemTreinoCommand(
    Guid ExercicioId,
    int Series,
    string Repeticoes,
    string Carga,
    string Pausa,
    string Observacoes,
    int Ordem
);

public record CriarTreinoCommand(
    string Nome,
    string Descricao,
    DateTime DataInicio,
    DateTime DataFim,
    Guid TreinadorId,
    Guid AlunoId,
    List<CriarItemTreinoCommand> Itens
) : IRequest<Result<object>>;
