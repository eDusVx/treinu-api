using Treinu.Domain.Dtos;
using Treinu.Domain.Enums;
using Treinu.Domain.Events;
using Treinu.Domain.Exceptions;

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
    AuthProviderEnum Provider,
    ObjetivoEnum Objetivo,
    List<AvaliacaoFisica.AvaliacaoFisica>? AvaliacaoFisica = null
);

public class Aluno : Usuario
{
    public ObjetivoEnum Objetivo { get; private set; }
    
    private readonly List<AvaliacaoFisica.AvaliacaoFisica> _avaliacaoFisica = new();
    public IReadOnlyCollection<AvaliacaoFisica.AvaliacaoFisica> AvaliacaoFisica => _avaliacaoFisica.AsReadOnly();

    protected Aluno() : base() { } // EF constructor

    private Aluno(Guid id) : base(id)
    {
    }

    public static Aluno Criar(CriarAlunoProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Aluno(id);
        
        instance.SetCpf(props.Cpf);
        instance.SetAtivo(props.Ativo);
        instance.SetAceiteTermoAdesao(props.AceiteTermoAdesao);
        instance.SetDataNascimento(props.DataNascimento);
        instance.SetEmail(props.Email);
        instance.SetGenero(props.Genero);
        instance.SetNomeCompleto(props.NomeCompleto);
        instance.SetPerfil(PerfilEnum.ALUNO);
        instance.SetSenha(props.Senha);
        instance.SetContato(props.Contato ?? new List<Contato>());
        instance.SetObjetivo(props.Objetivo);
        instance.SetAvaliacaoFisica(props.AvaliacaoFisica ?? new List<AvaliacaoFisica.AvaliacaoFisica>());
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

    public static Aluno Carregar(CriarAlunoProps props, Guid id)
    {
        var instance = new Aluno(id);
        
        instance.SetCpf(props.Cpf);
        instance.SetAtivo(props.Ativo);
        instance.SetContato(props.Contato ?? new List<Contato>());
        instance.SetAceiteTermoAdesao(props.AceiteTermoAdesao);
        instance.SetDataNascimento(props.DataNascimento);
        instance.SetEmail(props.Email);
        instance.SetGenero(props.Genero);
        instance.SetNomeCompleto(props.NomeCompleto);
        instance.SetPerfil(PerfilEnum.ALUNO);
        instance.CarregarSenha(props.Senha);
        instance.SetObjetivo(props.Objetivo);
        instance.SetAvaliacaoFisica(props.AvaliacaoFisica ?? new List<AvaliacaoFisica.AvaliacaoFisica>());
        instance.SetProvider(props.Provider);
        
        return instance;
    }

    private void SetObjetivo(ObjetivoEnum objetivo)
    {
        if (!Enum.IsDefined(typeof(ObjetivoEnum), objetivo))
        {
            var permitidos = string.Join(", ", Enum.GetNames(typeof(ObjetivoEnum)));
            throw new UsuarioException($"Objetivo inválido ({objetivo}). Valores permitidos: {permitidos}");
        }

        Objetivo = objetivo;
    }

    private void SetAvaliacaoFisica(List<AvaliacaoFisica.AvaliacaoFisica> avaliacaoFisica)
    {
        if (avaliacaoFisica == null)
            throw new UsuarioException("Avaliação física deve ser uma lista");

        var avaliacoesInvalidas = avaliacaoFisica.Where(av => av == null).ToList();
        if (avaliacoesInvalidas.Any())
            throw new UsuarioException("Lista contém avaliações físicas inválidas");

        _avaliacaoFisica.Clear();
        _avaliacaoFisica.AddRange(avaliacaoFisica);
    }

    public AlunoDto ToDto()
    {
        return new AlunoDto(
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
            Objetivo,
            AvaliacaoFisica
        );
    }
}
