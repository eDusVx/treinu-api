using FluentResults;
using Treinu.Domain.Dtos;
using Treinu.Domain.Enums;
using Treinu.Domain.Errors;
using Treinu.Domain.Events;

namespace Treinu.Domain.Entities;

public record CriarAlunoProps(
    string NomeCompleto,
    string Email,
    string Senha,
    DateTime DataNascimento,
    GeneroEnum Genero,
    List<Contato> Contato,
    string Cpf,
    bool Ativo,
    bool AceiteTermoAdesao,
    ObjetivoEnum Objetivo,
    List<AvaliacaoFisica.AvaliacaoFisica>? AvaliacaoFisica = null
);

public class Aluno : Usuario
{
    private readonly List<AvaliacaoFisica.AvaliacaoFisica> _avaliacaoFisica = new();

    protected Aluno()
    {
    }

    private Aluno(Guid id) : base(id)
    {
    }

    public ObjetivoEnum Objetivo { get; private set; }
    public IReadOnlyCollection<AvaliacaoFisica.AvaliacaoFisica> AvaliacaoFisica => _avaliacaoFisica.AsReadOnly();

    public static Result<Aluno> Criar(CriarAlunoProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Aluno(id);

        var rs1 = instance.SetCpf(props.Cpf);
        if (rs1.IsFailed) return Result.Fail<Aluno>(rs1.Errors);
        var rs2 = instance.SetAtivo(props.Ativo);
        if (rs2.IsFailed) return Result.Fail<Aluno>(rs2.Errors);
        var rs3 = instance.SetAceiteTermoAdesao(props.AceiteTermoAdesao);
        if (rs3.IsFailed) return Result.Fail<Aluno>(rs3.Errors);
        var rs4 = instance.SetDataNascimento(props.DataNascimento);
        if (rs4.IsFailed) return Result.Fail<Aluno>(rs4.Errors);
        var rs5 = instance.SetEmail(props.Email);
        if (rs5.IsFailed) return Result.Fail<Aluno>(rs5.Errors);
        var rs6 = instance.SetGenero(props.Genero);
        if (rs6.IsFailed) return Result.Fail<Aluno>(rs6.Errors);
        var rs7 = instance.SetNomeCompleto(props.NomeCompleto);
        if (rs7.IsFailed) return Result.Fail<Aluno>(rs7.Errors);
        var rs8 = instance.SetPerfil(PerfilEnum.ALUNO);
        if (rs8.IsFailed) return Result.Fail<Aluno>(rs8.Errors);
        var rs9 = instance.SetSenha(props.Senha);
        if (rs9.IsFailed) return Result.Fail<Aluno>(rs9.Errors);
        var rs10 = instance.SetContato(props.Contato ?? new List<Contato>());
        if (rs10.IsFailed) return Result.Fail<Aluno>(rs10.Errors);
        var rs11 = instance.SetObjetivo(props.Objetivo);
        if (rs11.IsFailed) return Result.Fail<Aluno>(rs11.Errors);
        var rs12 = instance.SetAvaliacaoFisica(props.AvaliacaoFisica ?? new List<AvaliacaoFisica.AvaliacaoFisica>());
        if (rs12.IsFailed) return Result.Fail<Aluno>(rs12.Errors);

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

    public static Result<Aluno> Carregar(CriarAlunoProps props, Guid id)
    {
        var instance = new Aluno(id);

        var rs1 = instance.SetCpf(props.Cpf);
        if (rs1.IsFailed) return Result.Fail<Aluno>(rs1.Errors);
        var rs2 = instance.SetAtivo(props.Ativo);
        if (rs2.IsFailed) return Result.Fail<Aluno>(rs2.Errors);
        var rs3 = instance.SetContato(props.Contato ?? new List<Contato>());
        if (rs3.IsFailed) return Result.Fail<Aluno>(rs3.Errors);
        var rs4 = instance.SetAceiteTermoAdesao(props.AceiteTermoAdesao);
        if (rs4.IsFailed) return Result.Fail<Aluno>(rs4.Errors);
        var rs5 = instance.SetDataNascimento(props.DataNascimento);
        if (rs5.IsFailed) return Result.Fail<Aluno>(rs5.Errors);
        var rs6 = instance.SetEmail(props.Email);
        if (rs6.IsFailed) return Result.Fail<Aluno>(rs6.Errors);
        var rs7 = instance.SetGenero(props.Genero);
        if (rs7.IsFailed) return Result.Fail<Aluno>(rs7.Errors);
        var rs8 = instance.SetNomeCompleto(props.NomeCompleto);
        if (rs8.IsFailed) return Result.Fail<Aluno>(rs8.Errors);
        var rs9 = instance.SetPerfil(PerfilEnum.ALUNO);
        if (rs9.IsFailed) return Result.Fail<Aluno>(rs9.Errors);
        instance.CarregarSenha(props.Senha);
        var rs10 = instance.SetObjetivo(props.Objetivo);
        if (rs10.IsFailed) return Result.Fail<Aluno>(rs10.Errors);
        var rs11 = instance.SetAvaliacaoFisica(props.AvaliacaoFisica ?? new List<AvaliacaoFisica.AvaliacaoFisica>());
        if (rs11.IsFailed) return Result.Fail<Aluno>(rs11.Errors);

        return Result.Ok(instance);
    }

    private Result SetObjetivo(ObjetivoEnum objetivo)
    {
        if (!Enum.IsDefined(typeof(ObjetivoEnum), objetivo))
            return Result.Fail(DomainErrors.Usuario.ObjetivoInvalido);

        Objetivo = objetivo;
        return Result.Ok();
    }

    private Result SetAvaliacaoFisica(List<AvaliacaoFisica.AvaliacaoFisica> avaliacaoFisica)
    {
        if (avaliacaoFisica == null)
            return Result.Fail(DomainErrors.Usuario.DadosVazios);

        var avaliacoesInvalidas = avaliacaoFisica.Where(av => av == null).ToList();
        if (avaliacoesInvalidas.Any())
            return Result.Fail(DomainErrors.Usuario.DadosVazios);

        _avaliacaoFisica.Clear();
        _avaliacaoFisica.AddRange(avaliacaoFisica);
        return Result.Ok();
    }

    public AlunoDto ToDto()
    {
        return new AlunoDto(
            Id, NomeCompleto, Email, DataNascimento, Genero, Contato, Cpf, Perfil, Ativo,
            AceiteTermoAdesao, Objetivo, AvaliacaoFisica
        );
    }
}