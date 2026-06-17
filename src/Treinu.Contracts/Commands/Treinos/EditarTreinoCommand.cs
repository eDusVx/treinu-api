using FluentResults;
using Treinu.Domain.Core.Mediator;

namespace Treinu.Contracts.Commands.Treinos;

public record EditarTreinoCommand(
    Guid TreinoId,
    string Nome,
    string Descricao,
    DateTime DataInicio,
    DateTime DataFim,
    Guid TreinadorId,
    string? NomeDivisaoA,
    string? NomeDivisaoB,
    string? NomeDivisaoC,
    string? NomeDivisaoD,
    string? DivisaoSegunda,
    string? DivisaoTerca,
    string? DivisaoQuarta,
    string? DivisaoQuinta,
    string? DivisaoSexta,
    string? DivisaoSabado,
    string? DivisaoDomingo
) : IRequest<Result<object>>;
