using Treinu.Domain.Enums;
using Treinu.Domain.Exceptions;

namespace Treinu.Domain.Entities.AvaliacaoFisica;

public record CriarDocumentoProps(
    string Nome,
    string Arquivo,
    DateTime Data
);

public class Documento : AvaliacaoFisica
{
    public string Nome { get; private set; } = string.Empty;
    public string Arquivo { get; private set; } = string.Empty;

    protected Documento() : base() { } // EF Constructor

    private Documento(Guid id) : base(id)
    {
    }

    public static Documento Criar(CriarDocumentoProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Documento(id);
        
        instance.SetTipo(TipoAvaliacaoEnum.DOCUMENTO);
        instance.SetData(props.Data);
        instance.SetNome(props.Nome);
        instance.SetArquivo(props.Arquivo);

        return instance;
    }

    public static Documento Carregar(CriarDocumentoProps props, Guid id)
    {
        var instance = new Documento(id);
        
        instance.SetTipo(TipoAvaliacaoEnum.DOCUMENTO);
        instance.SetData(props.Data);
        instance.SetNome(props.Nome);
        instance.SetArquivo(props.Arquivo);

        return instance;
    }

    private void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new AvaliacaoFisicaException("Nome do documento não pode ser vazio");

        if (nome.Length > 255)
            throw new AvaliacaoFisicaException("Nome do documento não pode exceder 255 caracteres");

        Nome = nome.Trim();
    }

    private void SetArquivo(string arquivo)
    {
        if (string.IsNullOrWhiteSpace(arquivo))
            throw new AvaliacaoFisicaException("Arquivo não pode ser vazio");

        if (arquivo.Length < 10)
            throw new AvaliacaoFisicaException("Arquivo inválido (tamanho mínimo não atendido)");

        Arquivo = arquivo;
    }
}
