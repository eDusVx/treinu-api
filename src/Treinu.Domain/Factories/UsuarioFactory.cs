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

    private Result<Treinador> CriarTreinador(FabricarUsuarioProps props)
    {
        var result = new Result();
        
        var contatosResult = CriarContatos(props.Contatos);
        if (contatosResult.IsFailed) result.WithReasons(contatosResult.Reasons);

        var certificadosResult = CriarCertificados(props.Certificados ?? new List<CertificadoDto>());
        if (certificadosResult.IsFailed) result.WithReasons(certificadosResult.Reasons);

        if (result.IsFailed) return Result.Fail<Treinador>(result.Errors);

        return Treinador.Criar(new CriarTreinadorProps(
            props.NomeCompleto,
            props.Email,
            props.Senha,
            props.DataNascimento,
            props.Genero,
            contatosResult.Value,
            props.Cpf,
            props.Ativo,
            props.AceiteTermoAdesao,
            certificadosResult.Value,
            props.Especializacoes ?? []
        ));
    }

    private Result<Aluno> CriarAluno(FabricarUsuarioProps props)
    {
        var result = new Result();
        
        var contatosResult = CriarContatos(props.Contatos);
        if (contatosResult.IsFailed) result.WithReasons(contatosResult.Reasons);

        var avaliacoesResult = _avaliacaoFisicaFactory.Fabricar(props.AvaliacoesFisicas ?? []);
        if (avaliacoesResult.IsFailed) result.WithReasons(avaliacoesResult.Reasons);

        if (result.IsFailed) return Result.Fail<Aluno>(result.Errors);

        return Aluno.Criar(new CriarAlunoProps(
            props.NomeCompleto,
            props.Email,
            props.Senha,
            props.DataNascimento,
            props.Genero,
            contatosResult.Value,
            props.Cpf,
            props.Ativo,
            props.AceiteTermoAdesao,
            props.Objetivo.GetValueOrDefault(),
            avaliacoesResult.Value
        ));
    }

    private static Result<List<Contato>> CriarContatos(IEnumerable<ContatoDto> contatosDto)
    {
        var result = new Result<List<Contato>>();
        var contatos = new List<Contato>();

        foreach (var dto in contatosDto)
        {
            var r = Contato.Criar(new CriarContatoProps(
                dto.Tipo,
                dto.Valor,
                dto.Descricao,
                dto.Principal,
                dto.Plataforma,
                dto.NomeExibicao
            ));
            
            if (r.IsFailed) result.WithReasons(r.Reasons);
            else contatos.Add(r.Value);
        }

        if (result.IsFailed) return result;
        return Result.Ok(contatos);
    }

    private static Result<List<Certificado>> CriarCertificados(IEnumerable<CertificadoDto> certificadosDto)
    {
        var result = new Result<List<Certificado>>();
        var certificados = new List<Certificado>();

        foreach (var dto in certificadosDto)
        {
            var r = Certificado.Criar(new CriarCertificadoProps(
                dto.Nome,
                dto.ArquivoPdf,
                dto.DataUpload,
                dto.Validado
            ));

            if (r.IsFailed) result.WithReasons(r.Reasons);
            else certificados.Add(r.Value);
        }

        if (result.IsFailed) return result;
        return Result.Ok(certificados);
    }
}