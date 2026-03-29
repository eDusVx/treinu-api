using FluentResults;
using Treinu.Contracts.Commands.Usuarios;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Factories;
using Treinu.Domain.Factories.Interfaces;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinadores;

public class RegistrarTreinadorHandler(
    IUsuarioRepository usuarioRepository,
    IUsuarioFactory usuarioFactory) : IRequestHandler<RegistrarTreinadorCommand, Result<object>>
{
    public async Task<Result<object>> Handle(RegistrarTreinadorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existenciaResult = await usuarioRepository.VerificarExistenciaAsync(request.Email, request.Cpf);
            if (existenciaResult.IsFailed) return Result.Fail<object>(existenciaResult.Errors);

            var props = new FabricarUsuarioProps(
                request.NomeCompleto,
                request.Email,
                request.Senha,
                request.DataNascimento,
                request.Genero,
                request.Cpf,
                false,
                request.AceiteTermoAdesao,
                PerfilEnum.TREINADOR,
                Certificados:
                [
                    .. request.Certificados.Select(c =>
                        Certificado.Criar(new CriarCertificadoProps(c.Nome, c.ArquivoPdf)).Value)
                ]
            );

            var usuarioResult = usuarioFactory.Fabricar(props);
            if (usuarioResult.IsFailed) return Result.Fail<object>(usuarioResult.Errors);

            var saveResult = await usuarioRepository.SalvarUsuarioAsync(usuarioResult.Value);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            return Result.Ok(usuarioResult.Value.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao registrar treinador: {ex.Message}");
        }
    }
}