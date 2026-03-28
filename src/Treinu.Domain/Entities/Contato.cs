using System.Text.RegularExpressions;
using FluentResults;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;

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

    public static Result<Contato> Criar(CriarContatoProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Contato(id);

        var result = new Result();
        result.WithReasons(instance.SetTipo(props.Tipo).Reasons);
        result.WithReasons(instance.SetValor(props.Valor).Reasons);
        result.WithReasons(instance.SetDescricao(props.Descricao).Reasons);
        instance.SetPrincipal(props.Principal ?? false);
        result.WithReasons(instance.SetNomeExibicao(props.NomeExibicao).Reasons);

        if (props.Tipo == TipoContatoEnum.REDE_SOCIAL)
            result.WithReasons(instance.SetPlataforma(props.Plataforma).Reasons);

        if (result.IsFailed) return result;

        return Result.Ok(instance);
    }

    public static Contato Carregar(CriarContatoProps props, Guid id)
    {
        var instance = new Contato(id);

        var result = new Result();
        result.WithReasons(instance.SetTipo(props.Tipo).Reasons);
        result.WithReasons(instance.SetValor(props.Valor).Reasons);
        result.WithReasons(instance.SetDescricao(props.Descricao).Reasons);
        instance.SetPrincipal(props.Principal ?? false);
        result.WithReasons(instance.SetNomeExibicao(props.NomeExibicao).Reasons);

        if (props.Tipo == TipoContatoEnum.REDE_SOCIAL)
            result.WithReasons(instance.SetPlataforma(props.Plataforma).Reasons);

        if (result.IsFailed)
            throw new InvalidOperationException($"Erro ao carregar Contato do banco: {result.Errors[0].Message}");

        return instance;
    }

    private Result SetTipo(TipoContatoEnum tipo)
    {
        if (!Enum.IsDefined(typeof(TipoContatoEnum), tipo))
            return Result.Fail("Tipo de contato inválido");

        Tipo = tipo;
        return Result.Ok();
    }

    private Result SetValor(string valor)
    {
        valor = valor.Trim();

        var validacao = Result.Ok();

        switch (Tipo)
        {
            case TipoContatoEnum.TELEFONE:
                validacao = ValidarTelefone(valor);
                break;
            case TipoContatoEnum.REDE_SOCIAL:
                validacao = ValidarRedeSocial(valor);
                break;
            case TipoContatoEnum.SITE:
                validacao = ValidarSite(valor);
                break;
        }

        if (validacao.IsFailed) return validacao;

        Valor = valor;
        return Result.Ok();
    }

    private Result ValidarTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            return Result.Fail("Telefone não pode ser vazio");

        if (!Regex.IsMatch(telefone, @"^\+?[0-9\s\-\(\)]{10,20}$"))
            return Result.Fail("Número de telefone inválido.");

        return Result.Ok();
    }

    private Result ValidarRedeSocial(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult) ||
            (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            return Result.Fail("URL da rede social inválida. Deve incluir http:// ou https://");

        return Result.Ok();
    }

    private Result ValidarSite(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult) ||
            (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            return Result.Fail("Site inválido. Deve ser uma URL válida com http:// ou https://");

        if (uriResult.HostNameType != UriHostNameType.Dns) return Result.Fail("Domínio do site inválido");

        return Result.Ok();
    }

    private Result SetPlataforma(PlataformaRedeSocialEnum? plataforma)
    {
        if (Tipo == TipoContatoEnum.REDE_SOCIAL && !plataforma.HasValue)
            return Result.Fail("Plataforma é obrigatória para contatos do tipo REDE_SOCIAL");

        if (plataforma.HasValue && !Enum.IsDefined(typeof(PlataformaRedeSocialEnum), plataforma.Value))
            return Result.Fail("Plataforma de rede social inválida");

        Plataforma = plataforma;
        return Result.Ok();
    }

    private Result SetDescricao(string? descricao)
    {
        if (descricao?.Length > 200)
            return Result.Fail("Descrição deve ter no máximo 200 caracteres");

        Descricao = descricao;
        return Result.Ok();
    }

    private void SetPrincipal(bool principal)
    {
        Principal = principal;
    }

    private Result SetNomeExibicao(string? nomeExibicao)
    {
        if (nomeExibicao?.Length > 50)
            return Result.Fail("Nome de exibição deve ter no máximo 50 caracteres");

        NomeExibicao = nomeExibicao;
        return Result.Ok();
    }
}