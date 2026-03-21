using Treinu.Domain.Core;
using Treinu.Domain.Enums;
using Treinu.Domain.Exceptions;

namespace Treinu.Domain.Entities.AvaliacaoFisica;

public record CriarMedidaProps(
    ChaveMedidaEnum Chave,
    decimal Valor
);

public class Medida : Entity
{
    public ChaveMedidaEnum Chave { get; private set; }
    public decimal Valor { get; private set; }

    protected Medida() { } // EF Constructor

    private Medida(Guid id) : base(id)
    {
    }

    public static Medida Criar(CriarMedidaProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Medida(id);
        
        instance.SetChave(props.Chave);
        instance.SetValor(props.Valor);
        
        return instance;
    }

    public static Medida Carregar(CriarMedidaProps props, Guid id)
    {
        var instance = new Medida(id);
        
        instance.SetChave(props.Chave);
        instance.SetValor(props.Valor);
        
        return instance;
    }

    private void SetChave(ChaveMedidaEnum chave)
    {
        if (!Enum.IsDefined(typeof(ChaveMedidaEnum), chave))
        {
            var permitidos = string.Join(", ", Enum.GetNames(typeof(ChaveMedidaEnum)));
            throw new MedidaException($"Chave inválida. Valores aceitos: {permitidos}");
        }

        Chave = chave;
    }

    private void SetValor(decimal valor)
    {
        if (valor <= 0)
            throw new MedidaException("Valor deve ser maior que zero");

        if (valor > 500)
            throw new MedidaException("Valor excede o limite máximo (500)");

        Valor = Math.Round(valor, 1);
    }
}
