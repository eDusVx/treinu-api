using FluentResults;
using Treinu.Contracts.Commands;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinadores;

public class AdicionarCertificadoTreinadorHandler(
    IUsuarioRepository usuarioRepository) : IRequestHandler<AdicionarCertificadoTreinadorCommand, Result<object>>
{
    public async Task<Result<object>> Handle(AdicionarCertificadoTreinadorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var treinadorResult = await usuarioRepository.BuscarTreinadorPorIdAsync(request.TreinadorId, cancellationToken);
            if (treinadorResult.IsFailed) return Result.Fail<object>(treinadorResult.Errors);

            var certificadoResult = Certificado.Criar(new CriarCertificadoProps(
                request.Nome,
                request.ArquivoPdf,
                request.Validado
            ));
            if (certificadoResult.IsFailed) return Result.Fail<object>(certificadoResult.Errors);

            var addResult = treinadorResult.Value.AdicionarCertificado(certificadoResult.Value);
            if (addResult.IsFailed) return Result.Fail<object>(addResult.Errors);

            var saveResult = await usuarioRepository.AtualizarUsuarioAsync(treinadorResult.Value);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok(treinadorResult.Value.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao adicionar certificado: {ex.Message}");
        }
    }
}
