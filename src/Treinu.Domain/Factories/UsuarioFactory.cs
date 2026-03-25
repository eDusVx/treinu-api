using FluentResults;
using Treinu.Domain.Dtos;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Errors;
using Treinu.Domain.Factories.Interfaces;

namespace Treinu.Domain.Factories;

public record FabricarUsuarioProps(
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
    ObjetivoEnum? Objetivo = null,
    List<AvaliacaoFisicaDto>? AvaliacoesFisicas = null,
    List<CertificadoDto>? Certificados = null,
    List<string>? Especializacoes = null
);

public class UsuarioFactory(AvaliacaoFisicaFactory avaliacaoFisicaFactory) : IUsuarioFactory
{
    private readonly AvaliacaoFisicaFactory _avaliacaoFisicaFactory = avaliacaoFisicaFactory;

    public Result<Usuario> Fabricar(FabricarUsuarioProps props)
    {
        switch (props.TipoUsuario)
        {
            case PerfilEnum.TREINADOR:
                var resultT = CriarTreinador(props);
                if (resultT.IsFailed) return Result.Fail<Usuario>(resultT.Errors);
                return Result.Ok<Usuario>(resultT.Value);

            case PerfilEnum.ALUNO:
                var resultA = CriarAluno(props);
                if (resultA.IsFailed) return Result.Fail<Usuario>(resultA.Errors);
                return Result.Ok<Usuario>(resultA.Value);

            default:
                return Result.Fail<Usuario>(DomainErrors.Usuario.DadosVazios);
        }
    }

    private static Result<Treinador> CriarTreinador(FabricarUsuarioProps props)
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
            props.Especializacoes ?? []
        ));
    }

    private Result<Aluno> CriarAluno(FabricarUsuarioProps props)
    {
        var contatos = CriarContatos(props.Contatos);
        var avaliacoesFisicas =
            _avaliacaoFisicaFactory.Fabricar(props.AvaliacoesFisicas ?? []);

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
            props.Objetivo.GetValueOrDefault(),
            avaliacoesFisicas
        ));
    }

    private static List<Contato> CriarContatos(IEnumerable<ContatoDto> contatos)
    {
        return [.. contatos.Select(contato => Contato.Criar(new CriarContatoProps(
            contato.Tipo,
            contato.Valor,
            contato.Descricao,
            contato.Principal,
            contato.Plataforma,
            contato.NomeExibicao
        )))];
    }

    private static List<Certificado> CriarCertificados(IEnumerable<CertificadoDto> certificados)
    {
        return [.. certificados.Select(certificado => Certificado.Criar(new CriarCertificadoProps(
            certificado.Nome,
            certificado.ArquivoPdf,
            certificado.DataUpload,
            certificado.Validado
        )))];
    }
}