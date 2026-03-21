using FluentAssertions;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Exceptions;

namespace Treinu.UnitTests.Domain;

public class TreinadorTests
{
    [Fact]
    public void CriarTreinador_Deve_Criar_Com_Sucesso()
    {
        var props = new CriarTreinadorProps(
            "Treinador Mestre", "treinador@teste.com", "Senha@123", new DateTime(1980, 1, 1),
            GeneroEnum.FEMININO, new List<Contato>(), "11144477735", true, true,
            new List<Certificado>(), new List<string> { "Musculação", "Crossfit" }
        );

        var treinador = Treinador.Criar(props);

        treinador.NomeCompleto.Should().Be("Treinador Mestre");
        treinador.Especializacoes.Should().HaveCount(2);
        treinador.DomainEvents.Should().HaveCount(1);
    }

    [Fact]
    public void AdicionarEspecializacao_Deve_Adicionar_Na_Lista()
    {
        var props = new CriarTreinadorProps(
            "Treinador Mestre", "treinador@teste.com", "Senha@123", new DateTime(1980, 1, 1),
            GeneroEnum.FEMININO, new List<Contato>(), "11144477735", true, true,
            new List<Certificado>(), new List<string>()
        );
        var treinador = Treinador.Criar(props);

        treinador.AdicionarEspecializacao("Nutrição");

        treinador.Especializacoes.Should().Contain("Nutrição");
    }

    [Fact]
    public void AdicionarEspecializacao_Deve_Falhar_Quando_Vazia()
    {
        var props = new CriarTreinadorProps(
            "Treinador Mestre", "treinador@teste.com", "Senha@123", new DateTime(1980, 1, 1),
            GeneroEnum.FEMININO, new List<Contato>(), "11144477735", true, true,
            new List<Certificado>(), new List<string>()
        );
        var treinador = Treinador.Criar(props);

        var action = () => treinador.AdicionarEspecializacao("");

        action.Should().Throw<UsuarioException>();
    }
}
