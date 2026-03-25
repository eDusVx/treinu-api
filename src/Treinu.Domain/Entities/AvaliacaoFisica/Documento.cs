using FluentResults;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Entities.AvaliacaoFisica;

public record CriarDocumentoProps(
    string Nome,
    string Arquivo,
    DateTime Data
);

public class Documento : AvaliacaoFisica
{
    protected Documento()
    {
    } // EF Constructor

    private Documento(Guid id) : base(id)
    {
    }

    public string Nome { get; private set; } = string.Empty;
    public string Arquivo { get; private set; } = string.Empty;

    public static Result<Documento> Criar(CriarDocumentoProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Documento(id);

        var result = Result.Merge(
            instance.SetTipo(TipoAvaliacaoEnum.DOCUMENTO),
            instance.SetData(props.Data),
            instance.SetNome(props.Nome),
            instance.SetArquivo(props.Arquivo)
        );

        if (result.IsFailed) return result;

        return Result.Ok(instance);
    }

    public static Documento Carregar(CriarDocumentoProps props, Guid id)
    {
        var instance = new Documento(id);

        var result = Result.Merge(
            instance.SetTipo(TipoAvaliacaoEnum.DOCUMENTO),
            instance.SetData(props.Data),
            instance.SetNome(props.Nome),
            instance.SetArquivo(props.Arquivo)
        );

        if (result.IsFailed)
            throw new InvalidOperationException($"Erro ao carregar Documento do banco: {result.Errors[0].Message}");

        return instance;
    }

    private Result SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Fail("Nome do documento não pode ser vazio");

        if (nome.Length > 255)
            return Result.Fail("Nome do documento não pode exceder 255 caracteres");

        Nome = nome.Trim();
        return Result.Ok();
    }

    private Result SetArquivo(string arquivo)
    {
        if (string.IsNullOrWhiteSpace(arquivo))
            return Result.Fail("Arquivo não pode ser vazio");

        if (arquivo.Length < 10)
            return Result.Fail("Arquivo inválido (tamanho mínimo não atendido)");

        Arquivo = arquivo;
        return Result.Ok();
    }
}