using FluentResults;
using Treinu.Contracts.Queries.Exercicios;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Exercicios;

public class BuscarExerciciosHandler(
    IExercicioRepository exercicioRepository)
    : IRequestHandler<BuscarExerciciosQuery, Result<object>>
{
    public async Task<Result<object>> Handle(BuscarExerciciosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await exercicioRepository.BuscarExerciciosPorTreinadorAsync(request.TreinadorId, request.Tags, cancellationToken);
            if (result.IsFailed) return Result.Fail<object>(result.Errors);

            var dtos = result.Value.Select(e => e.ToDto()).ToList();
            return Result.Ok((object)dtos);
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao buscar exercícios: {ex.Message}");
        }
    }
}
