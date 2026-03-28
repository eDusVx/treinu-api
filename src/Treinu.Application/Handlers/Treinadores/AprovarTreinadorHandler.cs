using FluentResults;
using Treinu.Contracts.Commands;
using Treinu.Domain.Core;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Enums;
using Treinu.Domain.Errors;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Treinadores;

public class AprovarTreinadorHandler(
    IUsuarioRepository usuarioRepository,
    IEmailService emailService) : IRequestHandler<AprovarTreinadorCommand, Result<Result>>
{
    public async Task<Result<Result>> Handle(AprovarTreinadorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var usuario = await usuarioRepository.BuscarPorIdAsync(request.TreinadorId, cancellationToken);
            if (usuario == null)
                return Result.Fail<Result>(DomainErrors.Usuario.UsuarioNaoEncontrado);

            if (usuario.Perfil != PerfilEnum.TREINADOR)
                return Result.Fail<Result>("Usuário não é um treinador.");

            var aprovarResult = usuario.Aprovar();
            if (aprovarResult.IsFailed) return Result.Fail<Result>(aprovarResult.Errors);

            var saveResult = await usuarioRepository.AtualizarUsuarioAsync(usuario);
            if (saveResult.IsFailed) return Result.Fail<Result>(saveResult.Errors);

            await emailService.SendEmailAsync(
                usuario.Email,
                "Bem-vindo ao Treinu!",
                $"Olá {usuario.NomeCompleto}, seu cadastro foi aprovado! Você já pode acessar a plataforma."
            );

            return Result.Ok(Result.Ok());
        }
        catch (Exception ex)
        {
            return Result.Fail<Result>($"Erro ao aprovar treinador: {ex.Message}");
        }
    }
}