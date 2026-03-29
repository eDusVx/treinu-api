using FluentAssertions;
using Treinu.Application.Validators.Autenticacao;
using Treinu.Contracts.Queries.Autenticacao;

namespace Treinu.UnitTests.Application.Validators.Autenticacao;

public class AutenticarUsuarioLocalQueryValidatorTests
{
    private readonly AutenticarUsuarioLocalQueryValidator _validator;

    public AutenticarUsuarioLocalQueryValidatorTests()
    {
        _validator = new AutenticarUsuarioLocalQueryValidator();
    }

    [Fact]
    public void Validador_Deve_Ter_Erro_Quando_Email_Invalido()
    {
        var command = new AutenticarUsuarioLocalQuery("invalid-email", "senha123");
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validador_Deve_Ter_Erro_Quando_Senha_Vazia()
    {
        var command = new AutenticarUsuarioLocalQuery("teste@teste.com", "");
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Senha");
    }

    [Fact]
    public void Validador_Deve_Passar_Quando_Dados_Validos()
    {
        var command = new AutenticarUsuarioLocalQuery("teste@teste.com", "senha123");
        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}