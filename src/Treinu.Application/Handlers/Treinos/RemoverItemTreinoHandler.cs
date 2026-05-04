using FluentResults;
using Treinu.Contracts.Commands.Treinos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinos;

public class RemoverItemTreinoHandler(ITreinoRepository treinoRepository) : IRequestHandler<RemoverItemTreinoCommand, Result<object>>
{
    public async Task<Result<object>> Handle(RemoverItemTreinoCommand request, CancellationToken cancellationToken)
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

            var removeResult = treino.RemoverItem(request.ItemId);
            if (removeResult.IsFailed) return Result.Fail<object>(removeResult.Errors);

            var saveResult = await treinoRepository.AtualizarTreinoAsync(treino);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok((object)treino.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao remover item do treino: {ex.Message}");
        }
    }
}
