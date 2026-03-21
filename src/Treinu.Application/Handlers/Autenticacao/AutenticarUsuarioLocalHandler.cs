using FluentResults;
using Treinu.Application.Interfaces;
using Treinu.Contracts.Queries;
using Treinu.Contracts.Responses;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Errors;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Autenticacao;

public class AutenticarUsuarioLocalHandler : IRequestHandler<AutenticarUsuarioLocalQuery, Result<TokenDto>>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly ITokenService _tokenService;

    public AutenticarUsuarioLocalHandler(ICredencialRepository credencialRepository, ITokenService tokenService)
    {
        _credencialRepository = credencialRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<TokenDto>> Handle(AutenticarUsuarioLocalQuery request, CancellationToken cancellationToken)
    {
        var credencial = await _credencialRepository.BuscarCredencialPorEmailAsync(request.Email);

        if (credencial == null)
            return Result.Fail<TokenDto>(DomainErrors.Credencial.NaoEncontrada);

        var verifyResult = credencial.VerificarSenha(request.Senha);
        if (verifyResult.IsFailed) return Result.Fail<TokenDto>(verifyResult.Errors);

        var token = _tokenService.GerarJwt(credencial.Email, credencial.TipoUsuario.ToString(),
            credencial.UsuarioId.ToString());
        var refreshToken = _tokenService.GerarRefreshToken();

        credencial.AtualizarRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
        await _credencialRepository.AtualizarCredencialAsync(credencial);

        return Result.Ok(new TokenDto(token, refreshToken));
    }
}