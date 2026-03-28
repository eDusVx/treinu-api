using FluentResults;
using Treinu.Contracts.Commands;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinadores;

public class RemoverContatoTreinadorHandler(
    IUsuarioRepository usuarioRepository) : IRequestHandler<RemoverContatoTreinadorCommand, Result<object>>
{
    public async Task<Result<object>> Handle(RemoverContatoTreinadorCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var treinadorResult =
                await usuarioRepository.BuscarTreinadorPorIdAsync(request.TreinadorId, cancellationToken);
            if (treinadorResult.IsFailed) return Result.Fail<object>(treinadorResult.Errors);

            var removeResult = treinadorResult.Value.RemoverContato(request.ContatoId);
            if (removeResult.IsFailed) return Result.Fail<object>(removeResult.Errors);

            var saveResult = await usuarioRepository.AtualizarUsuarioAsync(treinadorResult.Value);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok<object>("Contato removido com sucesso.");
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao remover contato: {ex.Message}");
        }
    }
}