using FluentResults;
using Treinu.Contracts.Commands.Treinos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinos;

public class EditarTreinoHandler(ITreinoRepository treinoRepository) : IRequestHandler<EditarTreinoCommand, Result<object>>
{
    public async Task<Result<object>> Handle(EditarTreinoCommand request, CancellationToken cancellationToken)
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

            var atualizarResult = treino.Atualizar(
                request.Nome,
                request.Descricao,
                request.DataInicio,
                request.DataFim,
                request.NomeDivisaoA,
                request.NomeDivisaoB,
                request.NomeDivisaoC,
                request.NomeDivisaoD,
                request.DivisaoSegunda,
                request.DivisaoTerca,
                request.DivisaoQuarta,
                request.DivisaoQuinta,
                request.DivisaoSexta,
                request.DivisaoSabado,
                request.DivisaoDomingo);
            if (atualizarResult.IsFailed) return Result.Fail<object>(atualizarResult.Errors);

            var saveResult = await treinoRepository.AtualizarTreinoAsync(treino);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok((object)treino.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao editar treino: {ex.Message}");
        }
    }
}
