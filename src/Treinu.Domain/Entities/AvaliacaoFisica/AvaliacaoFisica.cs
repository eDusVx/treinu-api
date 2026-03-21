using Treinu.Domain.Core;
using Treinu.Domain.Enums;
using Treinu.Domain.Exceptions;

namespace Treinu.Domain.Entities.AvaliacaoFisica;

public abstract class AvaliacaoFisica : AggregateRoot
{
    public TipoAvaliacaoEnum Tipo { get; protected set; }
    public DateTime Data { get; protected set; }

    protected AvaliacaoFisica() { } // EF Constructor

    protected AvaliacaoFisica(Guid id) : base(id)
    {
    }

    protected void SetTipo(TipoAvaliacaoEnum tipo)
    {
        if (!Enum.IsDefined(typeof(TipoAvaliacaoEnum), tipo))
        {
            var permitidos = string.Join(", ", Enum.GetNames(typeof(TipoAvaliacaoEnum)));
            throw new AvaliacaoFisicaException($"Tipo de avaliação inválido. Valores permitidos: {permitidos}");
        }

        Tipo = tipo;
    }

    protected void SetData(DateTime data)
    {
        if (data == default)
            throw new AvaliacaoFisicaException("Data da avaliação não pode ser vazia");

        var hoje = DateTime.Now.Date;
        if (data > hoje)
            throw new AvaliacaoFisicaException("Data da avaliação não pode ser no futuro");

        var cemAnosAtras = hoje.AddYears(-100);
        if (data < cemAnosAtras)
            throw new AvaliacaoFisicaException("Data da avaliação muito antiga");

        Data = data;
    }
}
