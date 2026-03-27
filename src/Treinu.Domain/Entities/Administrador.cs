using FluentResults;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Entities;

public class Administrador : Usuario
{
    protected Administrador() { }

    private Administrador(Guid id) : base(id) { }

    public static Result<Administrador> Criar(CriarUsuarioProps props)
    {
        var id = Guid.NewGuid();
        var instance = new Administrador(id);

        var rs1 = instance.SetCpf(props.Cpf);
        if (rs1.IsFailed) return Result.Fail<Administrador>(rs1.Errors);
        var rs2 = instance.SetAtivo(props.Ativo);
        if (rs2.IsFailed) return Result.Fail<Administrador>(rs2.Errors);
        var rs3 = instance.SetAceiteTermoAdesao(props.AceiteTermoAdesao);
        if (rs3.IsFailed) return Result.Fail<Administrador>(rs3.Errors);
        var rs4 = instance.SetDataNascimento(props.DataNascimento);
        if (rs4.IsFailed) return Result.Fail<Administrador>(rs4.Errors);
        var rs5 = instance.SetEmail(props.Email);
        if (rs5.IsFailed) return Result.Fail<Administrador>(rs5.Errors);
        var rs6 = instance.SetGenero(props.Genero);
        if (rs6.IsFailed) return Result.Fail<Administrador>(rs6.Errors);
        var rs7 = instance.SetNomeCompleto(props.NomeCompleto);
        if (rs7.IsFailed) return Result.Fail<Administrador>(rs7.Errors);
        var rs8 = instance.SetPerfil(PerfilEnum.ADMIN);
        if (rs8.IsFailed) return Result.Fail<Administrador>(rs8.Errors);
        var rs9 = instance.SetSenha(props.Senha);
        if (rs9.IsFailed) return Result.Fail<Administrador>(rs9.Errors);
        var rs10 = instance.SetContato(props.Contato ?? new List<Contato>());
        if (rs10.IsFailed) return Result.Fail<Administrador>(rs10.Errors);

        return Result.Ok(instance);
    }

    public override object ToDto()
    {
        return new { Id, NomeCompleto, Email, Perfil, Ativo };
    }
}
