using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FluentResults;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;
using Treinu.Domain.Errors;

namespace Treinu.Domain.Entities;

public record CriarUsuarioProps(
    string NomeCompleto,
    string Email,
    string Senha,
    DateTime DataNascimento,
    GeneroEnum Genero,
    List<Contato> Contato,
    string Cpf,
    bool Ativo,
    bool AceiteTermoAdesao
);

public abstract class Usuario : AggregateRoot
{
    private readonly List<Contato> _contato = new();

    protected Usuario()
    {
    }

    protected Usuario(Guid id) : base(id)
    {
    }

    public string NomeCompleto { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Senha { get; private set; } = string.Empty;
    public DateTime DataNascimento { get; private set; }
    public GeneroEnum Genero { get; private set; }
    public IReadOnlyCollection<Contato> Contato => _contato.AsReadOnly();

    public string Cpf { get; private set; } = string.Empty;
    public PerfilEnum Perfil { get; private set; }
    public bool Ativo { get; private set; }
    public bool AceiteTermoAdesao { get; private set; }

    protected Result SetNomeCompleto(string nomeCompleto)
    {
        if (string.IsNullOrWhiteSpace(nomeCompleto))
            return Result.Fail(DomainErrors.Usuario.DadosVazios);

        NomeCompleto = nomeCompleto;
        return Result.Ok();
    }

    protected Result SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Fail(DomainErrors.Usuario.EmailInvalido);

        if (!new EmailAddressAttribute().IsValid(email))
            return Result.Fail(DomainErrors.Usuario.EmailInvalido);

        Email = email;
        return Result.Ok();
    }

    protected Result SetSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            return Result.Fail(DomainErrors.Usuario.SenhaFraca);

        if (senha.Length > 20)
            return Result.Fail(DomainErrors.Usuario.SenhaFraca);

        var rx = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&.]{8,}$");
        if (!rx.IsMatch(senha))
            return Result.Fail(DomainErrors.Usuario.SenhaFraca);

        Senha = BCrypt.Net.BCrypt.HashPassword(senha, 10);
        return Result.Ok();
    }

    protected Result SetDataNascimento(DateTime dataNascimento)
    {
        if (dataNascimento == default)
            return Result.Fail(DomainErrors.Usuario.DataNascimentoInvalida);

        var hoje = DateTime.Now.Date;
        var minimoData = new DateTime(1930, 1, 1);

        if (dataNascimento >= hoje || dataNascimento <= minimoData)
            return Result.Fail(DomainErrors.Usuario.DataNascimentoInvalida);

        DataNascimento = dataNascimento;
        return Result.Ok();
    }

    protected Result SetGenero(GeneroEnum genero)
    {
        if (!Enum.IsDefined(typeof(GeneroEnum), genero))
            return Result.Fail(DomainErrors.Usuario.GeneroInvalido);

        Genero = genero;
        return Result.Ok();
    }

    protected Result SetContato(List<Contato> contato)
    {
        if (contato == null)
            return Result.Fail(DomainErrors.Usuario.DadosVazios);

        _contato.Clear();
        _contato.AddRange(contato);
        return Result.Ok();
    }

    protected Result SetCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return Result.Fail(DomainErrors.Usuario.CpfInvalido);

        if (!IsCpfValido(cpf))
            return Result.Fail(DomainErrors.Usuario.CpfInvalido);

        Cpf = cpf;
        return Result.Ok();
    }

    protected Result SetPerfil(PerfilEnum perfil)
    {
        if (!Enum.IsDefined(typeof(PerfilEnum), perfil))
            return Result.Fail(DomainErrors.Usuario.DadosVazios);

        Perfil = perfil;
        return Result.Ok();
    }

    protected Result SetAtivo(bool ativo)
    {
        Ativo = ativo;
        return Result.Ok();
    }

    protected Result SetAceiteTermoAdesao(bool aceiteTermoAdesao)
    {
        AceiteTermoAdesao = aceiteTermoAdesao;
        return Result.Ok();
    }

    protected void CarregarSenha(string senha)
    {
        Senha = senha;
    }

    private bool IsCpfValido(string cpf)
    {
        cpf = new string(cpf.Where(char.IsDigit).ToArray());
        if (cpf.Length != 11) return false;
        if (new string(cpf[0], 11) == cpf) return false;

        var soma = 0;
        for (var i = 0; i < 9; i++)
            soma += int.Parse(cpf[i].ToString()) * (10 - i);
        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        soma = 0;
        for (var i = 0; i < 10; i++)
            soma += int.Parse(cpf[i].ToString()) * (11 - i);
        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith($"{digito1}{digito2}");
    }

    public abstract object ToDto();
}