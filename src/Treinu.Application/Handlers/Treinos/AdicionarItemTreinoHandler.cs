using FluentResults;
using Treinu.Contracts.Commands.Treinos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinos;

public class AdicionarItemTreinoHandler(ITreinoRepository treinoRepository) : IRequestHandler<AdicionarItemTreinoCommand, Result<object>>
{
    public async Task<Result<object>> Handle(AdicionarItemTreinoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var treinoResult = await treinoRepository.BuscarTreinoPorIdAsync(request.TreinoId, cancellationToken);
            if (treinoResult.IsFailed) return Result.Fail<object>("Treino não encontrado.");

            var treino = treinoResult.Value;

            if (treino.TreinadorId != request.TreinadorId)
            {
                return Result.Fail<object>("Você não tem permissão para editar este treino.");
            }

            var props = new CriarItemTreinoProps(
                request.ExercicioId,
                request.Series,
                request.Repeticoes,
                request.Carga,
                request.Pausa,
                request.Observacoes,
                request.Ordem
            );

            var addResult = treino.AdicionarItem(props);
            if (addResult.IsFailed) return Result.Fail<object>(addResult.Errors);

            var saveResult = await treinoRepository.AtualizarTreinoAsync(treino);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok((object)treino.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao adicionar item ao treino: {ex.Message}");
        }
    }
}
