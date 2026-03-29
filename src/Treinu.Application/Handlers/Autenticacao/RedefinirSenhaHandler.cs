using FluentResults;
using Treinu.Contracts.Commands.Autenticacao;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Errors;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Autenticacao;

public class RedefinirSenhaHandler : IRequestHandler<RedefinirSenhaCommand, Result<object>>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public RedefinirSenhaHandler(ICredencialRepository credencialRepository, IUsuarioRepository usuarioRepository)
    {
        _credencialRepository = credencialRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Result<object>> Handle(RedefinirSenhaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var credencialResult = await _credencialRepository.BuscarCredencialPorTokenRecuperacaoAsync(request.Token);
            if (credencialResult.IsFailed) return Result.Fail<object>(credencialResult.Errors);

            var credencial = credencialResult.Value;
            if (credencial == null)
            {
                return Result.Fail<object>(DomainErrors.Credencial.TokenRecuperacaoInvalido);
            }

            var usuario = await _usuarioRepository.BuscarPorIdAsync(credencial.UsuarioId, cancellationToken);
            if (usuario == null)
            {
                return Result.Fail<object>(DomainErrors.Usuario.UsuarioNaoEncontrado);
            }

            var alterarSenhaResult = usuario.AlterarSenha(request.NovaSenha);
            if (alterarSenhaResult.IsFailed) return Result.Fail<object>(alterarSenhaResult.Errors);

            var redefinirResult = credencial.RedefinirSenha(request.Token, usuario.Senha);
            if (redefinirResult.IsFailed) return Result.Fail<object>(redefinirResult.Errors);

            var usuarioUpdateResult = await _usuarioRepository.AtualizarUsuarioAsync(usuario);
            if (usuarioUpdateResult.IsFailed) return Result.Fail<object>(usuarioUpdateResult.Errors);

            var credencialUpdateResult = await _credencialRepository.AtualizarCredencialAsync(credencial);
            if (credencialUpdateResult.IsFailed) return Result.Fail<object>(credencialUpdateResult.Errors);

            return Result.Ok<object>(new { Mensagem = "Senha redefinida com sucesso." });
        }
        catch (Exception ex)
        {
            return Result.Fail<object>($"Erro inesperado na redefinição de senha: {ex.Message}");
        }
    }
}
