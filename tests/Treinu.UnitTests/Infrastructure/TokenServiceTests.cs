using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Treinu.Infrastructure.Security;

namespace Treinu.UnitTests.Infrastructure;

public class TokenServiceTests
{
    private readonly Mock<IConfiguration> _configMock;
    private readonly TokenService _tokenService;

    public TokenServiceTests()
    {
        var inMemorySettings = new Dictionary<string, string?> {
            {"JwtSettings:Secret", "MINHACHAVEMUITOGRANDEMESMOSUPERSECRETA12345"},
            {"JwtSettings:Issuer", "TreinuApiTest"},
            {"JwtSettings:Audience", "TreinuAppTest"},
            {"JwtSettings:ExpirationInMinutes", "60"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _configMock = new Mock<IConfiguration>();
        _tokenService = new TokenService(configuration);
    }

    [Fact]
    public void GerarJwt_Deve_Criar_Token_Valido()
    {
        var token = _tokenService.GerarJwt("teste@teste.com", "ALUNO", "id-123");

        token.Should().NotBeNullOrEmpty();
        token.Split('.').Length.Should().Be(3); 
    }

    [Fact]
    public void GerarRefreshToken_Deve_Criar_String_Base64()
    {
        var refreshToken = _tokenService.GerarRefreshToken();

        refreshToken.Should().NotBeNullOrEmpty();
        
        var buffer = new Span<byte>(new byte[refreshToken.Length]);
        var isBase64 = Convert.TryFromBase64String(refreshToken, buffer, out var _);
        isBase64.Should().BeTrue();
    }
}
