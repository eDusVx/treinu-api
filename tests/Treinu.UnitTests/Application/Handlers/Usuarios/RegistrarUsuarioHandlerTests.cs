using FluentAssertions;
using MediatR;
using Moq;
using Treinu.Application.Handlers.Usuarios;
using Treinu.Contracts.Commands;
using Treinu.Domain.Dtos;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Factories;
using Treinu.Domain.Repositories;

namespace Treinu.UnitTests.Application.Handlers.Usuarios;

public class RegistrarUsuarioHandlerTests
{
    private readonly Mock<IUsuarioRepository> _repoMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly RegistrarUsuarioHandler _handler;

    public RegistrarUsuarioHandlerTests()
    {
        _repoMock = new Mock<IUsuarioRepository>();
        var avaliacaoFactory = new AvaliacaoFisicaFactory();
        var factory = new UsuarioFactory(avaliacaoFactory);
        _mediatorMock = new Mock<IMediator>();

        _handler = new RegistrarUsuarioHandler(_repoMock.Object, factory, _mediatorMock.Object);
    }

    [Fact]
    public async Task Handle_Deve_Salvar_E_Retornar_AlunoDto()
    {
        var request = new RegistrarUsuarioCommand(
            "Aluno Silva", "aluno@t.com", "Senh@123", new DateTime(1990, 1, 1),
            GeneroEnum.MASCULINO, "11144477735", true, true, PerfilEnum.ALUNO,
            new List<ContatoDto>(), ObjetivoEnum.HIPERTROFIA, new List<AvaliacaoFisicaDto>()
        );

        _repoMock.Setup(r => r.VerificarExistenciaAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<AlunoDto>();
        _repoMock.Verify(r => r.SalvarUsuarioAsync(It.IsAny<Usuario>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Deve_Salvar_E_Retornar_TreinadorDto()
    {
        var request = new RegistrarUsuarioCommand(
            "Treinador Costa", "treina@t.com", "Senh@123", new DateTime(1990, 1, 1),
            GeneroEnum.FEMININO, "11144477735", true, true, PerfilEnum.TREINADOR,
            new List<ContatoDto>(), null, null, new List<CertificadoDto>(), new List<string>()
        );

        _repoMock.Setup(r => r.VerificarExistenciaAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<TreinadorDto>();
        _repoMock.Verify(r => r.SalvarUsuarioAsync(It.IsAny<Usuario>()), Times.Once);
    }
}
