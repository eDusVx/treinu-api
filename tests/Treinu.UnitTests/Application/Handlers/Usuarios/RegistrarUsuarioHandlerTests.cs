using FluentAssertions;
using FluentResults;
using Moq;
using Treinu.Application.Handlers.Alunos;
using Treinu.Application.Handlers.Treinadores;
using Treinu.Contracts.Commands;
using Treinu.Domain.Dtos;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Factories;
using Treinu.Domain.Repositories;

namespace Treinu.UnitTests.Application.Handlers.Usuarios;

public class RegistrarTreinadorHandlerTests
{
    private readonly RegistrarTreinadorHandler _handler;
    private readonly Mock<IUsuarioRepository> _repoMock;
    private readonly Mock<IConviteRepository> _conviteRepoMock;

    public RegistrarTreinadorHandlerTests()
    {
        _repoMock = new Mock<IUsuarioRepository>();
        _conviteRepoMock = new Mock<IConviteRepository>();
        var factory = new UsuarioFactory();

        _handler = new RegistrarTreinadorHandler(_repoMock.Object, _conviteRepoMock.Object, factory);
    }

    [Fact]
    public async Task Handle_Deve_Salvar_E_Retornar_TreinadorDto()
    {
        var token = Guid.NewGuid();
        var request = new RegistrarTreinadorCommand(
            "Treinador Costa", "treina@t.com", "Senh@123", new DateTime(1990, 1, 1),
            GeneroEnum.FEMININO, "11144477735", true, token
        );

        var convite = Convite.Criar(request.Email, PerfilEnum.TREINADOR).Value;

        _conviteRepoMock.Setup(r => r.BuscarPorTokenAsync(token))
            .ReturnsAsync(Result.Ok(convite));

        _repoMock.Setup(r => r.VerificarExistenciaAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Ok());

        _repoMock.Setup(r => r.SalvarUsuarioAsync(It.IsAny<Usuario>()))
            .ReturnsAsync(Result.Ok());

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<TreinadorDto>();
        _repoMock.Verify(r => r.SalvarUsuarioAsync(It.IsAny<Usuario>()), Times.Once);
        _conviteRepoMock.Verify(r => r.AtualizarConviteAsync(It.IsAny<Convite>()), Times.Once);
    }
}

public class RegistrarAlunoHandlerTests
{
    private readonly RegistrarAlunoHandler _handler;
    private readonly Mock<IUsuarioRepository> _repoMock;
    private readonly Mock<IConviteRepository> _conviteRepoMock;

    public RegistrarAlunoHandlerTests()
    {
        _repoMock = new Mock<IUsuarioRepository>();
        _conviteRepoMock = new Mock<IConviteRepository>();
        var factory = new UsuarioFactory();

        _handler = new RegistrarAlunoHandler(_repoMock.Object, _conviteRepoMock.Object, factory);
    }

    [Fact]
    public async Task Handle_Deve_Salvar_E_Retornar_AlunoDto()
    {
        var token = Guid.NewGuid();
        var request = new RegistrarAlunoCommand(
            "Aluno Silva", "aluno@t.com", "Senh@123", new DateTime(1990, 1, 1),
            GeneroEnum.MASCULINO, "11144477735", true, ObjetivoEnum.HIPERTROFIA, token
        );

        var convite = Convite.Criar(request.Email, PerfilEnum.ALUNO, Guid.NewGuid()).Value;

        _conviteRepoMock.Setup(r => r.BuscarPorTokenAsync(token))
            .ReturnsAsync(Result.Ok(convite));

        _repoMock.Setup(r => r.VerificarExistenciaAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Ok());

        _repoMock.Setup(r => r.SalvarUsuarioAsync(It.IsAny<Usuario>()))
            .ReturnsAsync(Result.Ok());

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<AlunoDto>();
        _repoMock.Verify(r => r.SalvarUsuarioAsync(It.IsAny<Usuario>()), Times.Once);
        _conviteRepoMock.Verify(r => r.AtualizarConviteAsync(It.IsAny<Convite>()), Times.Once);
    }
}