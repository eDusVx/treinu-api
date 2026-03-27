using FluentResults;
using Treinu.Contracts.Commands;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinadores;

public class RemoverCertificadoTreinadorHandler(
    IUsuarioRepository usuarioRepository) : IRequestHandler<RemoverCertificadoTreinadorCommand, Result<object>>
{
    public async Task<Result<object>> Handle(RemoverCertificadoTreinadorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var treinadorResult = await usuarioRepository.BuscarTreinadorPorIdAsync(request.TreinadorId, cancellationToken);
            if (treinadorResult.IsFailed) return Result.Fail<object>(treinadorResult.Errors);

            var removeResult = treinadorResult.Value.RemoverCertificado(request.CertificadoId);
            if (removeResult.IsFailed) return Result.Fail<object>(removeResult.Errors);

            var saveResult = await usuarioRepository.AtualizarUsuarioAsync(treinadorResult.Value);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok<object>("Certificado removido com sucesso.");
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao remover certificado: {ex.Message}");
        }
    }
}
