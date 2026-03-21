using Treinu.Domain.Core;
using Treinu.Domain.Exceptions;

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

    public static Certificado Criar(CriarCertificadoProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Certificado(id);

        instance.SetNome(props.Nome);
        instance.SetArquivoPdf(props.ArquivoPdf);
        instance.SetDataUpload(props.DataUpload);
        instance.SetValidado(props.Validado);

        return instance;
    }

    public static Certificado Carregar(CriarCertificadoProps props, Guid id)
    {
        var instance = new Certificado(id);

        instance.SetNome(props.Nome);
        instance.SetArquivoPdf(props.ArquivoPdf);
        instance.SetDataUpload(props.DataUpload);
        instance.SetValidado(props.Validado);

        return instance;
    }

    private void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new CertificadoException("Nome não pode ser vazio");

        if (nome.Length > 100)
            throw new CertificadoException("Nome não pode ter mais de 100 caracteres");

        Nome = nome.Trim();
    }

    private void SetArquivoPdf(string arquivoPdf)
    {
        if (string.IsNullOrWhiteSpace(arquivoPdf))
            throw new CertificadoException("Arquivo PDF não pode ser vazio");

        if (arquivoPdf.Length < 10)
            throw new CertificadoException("Arquivo PDF inválido");

        ArquivoPdf = arquivoPdf;
    }

    private void SetDataUpload(DateTime dataUpload)
    {
        if (dataUpload == default)
            throw new CertificadoException("Data de upload inválida");

        if (dataUpload > DateTime.Now)
            throw new CertificadoException("Data de upload não pode ser no futuro");

        DataUpload = dataUpload;
    }

    private void SetValidado(bool validado)
    {
        Validado = validado;
    }
}