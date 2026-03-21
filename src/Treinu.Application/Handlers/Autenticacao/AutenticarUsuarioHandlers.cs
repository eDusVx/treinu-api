using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Treinu.Application.Interfaces;
using Treinu.Contracts.Queries;
using Treinu.Contracts.Responses;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;
using Treinu.Domain.Exceptions;

namespace Treinu.Application.Handlers.Autenticacao;

public class AutenticarUsuarioLocalQueryHandler : IRequestHandler<AutenticarUsuarioLocalQuery, TokenDto>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly ITokenService _tokenService;

    public AutenticarUsuarioLocalQueryHandler(ICredencialRepository credencialRepository, ITokenService tokenService)
    {
        _credencialRepository = credencialRepository;
        _tokenService = tokenService;
    }

    public async Task<TokenDto> Handle(AutenticarUsuarioLocalQuery request, CancellationToken cancellationToken)
    {
        var credencial = await _credencialRepository.BuscarCredencialPorEmailAsync(request.Email);
        
        if (credencial == null)
            throw new RepositoryException("Usuário não encontrado.");

        credencial.VerificarSenha(request.Senha);

        var token = _tokenService.GerarJwt(credencial.Email, credencial.TipoUsuario.ToString(), credencial.UsuarioId.ToString());
        var refreshToken = _tokenService.GerarRefreshToken();

        credencial.AtualizarRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
        await _credencialRepository.AtualizarCredencialAsync(credencial);

        return new TokenDto(token, refreshToken);
    }
}

public class RenovarTokenQueryHandler : IRequestHandler<RenovarTokenQuery, TokenDto>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly ITokenService _tokenService;

    public RenovarTokenQueryHandler(ICredencialRepository credencialRepository, ITokenService tokenService)
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
