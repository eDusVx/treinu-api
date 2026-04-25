using FluentResults;
using Treinu.Domain.Dtos;
using Treinu.Domain.Entities.AvaliacaoFisica;

namespace Treinu.Domain.Factories;

public class AvaliacaoFisicaFactory
{
    public Result<List<Treinu.Domain.Entities.AvaliacaoFisica.AvaliacaoFisica>> Fabricar(IEnumerable<AvaliacaoFisicaDto> props)
    {
        var avaliacoesFisicas = new List<Treinu.Domain.Entities.AvaliacaoFisica.AvaliacaoFisica>();
        var result = new Result<List<Treinu.Domain.Entities.AvaliacaoFisica.AvaliacaoFisica>>();

        foreach (var avaliacao in props)
        {
            var questResult = CriarAvaliacaoFisica(avaliacao);
            if (questResult.IsFailed) result.WithReasons(questResult.Reasons);
            else avaliacoesFisicas.Add(questResult.Value);
        }

        if (result.IsFailed) return result;
        return Result.Ok(avaliacoesFisicas);
    }

    private Result<Treinu.Domain.Entities.AvaliacaoFisica.AvaliacaoFisica> CriarAvaliacaoFisica(AvaliacaoFisicaDto props)
    {
        var medidasResult = CriarMedidas(props.Medidas);
        if (medidasResult.IsFailed) return Result.Fail<Treinu.Domain.Entities.AvaliacaoFisica.AvaliacaoFisica>(medidasResult.Errors);

        return Treinu.Domain.Entities.AvaliacaoFisica.AvaliacaoFisica.Criar(new CriarAvaliacaoFisicaProps(
            props.Altura,
            props.Peso,
            medidasResult.Value,
            props.Data
        ));
    }

    private Result<List<Medida>> CriarMedidas(IEnumerable<MedidaDto> medidasDto)
    {
        var result = new Result<List<Medida>>();
        var medidas = new List<Medida>();

        foreach (var dto in medidasDto)
        {
            var medidaResult = Medida.Criar(new CriarMedidaProps(dto.Chave, dto.Valor));
            if (medidaResult.IsFailed)
                result.WithReasons(medidaResult.Reasons);
            else
                medidas.Add(medidaResult.Value);
        }

        if (result.IsFailed) return result;
        return Result.Ok(medidas);
    }
}