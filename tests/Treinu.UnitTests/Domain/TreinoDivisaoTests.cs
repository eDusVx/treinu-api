using FluentAssertions;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Dtos;
using Xunit;

namespace Treinu.UnitTests.Domain;

public class TreinoDivisaoTests
{
    [Fact]
    public void CriarTreino_ComDivisoesValidas_DeveTerSucesso()
    {
        var item1 = new CriarItemTreinoProps(Guid.NewGuid(), 3, "10-12", "20kg", "60s", "Obs", 1, "A");
        var item2 = new CriarItemTreinoProps(Guid.NewGuid(), 4, "8-10", "30kg", "90s", "Obs B", 2, "B");

        var props = new CriarTreinoProps(
            "Treino A/B",
            "Descrição Teste",
            DateTime.UtcNow,
            DateTime.UtcNow.AddMonths(1),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new List<CriarItemTreinoProps> { item1, item2 },
            "Perna", // NomeDivisaoA
            "Peito", // NomeDivisaoB
            null,
            null,
            "A",     // Segunda
            "B",     // Terça
            null,    // Quarta (Descanso)
            "A",     // Quinta
            "B",     // Sexta
            null,    // Sábado
            null     // Domingo
        );

        var result = Treino.Criar(props);

        result.IsSuccess.Should().BeTrue();
        var treino = result.Value;
        treino.NomeDivisaoA.Should().Be("Perna");
        treino.NomeDivisaoB.Should().Be("Peito");
        treino.DivisaoSegunda.Should().Be("A");
        treino.DivisaoTerca.Should().Be("B");
        treino.DivisaoQuarta.Should().BeNull();
    }

    [Fact]
    public void CriarTreino_SemDivisaoAouB_DeveFalhar()
    {
        var item = new CriarItemTreinoProps(Guid.NewGuid(), 3, "12", "10kg", "60s", "", 1, "A");

        var props = new CriarTreinoProps(
            "Treino Sem Divisoes",
            "Descrição",
            DateTime.UtcNow,
            DateTime.UtcNow.AddMonths(1),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new List<CriarItemTreinoProps> { item },
            null,    // NomeDivisaoA null
            "Peito", // NomeDivisaoB
            null,
            null,
            "A",
            "B",
            null, null, null, null, null
        );

        var result = Treino.Criar(props);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message.Contains("O treino deve ter pelo menos a Divisão A configurada"));
    }

    [Fact]
    public void CriarTreino_AtribuindoDivisaoNaoConfigurada_DeveFalhar()
    {
        var item = new CriarItemTreinoProps(Guid.NewGuid(), 3, "12", "10kg", "60s", "", 1, "A");

        var props = new CriarTreinoProps(
            "Treino Invalido",
            "Descrição",
            DateTime.UtcNow,
            DateTime.UtcNow.AddMonths(1),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new List<CriarItemTreinoProps> { item },
            "Perna",
            "Peito",
            null, // C is null
            null,
            "C",  // Mapping Monday to C which is null
            "B",
            null, null, null, null, null
        );

        var result = Treino.Criar(props);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message.Contains("não está configurada no treino"));
    }

    [Fact]
    public void ToDto_ComDiaEspecifico_DeveFiltrarItensParaDivisaoDoDia()
    {
        var exercicioIdA = Guid.NewGuid();
        var exercicioIdB = Guid.NewGuid();
        var item1 = new CriarItemTreinoProps(exercicioIdA, 3, "10", "20kg", "60s", "Obs A", 1, "A");
        var item2 = new CriarItemTreinoProps(exercicioIdB, 4, "8", "30kg", "90s", "Obs B", 2, "B");

        var props = new CriarTreinoProps(
            "Treino A/B Filtro",
            "Descrição",
            DateTime.UtcNow,
            DateTime.UtcNow.AddMonths(1),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new List<CriarItemTreinoProps> { item1, item2 },
            "Perna",
            "Peito",
            null,
            null,
            "A", // Segunda -> A
            "B", // Terça -> B
            null, null, null, null, null
        );

        var treino = Treino.Criar(props).Value;

        // Se passarmos Segunda (Monday) para o ToDto, deve conter apenas o exercício da divisão A
        var dtoSegunda = treino.ToDto(DayOfWeek.Monday);
        dtoSegunda.Itens.Should().ContainSingle();
        dtoSegunda.Itens[0].ExercicioId.Should().Be(exercicioIdA);
        dtoSegunda.Itens[0].Divisao.Should().Be("A");

        // Se passarmos Terça (Tuesday) para o ToDto, deve conter apenas o exercício da divisão B
        var dtoTerca = treino.ToDto(DayOfWeek.Tuesday);
        dtoTerca.Itens.Should().ContainSingle();
        dtoTerca.Itens[0].ExercicioId.Should().Be(exercicioIdB);
        dtoTerca.Itens[0].Divisao.Should().Be("B");

        // Se passarmos Quarta (Wednesday - dia de descanso), deve retornar vazio
        var dtoQuarta = treino.ToDto(DayOfWeek.Wednesday);
        dtoQuarta.Itens.Should().BeEmpty();
    }
}
