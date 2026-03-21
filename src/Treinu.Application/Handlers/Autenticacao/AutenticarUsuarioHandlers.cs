using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Treinu.Contracts.Queries;
using Treinu.Contracts.Responses;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;
using Treinu.Domain.Exceptions;

namespace Treinu.Application.Handlers.Autenticacao;

public class AutenticarUsuarioLocalQueryHandler : IRequestHandler<AutenticarUsuarioLocalQuery, TokenDto>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly IConfiguration _configuration;

    public AutenticarUsuarioLocalQueryHandler(ICredencialRepository credencialRepository, IConfiguration configuration)
    {
        _credencialRepository = credencialRepository;
        _configuration = configuration;
    }

    public async Task<TokenDto> Handle(AutenticarUsuarioLocalQuery request, CancellationToken cancellationToken)
    {
        var credencial = await _credencialRepository.BuscarCredencialPorEmailAsync(request.Email);
        
        if (credencial == null)
            throw new RepositoryException("Usuário não encontrado.");

        credencial.VerificarProvider(AuthProviderEnum.LOCAL);
        credencial.VerificarSenha(request.Senha);

        var token = GerarJwt(credencial.Email, credencial.TipoUsuario.ToString(), credencial.UsuarioId.ToString());

        return new TokenDto(token);
    }

    private string GerarJwt(string email, string role, string sub)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secret = jwtSettings.GetValue<string>("Secret") ?? "DefaultSecretKeyss";
        var issuer = jwtSettings.GetValue<string>("Issuer");
        var audience = jwtSettings.GetValue<string>("Audience");
        var numMinutes = jwtSettings.GetValue<int>("ExpirationInMinutes");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secret);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, sub),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(numMinutes == 0 ? 60 : numMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class AutenticarUsuarioGoogleQueryHandler : IRequestHandler<AutenticarUsuarioGoogleQuery, TokenDto>
{
    public AutenticarUsuarioGoogleQueryHandler()
    {
    }

    public async Task<TokenDto> Handle(AutenticarUsuarioGoogleQuery request, CancellationToken cancellationToken)
    {
        // To strictly match MIGRAR2's Google validation, normally we parse the Google token here.
        // For the sake of this implementation, we throw NotImplemented.
        throw new NotImplementedException("Google Auth verification not yet implemented on C# Core.");
    }
}

