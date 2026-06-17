using FluentResults;
using Treinu.Contracts.Commands.Treinos;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinos;

public class CriarTreinoHandler(
    ITreinoRepository treinoRepository,
    IUsuarioRepository usuarioRepository)
    : IRequestHandler<CriarTreinoCommand, Result<object>>
{
    public async Task<Result<object>> Handle(CriarTreinoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var alunoResult = await usuarioRepository.BuscarAlunoPorIdAsync(request.AlunoId, cancellationToken);
            if (alunoResult.IsFailed) return Result.Fail<object>("Aluno não encontrado.");

            var treinadorResult = await usuarioRepository.BuscarTreinadorPorIdAsync(request.TreinadorId, cancellationToken);
            if (treinadorResult.IsFailed) return Result.Fail<object>("Treinador não encontrado.");

            var itensProps = request.Itens.Select(i => new CriarItemTreinoProps(
                i.ExercicioId,
                i.Series,
                i.Repeticoes,
                i.Carga,
                i.Pausa,
                i.Observacoes,
                i.Ordem,
                i.Divisao
            )).ToList();

            var props = new CriarTreinoProps(
                request.Nome,
                request.Descricao,
                request.DataInicio,
                request.DataFim,
                request.TreinadorId,
                request.AlunoId,
                itensProps,
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
                request.DivisaoDomingo
            );

            var treinoResult = Treino.Criar(props);
            if (treinoResult.IsFailed) return Result.Fail<object>(treinoResult.Errors);

            var saveResult = await treinoRepository.AdicionarTreinoAsync(treinoResult.Value);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok((object)treinoResult.Value.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao cadastrar treino: {ex.Message}");
        }
    }
}
