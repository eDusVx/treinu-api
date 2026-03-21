using Treinu.Domain.Dtos;
using Treinu.Domain.Entities.AvaliacaoFisica;
using Treinu.Domain.Enums;
using Treinu.Domain.Exceptions;

namespace Treinu.Domain.Factories;

public class AvaliacaoFisicaFactory
{
    public List<AvaliacaoFisica> Fabricar(IEnumerable<AvaliacaoFisicaDto> props)
    {
        var avaliacoesFisicas = new List<AvaliacaoFisica>();

        foreach (var avaliacao in props)
            switch (avaliacao.ContextoAvaliacao)
            {
                case TipoAvaliacaoEnum.DOCUMENTO:
                    if (avaliacao is DocumentoDto documentoDto) avaliacoesFisicas.Add(CriarDocumento(documentoDto));
                    break;
                case TipoAvaliacaoEnum.QUESTIONARIO:
                    if (avaliacao is QuestionarioDto questionarioDto)
                        avaliacoesFisicas.Add(CriarQuestionario(questionarioDto));
                    break;
                default:
                    throw new UsuarioException($"Tipo de avaliacao física inválida: {avaliacao.ContextoAvaliacao}");
            }

        return avaliacoesFisicas;
    }

    private Questionario CriarQuestionario(QuestionarioDto props)
    {
        var medidas = CriarMedidas(props.Medidas);

        return Questionario.Criar(new CriarQuestionarioProps(
            props.Altura,
            props.Peso,
            medidas,
            props.Data
        ));
    }

    private Documento CriarDocumento(DocumentoDto documento)
    {
        return Documento.Criar(new CriarDocumentoProps(
            documento.Nome,
            documento.Arquivo,
            documento.Data
        ));
    }

    private List<Medida> CriarMedidas(IEnumerable<MedidaDto> medidas)
    {
        return medidas.Select(medida => Medida.Criar(new CriarMedidaProps(
            medida.Chave,
            medida.Valor
        ))).ToList();
    }
}