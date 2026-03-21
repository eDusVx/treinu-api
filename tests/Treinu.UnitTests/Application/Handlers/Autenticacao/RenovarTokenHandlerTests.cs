using FluentAssertions;
using Moq;
using Treinu.Application.Handlers.Autenticacao;
using Treinu.Application.Interfaces;
using Treinu.Contracts.Queries;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;

namespace Treinu.UnitTests.Application.Handlers.Autenticacao;

public class RenovarTokenHandlerTests
{
    private readonly Mock<ICredencialRepository> _credencialRepositoryMock;
    private readonly RenovarTokenHandler _handler;
    private readonly Mock<ITokenService> _tokenServiceMock;

    public RenovarTokenHandlerTests()
    {
        _credencialRepositoryMock = new Mock<ICredencialRepository>();
        _tokenServiceMock = new Mock<ITokenService>();

        _handler = new RenovarTokenHandler(
            _credencialRepositoryMock.Object,
            _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Deve_Retornar_FailedResult_Quando_RefreshToken_Inexistente()
    {
        var query = new RenovarTokenQuery("token-invalido");
        _credencialRepositoryMock.Setup(repo => repo.BuscarCredencialPorRefreshTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((Credencial?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Deve_Retornar_FailedResult_E_Revogar_Quando_RefreshToken_Expirado()
    {
        var query = new RenovarTokenQuery("token-expirado");
        var fakeHash = BCrypt.Net.BCrypt.HashPassword("senha", 4);
        var credencialProps = new CriarCredencialProps(Guid.NewGuid(), "t@t.com", PerfilEnum.ALUNO, true, fakeHash);
        var credencial = Credencial.Criar(credencialProps).Value;

        credencial.AtualizarRefreshToken("token-expirado", DateTime.UtcNow.AddDays(-1));

        _credencialRepositoryMock.Setup(repo => repo.BuscarCredencialPorRefreshTokenAsync(It.IsAny<string>()))
            .ReturnsAsync(credencial);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.Should().BeTrue();

        credencial.RefreshToken.Should().BeNull();
        _credencialRepositoryMock.Verify(repo => repo.AtualizarCredencialAsync(It.IsAny<Credencial>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Deve_Testar_Sucesso_Gerando_Novos_Tokens()
    {
        var query = new RenovarTokenQuery("token-valido");
        var fakeHash = BCrypt.Net.BCrypt.HashPassword("senha", 4);
        var credencialProps = new CriarCredencialProps(Guid.NewGuid(), "t@t.com", PerfilEnum.ALUNO, true, fakeHash);
        var credencial = Credencial.Criar(credencialProps).Value;

        credencial.AtualizarRefreshToken("token-valido", DateTime.UtcNow.AddDays(1));

        _credencialRepositoryMock.Setup(repo => repo.BuscarCredencialPorRefreshTokenAsync(It.IsAny<string>()))
            .ReturnsAsync(credencial);

        _tokenServiceMock.Setup(service => service.GerarJwt(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns("novo_jwt_valido");

        _tokenServiceMock.Setup(service => service.GerarRefreshToken())
            .Returns("novo_refresh_valido");

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.AccessToken.Should().Be("novo_jwt_valido");
        result.Value.RefreshToken.Should().Be("novo_refresh_valido");

        credencial.RefreshToken.Should().Be("novo_refresh_valido");
        _credencialRepositoryMock.Verify(repo => repo.AtualizarCredencialAsync(It.IsAny<Credencial>()), Times.Once);
    }
}