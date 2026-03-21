using MediatR;
using Treinu.Application.Interfaces;
using Treinu.Contracts.Queries;
using Treinu.Contracts.Responses;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Autenticacao;

public class RenovarTokenHandler : IRequestHandler<RenovarTokenQuery, TokenDto>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly ITokenService _tokenService;

    public RenovarTokenHandler(ICredencialRepository credencialRepository, ITokenService tokenService)
    {
        _credencialRepository = credencialRepository;
        _tokenService = tokenService;
    }

    public async Task<TokenDto> Handle(RenovarTokenQuery request, CancellationToken cancellationToken)
    {
        var credencial = await _credencialRepository.BuscarCredencialPorRefreshTokenAsync(request.RefreshToken);

        if (credencial == null || credencial.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            if (credencial != null)
            {
                credencial.RevogarRefreshToken();
                await _credencialRepository.AtualizarCredencialAsync(credencial);
            }
            throw new UnauthorizedAccessException("Refresh Token inválido ou expirado. Faça login novamente.");
        }

        var newToken = _tokenService.GerarJwt(credencial.Email, credencial.TipoUsuario.ToString(), credencial.UsuarioId.ToString());
        var newRefreshToken = _tokenService.GerarRefreshToken();

        credencial.AtualizarRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(7));
        await _credencialRepository.AtualizarCredencialAsync(credencial);

        return new TokenDto(newToken, newRefreshToken);
    }
}
