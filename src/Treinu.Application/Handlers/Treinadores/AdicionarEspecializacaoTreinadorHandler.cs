using FluentResults;
using Treinu.Contracts.Commands;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinadores;

public class AdicionarEspecializacaoTreinadorHandler(
    IUsuarioRepository usuarioRepository) : IRequestHandler<AdicionarEspecializacaoTreinadorCommand, Result<object>>
{
    public async Task<Result<object>> Handle(AdicionarEspecializacaoTreinadorCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var treinadorResult =
                await usuarioRepository.BuscarTreinadorPorIdAsync(request.TreinadorId, cancellationToken);
            if (treinadorResult.IsFailed) return Result.Fail<object>(treinadorResult.Errors);

            var addResult = treinadorResult.Value.AdicionarEspecializacao(request.Especializacao);
            if (addResult.IsFailed) return Result.Fail<object>(addResult.Errors);

            var saveResult = await usuarioRepository.AtualizarUsuarioAsync(treinadorResult.Value);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok(treinadorResult.Value.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao adicionar especialização: {ex.Message}");
        }
    }
}