using FluentResults;
using Treinu.Application.Interfaces;
using Treinu.Contracts.Queries;
using Treinu.Contracts.Responses;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Errors;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Autenticacao;

public class RenovarTokenHandler : IRequestHandler<RenovarTokenQuery, Result<TokenDto>>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly ITokenService _tokenService;

    public RenovarTokenHandler(ICredencialRepository credencialRepository, ITokenService tokenService)
    {
        _credencialRepository = credencialRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<TokenDto>> Handle(RenovarTokenQuery request, CancellationToken cancellationToken)
    {
        var credencial = await _credencialRepository.BuscarCredencialPorRefreshTokenAsync(request.RefreshToken);

        if (credencial == null || credencial.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            if (credencial != null)
            {
                credencial.RevogarRefreshToken();
                await _credencialRepository.AtualizarCredencialAsync(credencial);
            }

            return Result.Fail<TokenDto>(DomainErrors.Credencial.TokenExpirado);
        }

        var newToken = _tokenService.GerarJwt(credencial.Email, credencial.TipoUsuario.ToString(),
            credencial.UsuarioId.ToString());
        var newRefreshToken = _tokenService.GerarRefreshToken();

        credencial.AtualizarRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(7));
        await _credencialRepository.AtualizarCredencialAsync(credencial);

        return Result.Ok(new TokenDto(newToken, newRefreshToken));
    }
}