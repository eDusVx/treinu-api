using FluentAssertions;
using Treinu.Domain.Dtos;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Factories;

namespace Treinu.UnitTests.Domain;

public class UsuarioFactoryTests
{
    private readonly AvaliacaoFisicaFactory _avaliacaoFactory;
    private readonly UsuarioFactory _factory;

    public UsuarioFactoryTests()
    {
        _avaliacaoFactory = new AvaliacaoFisicaFactory();
        _factory = new UsuarioFactory(_avaliacaoFactory);
    }

    [Fact]
    public void Fabricar_Deve_Retornar_Aluno_Quando_Props_Forem_Aluno()
    {
        var props = new FabricarUsuarioProps(
            "Aluno", "aluno@t.com", "Senh@123", new DateTime(1990, 1, 1),
            GeneroEnum.MASCULINO, "11144477735", true, true, PerfilEnum.ALUNO,
            new List<ContatoDto>(), ObjetivoEnum.HIPERTROFIA
        );

        var usuarioResult = _factory.Fabricar(props);
        usuarioResult.IsSuccess.Should().BeTrue();
        var usuario = usuarioResult.Value;

        usuario.Should().BeOfType<Aluno>();
        usuario.Perfil.Should().Be(PerfilEnum.ALUNO);
    }

    [Fact]
    public void Fabricar_Deve_Retornar_Treinador_Quando_Props_Forem_Treinador()
    {
        var props = new FabricarUsuarioProps(
            "Treinador", "t@t.com", "Senh@123", new DateTime(1990, 1, 1),
            GeneroEnum.MASCULINO, "11144477735", true, true, PerfilEnum.TREINADOR,
            new List<ContatoDto>(), Certificados: new List<CertificadoDto>(), Especializacoes: new List<string>()
        );

        var usuarioResult = _factory.Fabricar(props);
        usuarioResult.IsSuccess.Should().BeTrue();
        var usuario = usuarioResult.Value;

        usuario.Should().BeOfType<Treinador>();
        usuario.Perfil.Should().Be(PerfilEnum.TREINADOR);
    }


}