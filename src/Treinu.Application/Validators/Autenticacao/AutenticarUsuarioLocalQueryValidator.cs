using FluentValidation;
using Treinu.Contracts.Queries.Autenticacao;

namespace Treinu.Application.Validators.Autenticacao;

public class AutenticarUsuarioLocalQueryValidator : AbstractValidator<AutenticarUsuarioLocalQuery>
{
    public AutenticarUsuarioLocalQueryValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email inválido.");
        RuleFor(x => x.Senha).NotEmpty().WithMessage("Senha é obrigatória.");
    }
}