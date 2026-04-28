using FluentResults;
using Treinu.Application.Interfaces;
using Treinu.Contracts.Queries.Autenticacao;
using Treinu.Contracts.Responses;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Errors;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Autenticacao;

public class LoginComCodigoHandler : IRequestHandler<LoginComCodigoQuery, Result<TokenDto>>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly ITokenService _tokenService;

    public LoginComCodigoHandler(ICredencialRepository credencialRepository, ITokenService tokenService)
    {
        _credencialRepository = credencialRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<TokenDto>> Handle(LoginComCodigoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var credencialResult = await _credencialRepository.BuscarCredencialPorEmailAsync(request.Email);
            if (credencialResult.IsFailed) return Result.Fail<TokenDto>(credencialResult.Errors);

            var credencial = credencialResult.Value;
            if (credencial == null)
                return Result.Fail<TokenDto>(DomainErrors.Credencial.NaoEncontrada);

            var verifyResult = credencial.VerificarCodigoLogin(request.Codigo);
            if (verifyResult.IsFailed) return Result.Fail<TokenDto>(verifyResult.Errors);

            var token = _tokenService.GerarJwt(credencial.Email, credencial.TipoUsuario.ToString(),
                credencial.UsuarioId.ToString(), credencial.Usuario.NomeCompleto);
            var refreshToken = _tokenService.GerarRefreshToken();

            credencial.AtualizarRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));

            var updateResult = await _credencialRepository.AtualizarCredencialAsync(credencial);
            if (updateResult.IsFailed) return Result.Fail<TokenDto>(updateResult.Errors);

            return Result.Ok(new TokenDto(token, refreshToken));
        }
        catch (Exception ex)
        {
            return Result.Fail<TokenDto>($"Erro inesperado no login por código: {ex.Message}");
        }
    }
}
