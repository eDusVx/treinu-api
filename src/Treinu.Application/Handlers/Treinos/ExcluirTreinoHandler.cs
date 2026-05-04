using FluentResults;
using Treinu.Contracts.Commands.Treinos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinos;

public class ExcluirTreinoHandler(ITreinoRepository treinoRepository) : IRequestHandler<ExcluirTreinoCommand, Result<object>>
{
    public async Task<Result<object>> Handle(ExcluirTreinoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var treinoResult = await treinoRepository.BuscarTreinoPorIdAsync(request.TreinoId, cancellationToken);
            if (treinoResult.IsFailed) return Result.Fail<object>("Treino não encontrado.");

            var treino = treinoResult.Value;

            if (treino.TreinadorId != request.TreinadorId)
            {
                return Result.Fail<object>("Você não tem permissão para excluir este treino.");
            }

            var deleteResult = await treinoRepository.ExcluirTreinoAsync(treino);
            if (deleteResult.IsFailed) return Result.Fail<object>(deleteResult.Errors);

            return Result.Ok((object)new { Message = "Treino excluído com sucesso." });
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao excluir treino: {ex.Message}");
        }
    }
}
