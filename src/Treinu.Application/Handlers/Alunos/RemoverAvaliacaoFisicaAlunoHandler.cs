using FluentResults;
using Treinu.Contracts.Commands.Alunos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Alunos;

public class RemoverAvaliacaoFisicaAlunoHandler(
    IUsuarioRepository usuarioRepository,
    IAvaliacaoFisicaRepository avaliacaoFisicaRepository) : IRequestHandler<RemoverAvaliacaoFisicaAlunoCommand, Result<object>>
{
    public async Task<Result<object>> Handle(RemoverAvaliacaoFisicaAlunoCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var alunoResult = await usuarioRepository.BuscarAlunoPorIdAsync(request.AlunoId, cancellationToken);
            if (alunoResult.IsFailed) return Result.Fail<object>(alunoResult.Errors);

            var removeResult = await avaliacaoFisicaRepository.RemoverAvaliacaoFisicaAsync(request.AvaliacaoFisicaId, request.AlunoId, cancellationToken);
            if (removeResult.IsFailed) return Result.Fail<object>(removeResult.Errors);

            return Result.Ok<object>("Avaliação física removida com sucesso.");
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao remover avaliação física: {ex.Message}");
        }
    }
}