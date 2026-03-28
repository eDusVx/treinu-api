using FluentResults;
using Moq;
using Treinu.Application.Handlers.Autenticacao;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Events;
using Treinu.Domain.Repositories;

namespace Treinu.UnitTests.Application.Handlers.Autenticacao;

public class CadastrarCredencialHandlerTests
{
    private readonly Mock<ICredencialRepository> _credencialRepositoryMock;
    private readonly CadastrarCredencialHandler _handler;

    public CadastrarCredencialHandlerTests()
    {
        _credencialRepositoryMock = new Mock<ICredencialRepository>();
        _handler = new CadastrarCredencialHandler(_credencialRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Deve_Receber_Evento_E_Salvar_Nova_Credencial()
    {
        var fakeHash = BCrypt.Net.BCrypt.HashPassword("senha123", 4);
        var notification = new UsuarioCadastradoEvent(
            Guid.NewGuid(),
            "teste@event.com",
            fakeHash,
            PerfilEnum.ALUNO,
            true
        );

        _credencialRepositoryMock.Setup(repo => repo.SalvarCredencialAsync(It.IsAny<Credencial>()))
            .ReturnsAsync(Result.Ok());

        await _handler.Handle(notification, CancellationToken.None);

        _credencialRepositoryMock.Verify(
            repo => repo.SalvarCredencialAsync(It.Is<Credencial>(c =>
                c.Email == "teste@event.com" &&
                c.TipoUsuario == PerfilEnum.ALUNO &&
                c.Senha == fakeHash &&
                c.UsuarioId == notification.Id &&
                c.Ativo == true
            )),
            Times.Once
        );
    }
}