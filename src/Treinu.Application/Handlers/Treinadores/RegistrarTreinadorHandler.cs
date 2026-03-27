using FluentResults;
using Treinu.Contracts.Commands;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;
using Treinu.Domain.Factories;
using Treinu.Domain.Factories.Interfaces;
using Treinu.Domain.Errors;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinadores;

public class RegistrarTreinadorHandler(
    IUsuarioRepository usuarioRepository,
    IConviteRepository conviteRepository,
    IUsuarioFactory usuarioFactory) : IRequestHandler<RegistrarTreinadorCommand, Result<object>>
{
    public async Task<Result<object>> Handle(RegistrarTreinadorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var conviteResult = await conviteRepository.BuscarPorTokenAsync(request.TokenConvite);
            if (conviteResult.IsFailed) return Result.Fail<object>(conviteResult.Errors);

            var convite = conviteResult.Value;
            if (convite.Perfil != PerfilEnum.TREINADOR)
                return Result.Fail<object>(DomainErrors.Convite.PerfilInvalido);

            var aceitarResult = convite.Aceitar();
            if (aceitarResult.IsFailed) return Result.Fail<object>(aceitarResult.Errors);

            var existenciaResult = await usuarioRepository.VerificarExistenciaAsync(request.Email, request.Cpf);
            if (existenciaResult.IsFailed) return Result.Fail<object>(existenciaResult.Errors);

            var props = new FabricarUsuarioProps(
                request.NomeCompleto,
                request.Email,
                request.Senha,
                request.DataNascimento,
                request.Genero,
                request.Cpf,
                Ativo: true,
                request.AceiteTermoAdesao,
                PerfilEnum.TREINADOR
            );

            var usuarioResult = usuarioFactory.Fabricar(props);
            if (usuarioResult.IsFailed) return Result.Fail<object>(usuarioResult.Errors);

            var saveResult = await usuarioRepository.SalvarUsuarioAsync(usuarioResult.Value);
            if (saveResult.IsFailed) return Result.Fail<object>(saveResult.Errors);

            await conviteRepository.AtualizarConviteAsync(convite);

            return Result.Ok(usuarioResult.Value.ToDto());
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado ao registrar treinador: {ex.Message}");
        }
    }
}
