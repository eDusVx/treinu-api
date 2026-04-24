using FluentResults;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Entities.AvaliacaoFisica;

public abstract class AvaliacaoFisica : AggregateRoot
{
    protected AvaliacaoFisica()
    {
    } // EF Constructor

    protected AvaliacaoFisica(Guid id) : base(id)
    {
    }

    public TipoAvaliacaoEnum Tipo { get; protected set; }
    public DateTime Data { get; protected set; }

    protected Result SetTipo(TipoAvaliacaoEnum tipo)
    {
        if (!Enum.IsDefined(typeof(TipoAvaliacaoEnum), tipo))
        {
            var permitidos = string.Join(", ", Enum.GetNames(typeof(TipoAvaliacaoEnum)));
            return Result.Fail($"Tipo de avaliação inválido. Valores permitidos: {permitidos}");
        }

        Tipo = tipo;
        return Result.Ok();
    }

    protected Result SetData(DateTime data)
    {
        if (data == default)
            return Result.Fail("Data da avaliação não pode ser vazia");

        var hoje = DateTime.UtcNow.Date;
        if (data > hoje)
            return Result.Fail("Data da avaliação não pode ser no futuro");

        var cemAnosAtras = hoje.AddYears(-100);
        if (data < cemAnosAtras)
            return Result.Fail("Data da avaliação muito antiga");

        Data = data;
        return Result.Ok();
    }
}