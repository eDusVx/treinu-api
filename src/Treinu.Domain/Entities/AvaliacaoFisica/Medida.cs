using FluentResults;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Entities.AvaliacaoFisica;

public record CriarMedidaProps(
    ChaveMedidaEnum Chave,
    decimal Valor
);

public class Medida : Entity
{
    protected Medida()
    {
    } // EF Constructor

    private Medida(Guid id) : base(id)
    {
    }

    public ChaveMedidaEnum Chave { get; private set; }
    public decimal Valor { get; private set; }

    public static Result<Medida> Criar(CriarMedidaProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Medida(id);

        var result = Result.Merge(
            instance.SetChave(props.Chave),
            instance.SetValor(props.Valor)
        );

        if (result.IsFailed) return result;

        return Result.Ok(instance);
    }

    public static Medida Carregar(CriarMedidaProps props, Guid id)
    {
        var instance = new Medida(id);

        var result = Result.Merge(
            instance.SetChave(props.Chave),
            instance.SetValor(props.Valor)
        );

        if (result.IsFailed)
            throw new InvalidOperationException($"Erro ao carregar Medida do banco: {result.Errors[0].Message}");

        return instance;
    }

    private Result SetChave(ChaveMedidaEnum chave)
    {
        if (!Enum.IsDefined(typeof(ChaveMedidaEnum), chave))
        {
            var permitidos = string.Join(", ", Enum.GetNames(typeof(ChaveMedidaEnum)));
            return Result.Fail($"Chave inválida. Valores aceitos: {permitidos}");
        }

        Chave = chave;
        return Result.Ok();
    }

    private Result SetValor(decimal valor)
    {
        if (valor <= 0)
            return Result.Fail("Valor deve ser maior que zero");

        if (valor > 500)
            return Result.Fail("Valor excede o limite máximo (500)");

        Valor = Math.Round(valor, 1);
        return Result.Ok();
    }
}