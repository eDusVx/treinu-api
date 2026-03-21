using Treinu.Domain.Enums;
using Treinu.Domain.Exceptions;

namespace Treinu.Domain.Entities.AvaliacaoFisica;

public record CriarQuestionarioProps(
    double Altura,
    double Peso,
    List<Medida> Medida,
    DateTime Data
);

public class Questionario : AvaliacaoFisica
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

    protected Questionario()
    {
    } // EF Constructor

    private Questionario(Guid id) : base(id)
    {
    }

    public double Altura { get; private set; }
    public double Peso { get; private set; }
    public double Imc { get; private set; }
    public ClassificacaoIMC Classificacao { get; private set; }
    public IReadOnlyCollection<Medida> Medidas => _medidas.AsReadOnly();

    public static Questionario Criar(CriarQuestionarioProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Questionario(id);

        instance.SetTipo(TipoAvaliacaoEnum.QUESTIONARIO);
        instance.SetData(props.Data);
        instance.SetAltura(props.Altura);
        instance.SetPeso(props.Peso);
        instance.SetMedidas(props.Medida);
        instance.CalcularIMC();

        return instance;
    }

    public static Questionario Carregar(CriarQuestionarioProps props, Guid id)
    {
        var instance = new Questionario(id);

        instance.SetTipo(TipoAvaliacaoEnum.QUESTIONARIO);
        instance.SetData(props.Data);
        instance.SetAltura(props.Altura);
        instance.SetPeso(props.Peso);
        instance.SetMedidas(props.Medida);
        instance.CalcularIMC();

        return instance;
    }

    private void SetAltura(double altura)
    {
        if (altura <= 0)
            throw new AvaliacaoFisicaException("Altura deve ser maior que zero");

        if (altura > 3)
            throw new AvaliacaoFisicaException("Altura inválida (valor muito alto)");

        Altura = Math.Round(altura, 2);
    }

    private void SetPeso(double peso)
    {
        if (peso <= 0)
            throw new AvaliacaoFisicaException("Peso deve ser maior que zero");

        if (peso > 500)
            throw new AvaliacaoFisicaException("Peso inválido (valor muito alto)");

        Peso = Math.Round(peso, 2);
    }

    private void SetMedidas(List<Medida> medidas)
    {
        if (medidas == null)
            throw new AvaliacaoFisicaException("Medidas devem ser um array");

        if (!medidas.Any())
            throw new AvaliacaoFisicaException("Pelo menos uma medida deve ser informada");

        var medidasInvalidas = medidas.Where(m => m == null).ToList();
        if (medidasInvalidas.Any())
            throw new AvaliacaoFisicaException("Array contém medidas inválidas");

        _medidas.Clear();
        _medidas.AddRange(medidas);
    }

    private void CalcularIMC()
    {
        if (Altura <= 0 || Peso <= 0)
            throw new AvaliacaoFisicaException("Altura e peso devem ser definidos antes de calcular o IMC");

        var valorImc = Peso / Math.Pow(Altura, 2);
        Imc = Math.Round(valorImc, 2);
        Classificacao = DeterminarClassificacaoIMC(Imc);
    }

    private ClassificacaoIMC DeterminarClassificacaoIMC(double imc)
    {
        var faixaEncontrada = ClassificacoesImc.FirstOrDefault(f => imc <= f.LimiteMaximo);
        return faixaEncontrada.Classificacao != default ? faixaEncontrada.Classificacao : ClassificacaoIMC.OUTRO;
    }
}