using FluentValidation;
using Treinu.Domain.Dtos;
using Treinu.Domain.Enums;

namespace Treinu.Application.Validators.Alunos;

public class AvaliacaoFisicaDtoValidator : AbstractValidator<AvaliacaoFisicaDto>
{
    public AvaliacaoFisicaDtoValidator()
    {
        RuleFor(x => x.Altura)
            .GreaterThan(0).WithMessage("Altura deve ser maior que zero.");

        RuleFor(x => x.Peso)
            .GreaterThan(0).WithMessage("Peso deve ser maior que zero.");

        RuleFor(x => x.Medidas)
            .NotEmpty().WithMessage("A lista de medidas não pode estar vazia.")
            .Must((dto, medidas, context) => 
            {
                var required = Enum.GetValues<ChaveMedidaEnum>();
                var provided = medidas?.Select(m => m.Chave).Distinct().ToList() ?? new List<ChaveMedidaEnum>();
                var missing = required.Except(provided).ToList();

                if (missing.Any())
                {
                    context.MessageFormatter.AppendArgument("MissingMeasurements", string.Join(", ", missing));
                    return false;
                }
                return true;
            })
            .WithMessage("Para uma avaliação completa, faltam as seguintes medidas: {MissingMeasurements}");
    }
}
