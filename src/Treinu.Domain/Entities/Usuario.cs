using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Treinu.Domain.Core;
using Treinu.Domain.Enums;
using Treinu.Domain.Exceptions;
using BCrypt.Net;

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
    bool AceiteTermoAdesao,
    AuthProviderEnum Provider
);

public abstract class Usuario : AggregateRoot
{
    public string NomeCompleto { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Senha { get; private set; } = string.Empty;
    public DateTime DataNascimento { get; private set; }
    public GeneroEnum Genero { get; private set; }
    
    private readonly List<Contato> _contato = new();
    public IReadOnlyCollection<Contato> Contato => _contato.AsReadOnly();
    
    public string Cpf { get; private set; } = string.Empty;
    public PerfilEnum Perfil { get; private set; }
    public bool Ativo { get; private set; }
    public bool AceiteTermoAdesao { get; private set; }
    public AuthProviderEnum Provider { get; private set; }

    protected Usuario() { } // EF constructor

    protected Usuario(Guid id) : base(id)
    {
    }

    protected void SetNomeCompleto(string nomeCompleto)
    {
        if (string.IsNullOrWhiteSpace(nomeCompleto))
            throw new UsuarioException("nome nao pode ser vazio");
        
        NomeCompleto = nomeCompleto;
    }

    protected void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new UsuarioException("email nao pode ser vazio");
            
        if (!new EmailAddressAttribute().IsValid(email))
            throw new UsuarioException("email invalido");
            
        Email = email;
    }

    protected void SetSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new UsuarioException("senha nao pode ser vazia");
            
        if (senha.Length > 20)
            throw new UsuarioException("senha deve ter no maximo 20 caracteres");

        // password must contain at least 8 chars, 1 uppercase, 1 lowercase, 1 number, 1 symbol (@,$,%.!)
        var rx = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&.]{8,}$");
        if (!rx.IsMatch(senha))
            throw new UsuarioException("senha deve conter pelo menos 8 caracteres, com letras maiusculas, numeros e simbolos (@,$,%.!)");

        Senha = BCrypt.Net.BCrypt.HashPassword(senha, 10);
    }

    protected void SetDataNascimento(DateTime dataNascimento)
    {
        if (dataNascimento == default)
            throw new UsuarioException("data nao pode ser vazia");
            
        var hoje = DateTime.Now.Date;
        var minimoData = new DateTime(1930, 1, 1);
        
        if (dataNascimento >= hoje || dataNascimento <= minimoData)
            throw new UsuarioException("data nao pode ser menor que 01-01-1930 ou superior ao dia atual");
            
        DataNascimento = dataNascimento;
    }

    protected void SetGenero(GeneroEnum genero)
    {
        if (!Enum.IsDefined(typeof(GeneroEnum), genero))
            throw new UsuarioException("Genero invalido.Utilizar apenas 'MASCULINO' ou'FEMININO'");
            
        Genero = genero;
    }

    protected void SetContato(List<Contato> contato)
    {
        if (contato == null)
            throw new UsuarioException("contato nao pode ser vazio");
            
        _contato.Clear();
        _contato.AddRange(contato);
    }

    protected void SetCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new UsuarioException("cpf nao deve ser vazio");
            
        if (!IsCpfValido(cpf))
            throw new UsuarioException("cpf nao é valido");
            
        Cpf = cpf;
    }

    protected void SetPerfil(PerfilEnum perfil)
    {
        if (!Enum.IsDefined(typeof(PerfilEnum), perfil))
            throw new UsuarioException("perfil nao pode ser vazio");
            
        Perfil = perfil;
    }

    protected void SetProvider(AuthProviderEnum provider)
    {
        if (!Enum.IsDefined(typeof(AuthProviderEnum), provider))
            throw new UsuarioException("provider nao pode ser vazio");
            
        Provider = provider;
    }

    protected void SetAtivo(bool ativo)
    {
        Ativo = ativo;
    }

    protected void SetAceiteTermoAdesao(bool aceiteTermoAdesao)
    {
        AceiteTermoAdesao = aceiteTermoAdesao;
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
}
