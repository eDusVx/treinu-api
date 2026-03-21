using Treinu.Domain.Dtos;
using Treinu.Domain.Enums;
using Treinu.Domain.Events;
using Treinu.Domain.Exceptions;

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
    AuthProviderEnum Provider,
    List<Certificado>? Certificados = null,
    List<string>? Especializacoes = null
);

public class Treinador : Usuario
{
    private readonly List<Certificado> _certificados = new();
    public IReadOnlyCollection<Certificado> Certificados => _certificados.AsReadOnly();

    private readonly List<string> _especializacoes = new();
    public IReadOnlyCollection<string> Especializacoes => _especializacoes.AsReadOnly();

    protected Treinador() : base() { } // EF constructor

    private Treinador(Guid id) : base(id)
    {
    }

    public static Treinador Criar(CriarTreinadorProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Treinador(id);
        
        instance.SetCpf(props.Cpf);
        instance.SetAtivo(props.Ativo);
        instance.SetAceiteTermoAdesao(props.AceiteTermoAdesao);
        instance.SetDataNascimento(props.DataNascimento);
        instance.SetEmail(props.Email);
        instance.SetGenero(props.Genero);
        instance.SetNomeCompleto(props.NomeCompleto);
        instance.SetPerfil(PerfilEnum.TREINADOR);
        instance.SetSenha(props.Senha);
        instance.SetContato(props.Contato ?? new List<Contato>());
        instance.SetCertificados(props.Certificados ?? new List<Certificado>());
        instance.SetEspecializacoes(props.Especializacoes ?? new List<string>());
        instance.SetProvider(props.Provider);
        
        instance.Apply(
            new UsuarioCadastradoEvent(
                instance.Id,
                instance.Email,
                instance.Senha,
                instance.Perfil,
                instance.Ativo,
                instance.Provider
            )
        );
        
        return instance;
    }

    public static Treinador Carregar(CriarTreinadorProps props, Guid id)
    {
        var instance = new Treinador(id);
        
        instance.SetCpf(props.Cpf);
        instance.SetAtivo(props.Ativo);
        instance.SetContato(props.Contato ?? new List<Contato>());
        instance.SetAceiteTermoAdesao(props.AceiteTermoAdesao);
        instance.SetDataNascimento(props.DataNascimento);
        instance.SetEmail(props.Email);
        instance.SetGenero(props.Genero);
        instance.SetNomeCompleto(props.NomeCompleto);
        instance.SetPerfil(PerfilEnum.TREINADOR);
        instance.CarregarSenha(props.Senha);
        instance.SetCertificados(props.Certificados ?? new List<Certificado>());
        instance.SetEspecializacoes(props.Especializacoes ?? new List<string>());
        instance.SetProvider(props.Provider);
        
        return instance;
    }

    private void SetCertificados(List<Certificado> certificados)
    {
        if (certificados == null)
            throw new UsuarioException("Certificados não podem ser nulos");
            
        _certificados.Clear();
        _certificados.AddRange(certificados);
    }

    private void SetEspecializacoes(List<string> especializacoes)
    {
        if (especializacoes == null)
            throw new UsuarioException("Especializações não podem ser nulas");
            
        _especializacoes.Clear();
        _especializacoes.AddRange(especializacoes);
    }

    public void AdicionarCertificado(Certificado certificado)
    {
        if (certificado == null)
            throw new UsuarioException("Certificado não pode ser vazio");
            
        _certificados.Add(certificado);
    }

    public void AdicionarEspecializacao(string especializacao)
    {
        if (string.IsNullOrWhiteSpace(especializacao))
            throw new UsuarioException("Especialização não pode ser vazia");
            
        _especializacoes.Add(especializacao);
    }

    public TreinadorDto ToDto()
    {
        return new TreinadorDto(
            Id,
            NomeCompleto,
            Email,
            DataNascimento,
            Genero,
            Contato,
            Cpf,
            Perfil,
            Ativo,
            AceiteTermoAdesao,
            Certificados,
            Especializacoes
        );
    }
}
