using FluentAssertions;
using Moq;
using Treinu.Application.Handlers.Autenticacao;
using Treinu.Application.Interfaces;
using Treinu.Contracts.Queries;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Exceptions;
using Treinu.Domain.Repositories;

namespace Treinu.UnitTests.Application.Handlers.Autenticacao;

public class AutenticarUsuarioLocalHandlerTests
{
    private readonly Mock<ICredencialRepository> _credencialRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly AutenticarUsuarioLocalHandler _handler;

    public AutenticarUsuarioLocalHandlerTests()
    {
        _credencialRepositoryMock = new Mock<ICredencialRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        
        _handler = new AutenticarUsuarioLocalHandler(
            _credencialRepositoryMock.Object, 
            _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Deve_Retornar_TokenDto_Quando_Credenciais_Validas()
    {
        var query = new AutenticarUsuarioLocalQuery("teste@teste.com", "senha123");
        var fakeHash = BCrypt.Net.BCrypt.HashPassword("senha123", 4);
        var credencialProps = new CriarCredencialProps(Guid.NewGuid(), "teste@teste.com", PerfilEnum.ALUNO, true, fakeHash);
        var credencial = Credencial.Criar(credencialProps).Value;

        _credencialRepositoryMock.Setup(repo => repo.BuscarCredencialPorEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(credencial);

        _tokenServiceMock.Setup(service => service.GerarJwt(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns("fake_jwt_token");

        _tokenServiceMock.Setup(service => service.GerarRefreshToken())
            .Returns("fake_refresh_token");

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.AccessToken.Should().Be("fake_jwt_token");
        result.Value.RefreshToken.Should().Be("fake_refresh_token");
        
        _credencialRepositoryMock.Verify(repo => repo.AtualizarCredencialAsync(It.IsAny<Credencial>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Deve_Retornar_FailedResult_Quando_Usuario_Nao_Existe()
    {
        var query = new AutenticarUsuarioLocalQuery("fake@teste.com", "senha123");
        _credencialRepositoryMock.Setup(repo => repo.BuscarCredencialPorEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Credencial?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.Should().BeTrue();
    }
}
