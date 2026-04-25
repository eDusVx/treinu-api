using FluentResults;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Entities.AvaliacaoFisica;

public record CriarAvaliacaoFisicaProps(
    double Altura,
    double Peso,
    List<Medida> Medidas,
    DateTime Data
);

public class AvaliacaoFisica : AggregateRoot
{
    private static readonly (double LimiteMaximo, ClassificacaoIMC Classificacao)[] ClassificacoesImc =
    {
        (18.5, ClassificacaoIMC.ABAIXO_DO_PESO),
        (24.9, ClassificacaoIMC.PESO_NORMAL),
        (29.9, ClassificacaoIMC.SOBREPESO),
        (34.9, ClassificacaoIMC.OBESIDADE_GRAU_I),
        (39.9, ClassificacaoIMC.OBESIDADE_GRAU_II),
        (double.PositiveInfinity, ClassificacaoIMC.OBESIDADE_GRAU_III)
    };

    private readonly List<Medida> _medidas = new();

    protected AvaliacaoFisica()
    {
    } // EF Constructor

    private AvaliacaoFisica(Guid id) : base(id)
    {
    }

    public DateTime Data { get; private set; }
    public double Altura { get; private set; }
    public double Peso { get; private set; }
    public double Imc { get; private set; }
    public ClassificacaoIMC Classificacao { get; private set; }

    public IReadOnlyCollection<Medida> Medidas => _medidas.AsReadOnly();

    public static Result<AvaliacaoFisica> Criar(CriarAvaliacaoFisicaProps props)
    {
        var id = Guid.NewGuid();
        var instance = new AvaliacaoFisica(id);

        var result = Result.Merge(
            instance.SetData(props.Data),
            instance.SetAltura(props.Altura),
            instance.SetPeso(props.Peso),
            instance.SetMedidas(props.Medidas)
        );

        if (result.IsSuccess)
            result.WithReasons(instance.CalcularIMC().Reasons);

        if (result.IsFailed) return result;

        return Result.Ok(instance);
    }

    public static AvaliacaoFisica Carregar(CriarAvaliacaoFisicaProps props, Guid id)
    {
        var instance = new AvaliacaoFisica(id);

        var result = Result.Merge(
            instance.SetData(props.Data),
            instance.SetAltura(props.Altura),
            instance.SetPeso(props.Peso),
            instance.SetMedidas(props.Medidas)
        );

        if (result.IsSuccess)
            result.WithReasons(instance.CalcularIMC().Reasons);

        if (result.IsFailed)
            throw new InvalidOperationException($"Erro ao carregar AvaliacaoFisica do banco: {result.Errors[0].Message}");

        return instance;
    }

    private Result SetData(DateTime data)
    {
        Data = DateTime.UtcNow.Date;
        return Result.Ok();
    }

    private Result SetAltura(double altura)
    {
        if (altura <= 0)
            return Result.Fail("Altura deve ser maior que zero");

        if (altura > 3)
            return Result.Fail("Altura inválida (valor muito alto)");

        Altura = Math.Round(altura, 2);
        return Result.Ok();
    }

    private Result SetPeso(double peso)
    {
        if (peso <= 0)
            return Result.Fail("Peso deve ser maior que zero");

        if (peso > 500)
            return Result.Fail("Peso inválido (valor muito alto)");

        Peso = Math.Round(peso, 2);
        return Result.Ok();
    }

    private Result SetMedidas(List<Medida> medidas)
    {
        if (medidas == null)
            return Result.Fail("Medidas devem ser um array");

        if (!medidas.Any())
            return Result.Fail("Pelo menos uma medida deve ser informada");

        var medidasInvalidas = medidas.Where(m => m == null).ToList();
        if (medidasInvalidas.Any())
            return Result.Fail("Array contém medidas inválidas");

        _medidas.Clear();
        _medidas.AddRange(medidas);
        return Result.Ok();
    }

    private Result CalcularIMC()
    {
        if (Altura <= 0 || Peso <= 0)
            return Result.Fail("Altura e peso devem ser definidos antes de calcular o IMC");

        var valorImc = Peso / Math.Pow(Altura, 2);
        Imc = Math.Round(valorImc, 2);
        Classificacao = DeterminarClassificacaoIMC(Imc);
        return Result.Ok();
    }

    private ClassificacaoIMC DeterminarClassificacaoIMC(double imc)
    {
        var faixaEncontrada = ClassificacoesImc.FirstOrDefault(f => imc <= f.LimiteMaximo);
        return faixaEncontrada.Classificacao != default ? faixaEncontrada.Classificacao : ClassificacaoIMC.OUTRO;
    }
}