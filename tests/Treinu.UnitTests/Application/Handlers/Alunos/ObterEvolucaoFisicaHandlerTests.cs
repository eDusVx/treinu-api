using FluentAssertions;
using FluentResults;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Treinu.Application.Handlers.Alunos;
using Treinu.Contracts.Queries.Alunos;
using Treinu.Domain.Dtos;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;
using Xunit;

namespace Treinu.UnitTests.Application.Handlers.Alunos;

public class ObterEvolucaoFisicaHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IMetaRepository> _metaRepositoryMock;
    private readonly ObterEvolucaoFisicaHandler _handler;

    public ObterEvolucaoFisicaHandlerTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _metaRepositoryMock = new Mock<IMetaRepository>();
        _handler = new ObterEvolucaoFisicaHandler(_usuarioRepositoryMock.Object, _metaRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_DeveCalcularEvolucaoFisicaCorretamente()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var data1 = DateTime.UtcNow.AddDays(-10);
        var data2 = DateTime.UtcNow;

        var medidas1 = new List<Treinu.Domain.Entities.AvaliacaoFisica.Medida> {
            Treinu.Domain.Entities.AvaliacaoFisica.Medida.Criar(new Treinu.Domain.Entities.AvaliacaoFisica.CriarMedidaProps(ChaveMedidaEnum.CINTURA, 80)).Value
        };
        var medidas2 = new List<Treinu.Domain.Entities.AvaliacaoFisica.Medida> {
            Treinu.Domain.Entities.AvaliacaoFisica.Medida.Criar(new Treinu.Domain.Entities.AvaliacaoFisica.CriarMedidaProps(ChaveMedidaEnum.CINTURA, 76)).Value
        };

        var av1 = Treinu.Domain.Entities.AvaliacaoFisica.AvaliacaoFisica.Criar(
            new Treinu.Domain.Entities.AvaliacaoFisica.CriarAvaliacaoFisicaProps(1.75, 80.0, medidas1, data1, 20.0)
        ).Value;

        var av2 = Treinu.Domain.Entities.AvaliacaoFisica.AvaliacaoFisica.Criar(
            new Treinu.Domain.Entities.AvaliacaoFisica.CriarAvaliacaoFisicaProps(1.75, 75.0, medidas2, data2, 18.0)
        ).Value;

        var alunoProps = new CriarAlunoProps(
            "João Aluno", "joao@email.com", "SenhaForte123!", DateTime.UtcNow.AddYears(-20), GeneroEnum.MASCULINO,
            new List<Contato>(), "12345678909", true, true, ObjetivoEnum.EMAGRECIMENTO,
            Guid.NewGuid(), new List<Treinu.Domain.Entities.AvaliacaoFisica.AvaliacaoFisica> { av1, av2 }
        );

        var aluno = Aluno.Criar(alunoProps).Value;

        _usuarioRepositoryMock.Setup(repo => repo.BuscarAlunoPorIdAsync(alunoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(aluno));

        var meta = Meta.Criar(alunoId, TipoMetaEnum.PESO, 70, DateTime.UtcNow.AddMonths(1)).Value;
        _metaRepositoryMock.Setup(repo => repo.BuscarMetasPorAlunoAsync(alunoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Meta> { meta });

        var query = new ObterEvolucaoFisicaQuery(alunoId, null, null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var responseDict = result.Value as Dictionary<string, EvolucaoMedidaDto>;
        responseDict.Should().NotBeNull();

        // Check Weight (peso)
        var pesoResult = responseDict["peso"];
        pesoResult.UltimoValor.Should().Be(75.0);
        pesoResult.DeltaAbsoluto.Should().Be(-5.0);
        pesoResult.DeltaPercentual.Should().Be(-6.25);
        pesoResult.Tendencia.Should().Be("DESCENDO");

        // Check goal crossing
        var metaPeso = pesoResult.Meta;
        metaPeso.Should().NotBeNull();
        metaPeso.Status.Should().Be("PENDENTE");
        // dist total = 80 - 70 = 10. dist atual = 80 - 75 = 5. progresso = 50%
        metaPeso.Progresso.Should().Be(50.0);
    }
}
