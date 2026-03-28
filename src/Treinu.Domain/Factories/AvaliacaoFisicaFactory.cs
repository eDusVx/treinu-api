using FluentResults;
using Treinu.Domain.Dtos;
using Treinu.Domain.Entities.AvaliacaoFisica;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Factories;

public class AvaliacaoFisicaFactory
{
    public Result<List<AvaliacaoFisica>> Fabricar(IEnumerable<AvaliacaoFisicaDto> props)
    {
        var avaliacoesFisicas = new List<AvaliacaoFisica>();
        var result = new Result<List<AvaliacaoFisica>>();

        foreach (var avaliacao in props)
            switch (avaliacao.ContextoAvaliacao)
            {
                case TipoAvaliacaoEnum.DOCUMENTO:
                    if (avaliacao is DocumentoDto documentoDto)
                    {
                        var docResult = CriarDocumento(documentoDto);
                        if (docResult.IsFailed) result.WithReasons(docResult.Reasons);
                        else avaliacoesFisicas.Add(docResult.Value);
                    }

                    break;
                case TipoAvaliacaoEnum.QUESTIONARIO:
                    if (avaliacao is QuestionarioDto questionarioDto)
                    {
                        var questResult = CriarQuestionario(questionarioDto);
                        if (questResult.IsFailed) result.WithReasons(questResult.Reasons);
                        else avaliacoesFisicas.Add(questResult.Value);
                    }

                    break;
                default:
                    result.WithError($"Tipo de avaliacao física inválida: {avaliacao.ContextoAvaliacao}");
                    break;
            }

        if (result.IsFailed) return result;
        return Result.Ok(avaliacoesFisicas);
    }

    private Result<Questionario> CriarQuestionario(QuestionarioDto props)
    {
        var medidasResult = CriarMedidas(props.Medidas);
        if (medidasResult.IsFailed) return Result.Fail<Questionario>(medidasResult.Errors);

        return Questionario.Criar(new CriarQuestionarioProps(
            props.Altura,
            props.Peso,
            medidasResult.Value,
            props.Data
        ));
    }

    private Result<Documento> CriarDocumento(DocumentoDto documento)
    {
        return Documento.Criar(new CriarDocumentoProps(
            documento.Nome,
            documento.Arquivo,
            documento.Data
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