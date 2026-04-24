using FluentResults;
using Treinu.Contracts.Commands.Exercicios;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Exercicios;

public class RegistrarExercicioHandler(
    IExercicioRepository exercicioRepository,
    IUsuarioRepository usuarioRepository)
    : IRequestHandler<RegistrarExercicioCommand, Result<object>>
{
    public async Task<Result<object>> Handle(RegistrarExercicioCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var treinadorResult = await usuarioRepository.BuscarTreinadorPorIdAsync(request.TreinadorId, cancellationToken);
            if (treinadorResult.IsFailed) return Result.Fail<object>("Treinador não encontrado.");

            var props = new CriarExercicioProps(
                request.Nome,
                request.Descricao,
                request.Tags,
                request.ArquivoDemonstracao,
                request.TreinadorId
            );

            var exercicioResult = Exercicio.Criar(props);
            if (exercicioResult.IsFailed) return Result.Fail<object>(exercicioResult.Errors);

            var saveResult = await exercicioRepository.AdicionarExercicioAsync(exercicioResult.Value);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok((object)exercicioResult.Value.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao registrar exercício: {ex.Message}");
        }
    }
}
