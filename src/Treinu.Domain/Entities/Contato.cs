using System.Text.RegularExpressions;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;
using Treinu.Domain.Exceptions;

namespace Treinu.Domain.Entities;

public record CriarContatoProps(
    TipoContatoEnum Tipo,
    string Valor,
    string? Descricao = null,
    bool? Principal = null,
    PlataformaRedeSocialEnum? Plataforma = null,
    string? NomeExibicao = null
);

public class Contato : Entity
{
    protected Contato()
    {
    } // EF Constructor

    private Contato(Guid id) : base(id)
    {
    }

    public TipoContatoEnum Tipo { get; private set; }
    public string Valor { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public bool Principal { get; private set; }
    public PlataformaRedeSocialEnum? Plataforma { get; private set; }
    public string? NomeExibicao { get; private set; }

    public static Contato Criar(CriarContatoProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Contato(id);

        instance.SetTipo(props.Tipo);
        instance.SetValor(props.Valor);
        instance.SetDescricao(props.Descricao);
        instance.SetPrincipal(props.Principal ?? false);
        instance.SetNomeExibicao(props.NomeExibicao);

        if (props.Tipo == TipoContatoEnum.REDE_SOCIAL) instance.SetPlataforma(props.Plataforma);

        return instance;
    }

    public static Contato Carregar(CriarContatoProps props, Guid id)
    {
        var instance = new Contato(id);

        instance.SetTipo(props.Tipo);
        instance.SetValor(props.Valor);
        instance.SetDescricao(props.Descricao);
        instance.SetPrincipal(props.Principal ?? false);
        instance.SetNomeExibicao(props.NomeExibicao);

        if (props.Tipo == TipoContatoEnum.REDE_SOCIAL) instance.SetPlataforma(props.Plataforma);

        return instance;
    }

    private void SetTipo(TipoContatoEnum tipo)
    {
        if (!Enum.IsDefined(typeof(TipoContatoEnum), tipo))
            throw new ContatoException("Tipo de contato inválido");

        Tipo = tipo;
    }

    private void SetValor(string valor)
    {
        valor = valor.Trim();

        switch (Tipo)
        {
            case TipoContatoEnum.TELEFONE:
                ValidarTelefone(valor);
                break;
            case TipoContatoEnum.REDE_SOCIAL:
                ValidarRedeSocial(valor);
                break;
            case TipoContatoEnum.SITE:
                ValidarSite(valor);
                break;
        }

        Valor = valor;
    }

    private void ValidarTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            throw new ContatoException("Telefone não pode ser vazio");

        // Allowed characters: digits, plus, spaces, hyphens, parenthesis
        if (!Regex.IsMatch(telefone, @"^\+?[0-9\s\-\(\)]{10,20}$"))
            throw new ContatoException("Número de telefone inválido.");
    }

    private void ValidarRedeSocial(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult) ||
            (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            throw new ContatoException("URL da rede social inválida. Deve incluir http:// ou https://");
    }

    private void ValidarSite(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult) ||
            (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            throw new ContatoException("Site inválido. Deve ser uma URL válida com http:// ou https://");

        if (uriResult.HostNameType != UriHostNameType.Dns) throw new ContatoException("Domínio do site inválido");
    }

    private void SetPlataforma(PlataformaRedeSocialEnum? plataforma)
    {
        if (Tipo == TipoContatoEnum.REDE_SOCIAL && !plataforma.HasValue)
            throw new ContatoException("Plataforma é obrigatória para contatos do tipo REDE_SOCIAL");

        if (plataforma.HasValue && !Enum.IsDefined(typeof(PlataformaRedeSocialEnum), plataforma.Value))
            throw new ContatoException("Plataforma de rede social inválida");

        Plataforma = plataforma;
    }

    private void SetDescricao(string? descricao)
    {
        if (descricao?.Length > 200)
            throw new ContatoException("Descrição deve ter no máximo 200 caracteres");

        Descricao = descricao;
    }

    private void SetPrincipal(bool principal)
    {
        Principal = principal;
    }

    private void SetNomeExibicao(string? nomeExibicao)
    {
        if (nomeExibicao?.Length > 50)
            throw new ContatoException("Nome de exibição deve ter no máximo 50 caracteres");

        NomeExibicao = nomeExibicao;
    }
}