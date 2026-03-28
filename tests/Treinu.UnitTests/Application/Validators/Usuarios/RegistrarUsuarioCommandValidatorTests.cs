using FluentAssertions;
using Treinu.Application.Validators.Usuarios;
using Treinu.Contracts.Commands;
using Treinu.Domain.Enums;

namespace Treinu.UnitTests.Application.Validators.Usuarios;

public class RegistrarTreinadorCommandValidatorTests
{
    private readonly RegistrarTreinadorCommandValidator _validator;

    public RegistrarTreinadorCommandValidatorTests()
    {
        _validator = new RegistrarTreinadorCommandValidator();
    }

    [Fact]
    public void Validador_Deve_Ter_Erro_Quando_Cpf_Invalido()
    {
        var command = new RegistrarTreinadorCommand(
            "Fulano Silva", "valido@email.com", "Senha@123", new DateTime(1990, 1, 1),
            GeneroEnum.MASCULINO, "123", true,
            []
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Cpf");
    }

    [Fact]
    public void Validador_Deve_Ter_Erro_Quando_Certificados_Vazio()
    {
        var command = new RegistrarTreinadorCommand(
            "Fulano Silva", "valido@email.com", "Senha@123", new DateTime(1990, 1, 1),
            GeneroEnum.MASCULINO, "11144477735", true,
            []
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Certificados");
    }

    [Fact]
    public void Validador_Deve_Passar_Quando_Dados_Validos()
    {
        var command = new RegistrarTreinadorCommand(
            "Fulano Silva", "valido@email.com", "Senha@123", new DateTime(1990, 1, 1),
            GeneroEnum.MASCULINO, "11144477735", true,
            [new CertificadoDto("CREF", "path/to/pdf")]
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}