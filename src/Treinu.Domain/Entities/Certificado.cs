using FluentResults;
using Treinu.Domain.Core;

namespace Treinu.Domain.Entities;

public record CriarCertificadoProps(
    string Nome,
    string ArquivoPdf,
    DateTime DataUpload,
    bool Validado
);

public class Certificado : Entity
{
    protected Certificado()
    {
    } // EF Constructor

    private Certificado(Guid id) : base(id)
    {
    }

    public string Nome { get; private set; } = string.Empty;
    public string ArquivoPdf { get; private set; } = string.Empty;
    public DateTime DataUpload { get; private set; }
    public bool Validado { get; private set; }

    public static Result<Certificado> Criar(CriarCertificadoProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Certificado(id);

        var result = Result.Merge(
            instance.SetNome(props.Nome),
            instance.SetArquivoPdf(props.ArquivoPdf),
            instance.SetDataUpload(props.DataUpload)
        );
        
        instance.SetValidado(props.Validado);

        if (result.IsFailed) return result;

        return Result.Ok(instance);
    }

    public static Certificado Carregar(CriarCertificadoProps props, Guid id)
    {
        var instance = new Certificado(id);

        var result = Result.Merge(
            instance.SetNome(props.Nome),
            instance.SetArquivoPdf(props.ArquivoPdf),
            instance.SetDataUpload(props.DataUpload)
        );
        
        instance.SetValidado(props.Validado);

        if (result.IsFailed) 
            throw new InvalidOperationException($"Erro ao carregar Certificado do banco: {result.Errors[0].Message}");

        return instance;
    }

    private Result SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Fail("Nome não pode ser vazio");

        if (nome.Length > 100)
            return Result.Fail("Nome não pode ter mais de 100 caracteres");

        Nome = nome.Trim();
        return Result.Ok();
    }

    private Result SetArquivoPdf(string arquivoPdf)
    {
        if (string.IsNullOrWhiteSpace(arquivoPdf))
            return Result.Fail("Arquivo PDF não pode ser vazio");

        if (arquivoPdf.Length < 10)
            return Result.Fail("Arquivo PDF inválido");

        ArquivoPdf = arquivoPdf;
        return Result.Ok();
    }

    private Result SetDataUpload(DateTime dataUpload)
    {
        if (dataUpload == default)
            return Result.Fail("Data de upload inválida");

        if (dataUpload > DateTime.Now)
            return Result.Fail("Data de upload não pode ser no futuro");

        DataUpload = dataUpload;
        return Result.Ok();
    }

    private void SetValidado(bool validado)
    {
        Validado = validado;
    }
}