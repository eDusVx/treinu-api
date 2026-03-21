using FluentAssertions;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Exceptions;

namespace Treinu.UnitTests.Domain;

public class UsuarioTests
{
    [Fact]
    public void CriarAluno_Deve_Atribuir_Criar_Senha_Em_Hash()
    {
        var props = new CriarAlunoProps(
            "Testinho da Silva", "teste@teste.com", "SenhaValida@123", new DateTime(2000, 1, 1),
            GeneroEnum.MASCULINO, new List<Contato>(), "00000000000", true, true, ObjetivoEnum.HIPERTROFIA
        );

        var validProps = props with { Cpf = "11144477735" };
        var aluno = Aluno.Criar(validProps);

        aluno.NomeCompleto.Should().Be("Testinho da Silva");
        aluno.Senha.Should().NotBeNullOrEmpty();
        aluno.Senha.Should().NotBe("SenhaValida@123");
        aluno.DomainEvents.Should().HaveCount(1);
    }

    [Fact]
    public void CriarAluno_Deve_Falhar_Com_Cpf_Invalido()
    {
        var props = new CriarAlunoProps(
            "Testinho da Silva", "teste@teste.com", "SenhaValida@123", new DateTime(2000, 1, 1),
            GeneroEnum.MASCULINO, new List<Contato>(), "00000000000", true, true, ObjetivoEnum.EMAGRECIMENTO
        );

        var action = () => Aluno.Criar(props);

        action.Should().Throw<UsuarioException>().WithMessage("cpf nao é valido");
    }

    [Fact]
    public void CriarAluno_Deve_Falhar_Com_Senha_Fraca()
    {
        var props = new CriarAlunoProps(
            "Testinho da Silva", "teste@teste.com", "fraca", new DateTime(2000, 1, 1),
            GeneroEnum.MASCULINO, new List<Contato>(), "11144477735", true, true, ObjetivoEnum.EMAGRECIMENTO
        );

        var action = () => Aluno.Criar(props);

        action.Should().Throw<UsuarioException>();
    }
}
