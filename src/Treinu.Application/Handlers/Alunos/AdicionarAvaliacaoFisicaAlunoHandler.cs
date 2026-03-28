using FluentResults;
using Treinu.Contracts.Commands;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Factories;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Alunos;

public class AdicionarAvaliacaoFisicaAlunoHandler(
    IUsuarioRepository usuarioRepository,
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

            var addResult = alunoResult.Value.AdicionarAvaliacaoFisica(avaliacao);
            if (addResult.IsFailed) return Result.Fail<object>(addResult.Errors);

            var saveResult = await usuarioRepository.AtualizarUsuarioAsync(alunoResult.Value);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok(alunoResult.Value.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao adicionar avaliação física: {ex.Message}");
        }
    }
}