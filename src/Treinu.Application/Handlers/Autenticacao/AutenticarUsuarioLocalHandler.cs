using MediatR;
using Treinu.Application.Interfaces;
using Treinu.Contracts.Queries;
using Treinu.Contracts.Responses;
using Treinu.Domain.Repositories;
using Treinu.Domain.Exceptions;

namespace Treinu.Application.Handlers.Autenticacao;

public class AutenticarUsuarioLocalHandler : IRequestHandler<AutenticarUsuarioLocalQuery, TokenDto>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly ITokenService _tokenService;

    public AutenticarUsuarioLocalHandler(ICredencialRepository credencialRepository, ITokenService tokenService)
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
