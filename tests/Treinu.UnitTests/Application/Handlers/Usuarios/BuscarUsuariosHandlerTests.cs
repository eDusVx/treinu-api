using FluentAssertions;
using FluentResults;
using Moq;
using Treinu.Application.Handlers.Usuarios;
using Treinu.Contracts.Queries.Usuarios;
using Treinu.Contracts.Responses;
using Treinu.Domain.Dtos;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;

namespace Treinu.UnitTests.Application.Handlers.Usuarios;

public class BuscarUsuariosHandlerTests
{
    private readonly BuscarUsuariosHandler _handler;
    private readonly Mock<IUsuarioRepository> _repoMock;

    public BuscarUsuariosHandlerTests()
    {
        _repoMock = new Mock<IUsuarioRepository>();
        _handler = new BuscarUsuariosHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_Deve_Retornar_PaginationResponse_Com_Dtos()
    {
        var request = new BuscarUsuariosQuery(PerfilEnum.ALUNO);

        var mockProps = new CriarAlunoProps(
            "Teste", "b@b.com", "Senh@123", new DateTime(1990, 1, 1),
            GeneroEnum.MASCULINO, new List<Contato>(), "11144477735", true, true, ObjetivoEnum.SAUDE
        );
        var mockAluno = Aluno.Criar(mockProps).Value;

        var usuarios = new List<Usuario> { mockAluno };

        _repoMock.Setup(r =>
                r.BuscarUsuariosPaginadoAsync(It.IsAny<PerfilEnum?>(), 1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok((1, (IEnumerable<Usuario>)usuarios)));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<PaginationResponse<object>>();
        result.Value.Total.Should().Be(1);
        result.Value.TotalPages.Should().Be(1);
        result.Value.Data.Should().HaveCount(1);
        result.Value.Data.First().Should().BeOfType<AlunoDto>();
    }
}