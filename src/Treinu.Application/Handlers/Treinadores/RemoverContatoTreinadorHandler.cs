using FluentResults;
using Treinu.Contracts.Commands.Treinadores;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinadores;

public class RemoverContatoTreinadorHandler(
    IUsuarioRepository usuarioRepository,
    IContatoRepository contatoRepository) : IRequestHandler<RemoverContatoTreinadorCommand, Result<object>>
{
    public async Task<Result<object>> Handle(RemoverContatoTreinadorCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var treinadorResult =
                await usuarioRepository.BuscarTreinadorPorIdAsync(request.TreinadorId, cancellationToken);
            if (treinadorResult.IsFailed) return Result.Fail<object>(treinadorResult.Errors);

            var removeResult = await contatoRepository.RemoverContatoAsync(request.ContatoId, request.TreinadorId, cancellationToken);
            if (removeResult.IsFailed) return Result.Fail<object>(removeResult.Errors);

            return Result.Ok<object>("Contato removido com sucesso.");
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao remover contato: {ex.Message}");
        }
    }
}