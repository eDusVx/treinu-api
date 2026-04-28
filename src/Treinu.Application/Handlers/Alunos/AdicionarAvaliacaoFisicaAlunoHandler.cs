using FluentResults;
using Treinu.Contracts.Commands.Alunos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Factories;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Alunos;

public class AdicionarAvaliacaoFisicaAlunoHandler(
    IUsuarioRepository usuarioRepository,
    IAvaliacaoFisicaRepository avaliacaoFisicaRepository,
    AvaliacaoFisicaFactory avaliacaoFisicaFactory)
    : IRequestHandler<AdicionarAvaliacaoFisicaAlunoCommand, Result<object>>
{
    public async Task<Result<object>> Handle(AdicionarAvaliacaoFisicaAlunoCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var alunoResult = await usuarioRepository.BuscarAlunoPorIdAsync(request.AlunoId, cancellationToken);
            if (alunoResult.IsFailed) return Result.Fail<object>(alunoResult.Errors);

            var avaliacoesResult = avaliacaoFisicaFactory.Fabricar([request.AvaliacaoFisica]);
            if (avaliacoesResult.IsFailed) return Result.Fail<object>(avaliacoesResult.Errors);

            var avaliacao = avaliacoesResult.Value.First();

            var saveResult = await avaliacaoFisicaRepository.AdicionarAvaliacaoFisicaAsync(avaliacao, request.AlunoId, cancellationToken);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            // Recarrega o aluno com as avaliações atualizadas para retornar o DTO completo
            var alunoAtualizado = await usuarioRepository.BuscarAlunoPorIdAsync(request.AlunoId, cancellationToken);
            if (alunoAtualizado.IsFailed) return Result.Fail<object>(alunoAtualizado.Errors);

            return Result.Ok(alunoAtualizado.Value.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao adicionar avaliação física: {ex.Message}");
        }
    }
}