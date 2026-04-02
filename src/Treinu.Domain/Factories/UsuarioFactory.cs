using FluentResults;
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
    ObjetivoEnum? Objetivo = null,
    Guid? TreinadorId = null,
    string? Cref = null
);

public class UsuarioFactory : IUsuarioFactory
{
    public Result<Usuario> Fabricar(FabricarUsuarioProps props)
    {
        switch (props.TipoUsuario)
        {
            case PerfilEnum.TREINADOR:
                var resultT = Treinador.Criar(new CriarTreinadorProps(
                    props.NomeCompleto,
                    props.Email,
                    props.Senha,
                    props.DataNascimento,
                    props.Genero,
                    new List<Contato>(),
                    props.Cpf,
                    props.Ativo,
                    props.AceiteTermoAdesao,
                    props.Cref ?? string.Empty
                ));
                if (resultT.IsFailed) return Result.Fail<Usuario>(resultT.Errors);
                return Result.Ok<Usuario>(resultT.Value);

            case PerfilEnum.ALUNO:
                var resultA = Aluno.Criar(new CriarAlunoProps(
                    props.NomeCompleto,
                    props.Email,
                    props.Senha,
                    props.DataNascimento,
                    props.Genero,
                    new List<Contato>(),
                    props.Cpf,
                    props.Ativo,
                    props.AceiteTermoAdesao,
                    props.Objetivo.GetValueOrDefault(),
                    props.TreinadorId
                ));
                if (resultA.IsFailed) return Result.Fail<Usuario>(resultA.Errors);
                return Result.Ok<Usuario>(resultA.Value);

            default:
                return Result.Fail<Usuario>(DomainErrors.Usuario.DadosVazios);
        }
    }
}