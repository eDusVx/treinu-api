using FluentValidation;
using Treinu.Contracts.Commands;

namespace Treinu.Application.Validators.Usuarios;

public class RegistrarUsuarioCommandValidator : AbstractValidator<RegistrarUsuarioCommand>
{
    public RegistrarUsuarioCommandValidator()
    {
        RuleFor(x => x.NomeCompleto).NotEmpty().MinimumLength(3)
            .WithMessage("Nome completo deve ter pelo menos 3 caracteres.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email inválido.");
        RuleFor(x => x.Senha).NotEmpty().MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");
        RuleFor(x => x.Cpf).NotEmpty().Length(11).WithMessage("CPF deve ter exatos 11 caracteres.");
        RuleFor(x => x.DataNascimento).LessThan(DateTime.UtcNow).WithMessage("Data de nascimento inválida.");
        RuleFor(x => x.AceiteTermoAdesao).Equal(true)
            .WithMessage("Você deve aceitar os termos de adesão para se registrar.");
    }
}