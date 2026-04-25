using FluentValidation;
using Treinu.Contracts.Commands.Alunos;

namespace Treinu.Application.Validators.Alunos;

public class AdicionarAvaliacaoFisicaProprioAlunoCommandValidator : AbstractValidator<AdicionarAvaliacaoFisicaProprioAlunoCommand>
{
    public AdicionarAvaliacaoFisicaProprioAlunoCommandValidator()
    {
        RuleFor(x => x.AlunoId).NotEmpty().WithMessage("ID do aluno é obrigatório.");
        RuleFor(x => x.AvaliacaoFisica).SetValidator(new AvaliacaoFisicaDtoValidator());
    }
}
