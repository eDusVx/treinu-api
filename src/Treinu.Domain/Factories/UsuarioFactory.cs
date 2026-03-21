using FluentResults;
using Treinu.Domain.Core;
using Treinu.Domain.Dtos;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Errors;

namespace Treinu.Domain.Factories;

public record UsuarioBaseProps(
    // ...
    string NomeCompleto,
    string Email,
    string Senha,
    DateTime DataNascimento,
    GeneroEnum Genero,
    string Cpf,
    bool Ativo,
    bool AceiteTermoAdesao,
    PerfilEnum TipoUsuario,
    List<ContatoDto> Contatos
);

public record CriarUsuarioAlunoProps(
    string NomeCompleto,
    string Email,
    string Senha,
    DateTime DataNascimento,
    GeneroEnum Genero,
    string Cpf,
    bool Ativo,
    bool AceiteTermoAdesao,
    PerfilEnum TipoUsuario,
    List<ContatoDto> Contatos,
    ObjetivoEnum Objetivo,
    List<AvaliacaoFisicaDto>? AvaliacoesFisicas = null
) : UsuarioBaseProps(NomeCompleto, Email, Senha, DataNascimento, Genero, Cpf, Ativo, AceiteTermoAdesao, TipoUsuario, Contatos);

public record CriarUsuarioTreinadorProps(
    string NomeCompleto,
    string Email,
    string Senha,
    DateTime DataNascimento,
    GeneroEnum Genero,
    string Cpf,
    bool Ativo,
    bool AceiteTermoAdesao,
    PerfilEnum TipoUsuario,
    List<ContatoDto> Contatos,
    List<CertificadoDto>? Certificados = null,
    List<string>? Especializacoes = null
) : UsuarioBaseProps(NomeCompleto, Email, Senha, DataNascimento, Genero, Cpf, Ativo, AceiteTermoAdesao, TipoUsuario, Contatos);

public class UsuarioFactory
{
    private readonly AvaliacaoFisicaFactory _avaliacaoFisicaFactory;

    public UsuarioFactory(AvaliacaoFisicaFactory avaliacaoFisicaFactory)
    {
        _avaliacaoFisicaFactory = avaliacaoFisicaFactory;
    }

    public Result<Usuario> Fabricar(UsuarioBaseProps props)
    {
        switch (props.TipoUsuario)
        {
            case PerfilEnum.TREINADOR:
                if (props is CriarUsuarioTreinadorProps treinadorProps)
                {
                    var resultT = CriarTreinador(treinadorProps);
                    if (resultT.IsFailed) return Result.Fail<Usuario>(resultT.Errors);
                    return Result.Ok<Usuario>(resultT.Value);
                }
                return Result.Fail<Usuario>(DomainErrors.Usuario.DadosVazios);
                
            case PerfilEnum.ALUNO:
                if (props is CriarUsuarioAlunoProps alunoProps)
                {
                    var resultA = CriarAluno(alunoProps);
                    if (resultA.IsFailed) return Result.Fail<Usuario>(resultA.Errors);
                    return Result.Ok<Usuario>(resultA.Value);
                }
                return Result.Fail<Usuario>(DomainErrors.Usuario.DadosVazios);
                
            default:
                return Result.Fail<Usuario>(DomainErrors.Usuario.DadosVazios);
        }
    }

    private Result<Treinador> CriarTreinador(CriarUsuarioTreinadorProps props)
    {
        var contatos = CriarContatos(props.Contatos);
        var certificados = CriarCertificados(props.Certificados ?? new List<CertificadoDto>());

        return Treinador.Criar(new CriarTreinadorProps(
            props.NomeCompleto,
            props.Email,
            props.Senha,
            props.DataNascimento,
            props.Genero,
            contatos,
            props.Cpf,
            props.Ativo,
            props.AceiteTermoAdesao,
            certificados,
            props.Especializacoes ?? new List<string>()
        ));
    }

    private Result<Aluno> CriarAluno(CriarUsuarioAlunoProps props)
    {
        var contatos = CriarContatos(props.Contatos);
        var avaliacoesFisicas = _avaliacaoFisicaFactory.Fabricar(props.AvaliacoesFisicas ?? new List<AvaliacaoFisicaDto>());

        return Aluno.Criar(new CriarAlunoProps(
            props.NomeCompleto,
            props.Email,
            props.Senha,
            props.DataNascimento,
            props.Genero,
            contatos,
            props.Cpf,
            props.Ativo,
            props.AceiteTermoAdesao,
            props.Objetivo,
            avaliacoesFisicas
        ));
    }

    private List<Contato> CriarContatos(IEnumerable<ContatoDto> contatos)
    {
        return contatos.Select(contato => Contato.Criar(new CriarContatoProps(
            contato.Tipo,
            contato.Valor,
            contato.Descricao,
            contato.Principal,
            contato.Plataforma,
            contato.NomeExibicao
        ))).ToList();
    }

    private List<Certificado> CriarCertificados(IEnumerable<CertificadoDto> certificados)
    {
        return certificados.Select(certificado => Certificado.Criar(new CriarCertificadoProps(
            certificado.Nome,
            certificado.ArquivoPdf,
            certificado.DataUpload,
            certificado.Validado
        ))).ToList();
    }
}
