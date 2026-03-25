using FluentResults;
using Treinu.Domain.Dtos;
using Treinu.Domain.Enums;
using Treinu.Domain.Errors;
using Treinu.Domain.Events;

namespace Treinu.Domain.Entities;

public record CriarTreinadorProps(
    string NomeCompleto,
    string Email,
    string Senha,
    DateTime DataNascimento,
    GeneroEnum Genero,
    List<Contato> Contato,
    string Cpf,
    bool Ativo,
    bool AceiteTermoAdesao,
    List<Certificado>? Certificados = null,
    List<string>? Especializacoes = null
);

public class Treinador : Usuario
{
    private readonly List<Certificado> _certificados = new();

    private readonly List<string> _especializacoes = new();

    protected Treinador()
    {
    }

    private Treinador(Guid id) : base(id)
    {
    }

    public IReadOnlyCollection<Certificado> Certificados => _certificados.AsReadOnly();
    public IReadOnlyCollection<string> Especializacoes => _especializacoes.AsReadOnly();

    public static Result<Treinador> Criar(CriarTreinadorProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Treinador(id);

        var rs1 = instance.SetCpf(props.Cpf);
        if (rs1.IsFailed) return Result.Fail<Treinador>(rs1.Errors);
        var rs2 = instance.SetAtivo(props.Ativo);
        if (rs2.IsFailed) return Result.Fail<Treinador>(rs2.Errors);
        var rs3 = instance.SetAceiteTermoAdesao(props.AceiteTermoAdesao);
        if (rs3.IsFailed) return Result.Fail<Treinador>(rs3.Errors);
        var rs4 = instance.SetDataNascimento(props.DataNascimento);
        if (rs4.IsFailed) return Result.Fail<Treinador>(rs4.Errors);
        var rs5 = instance.SetEmail(props.Email);
        if (rs5.IsFailed) return Result.Fail<Treinador>(rs5.Errors);
        var rs6 = instance.SetGenero(props.Genero);
        if (rs6.IsFailed) return Result.Fail<Treinador>(rs6.Errors);
        var rs7 = instance.SetNomeCompleto(props.NomeCompleto);
        if (rs7.IsFailed) return Result.Fail<Treinador>(rs7.Errors);
        var rs8 = instance.SetPerfil(PerfilEnum.TREINADOR);
        if (rs8.IsFailed) return Result.Fail<Treinador>(rs8.Errors);
        var rs9 = instance.SetSenha(props.Senha);
        if (rs9.IsFailed) return Result.Fail<Treinador>(rs9.Errors);
        var rs10 = instance.SetContato(props.Contato ?? new List<Contato>());
        if (rs10.IsFailed) return Result.Fail<Treinador>(rs10.Errors);
        var rs11 = instance.SetCertificados(props.Certificados ?? new List<Certificado>());
        if (rs11.IsFailed) return Result.Fail<Treinador>(rs11.Errors);
        var rs12 = instance.SetEspecializacoes(props.Especializacoes ?? new List<string>());
        if (rs12.IsFailed) return Result.Fail<Treinador>(rs12.Errors);

        instance.Apply(
            new UsuarioCadastradoEvent(
                instance.Id,
                instance.Email,
                instance.Senha,
                instance.Perfil,
                instance.Ativo
            )
        );

        return Result.Ok(instance);
    }

    public static Result<Treinador> Carregar(CriarTreinadorProps props, Guid id)
    {
        var instance = new Treinador(id);

        var rs1 = instance.SetCpf(props.Cpf);
        if (rs1.IsFailed) return Result.Fail<Treinador>(rs1.Errors);
        var rs2 = instance.SetAtivo(props.Ativo);
        if (rs2.IsFailed) return Result.Fail<Treinador>(rs2.Errors);
        var rs3 = instance.SetContato(props.Contato ?? new List<Contato>());
        if (rs3.IsFailed) return Result.Fail<Treinador>(rs3.Errors);
        var rs4 = instance.SetAceiteTermoAdesao(props.AceiteTermoAdesao);
        if (rs4.IsFailed) return Result.Fail<Treinador>(rs4.Errors);
        var rs5 = instance.SetDataNascimento(props.DataNascimento);
        if (rs5.IsFailed) return Result.Fail<Treinador>(rs5.Errors);
        var rs6 = instance.SetEmail(props.Email);
        if (rs6.IsFailed) return Result.Fail<Treinador>(rs6.Errors);
        var rs7 = instance.SetGenero(props.Genero);
        if (rs7.IsFailed) return Result.Fail<Treinador>(rs7.Errors);
        var rs8 = instance.SetNomeCompleto(props.NomeCompleto);
        if (rs8.IsFailed) return Result.Fail<Treinador>(rs8.Errors);
        var rs9 = instance.SetPerfil(PerfilEnum.TREINADOR);
        if (rs9.IsFailed) return Result.Fail<Treinador>(rs9.Errors);
        instance.CarregarSenha(props.Senha);
        var rs10 = instance.SetCertificados(props.Certificados ?? new List<Certificado>());
        if (rs10.IsFailed) return Result.Fail<Treinador>(rs10.Errors);
        var rs11 = instance.SetEspecializacoes(props.Especializacoes ?? new List<string>());
        if (rs11.IsFailed) return Result.Fail<Treinador>(rs11.Errors);

        return Result.Ok(instance);
    }

    private Result SetCertificados(List<Certificado> certificados)
    {
        if (certificados == null) return Result.Fail(DomainErrors.Usuario.DadosVazios);
        _certificados.Clear();
        _certificados.AddRange(certificados);
        return Result.Ok();
    }

    private Result SetEspecializacoes(List<string> especializacoes)
    {
        if (especializacoes == null) return Result.Fail(DomainErrors.Usuario.DadosVazios);
        _especializacoes.Clear();
        _especializacoes.AddRange(especializacoes);
        return Result.Ok();
    }

    public Result AdicionarCertificado(Certificado certificado)
    {
        if (certificado == null) return Result.Fail(DomainErrors.Usuario.DadosVazios);
        _certificados.Add(certificado);
        return Result.Ok();
    }

    public Result AdicionarEspecializacao(string especializacao)
    {
        if (string.IsNullOrWhiteSpace(especializacao)) return Result.Fail(DomainErrors.Usuario.DadosVazios);
        _especializacoes.Add(especializacao);
        return Result.Ok();
    }

    public override object ToDto()
    {
        return new TreinadorDto(
            Id, NomeCompleto, Email, DataNascimento, Genero, Contato, Cpf, Perfil, Ativo,
            AceiteTermoAdesao, Certificados, Especializacoes
        );
    }
}