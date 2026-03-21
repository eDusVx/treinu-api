using FluentResults;
using MediatR;
using Treinu.Contracts.Commands;
using Treinu.Domain.Enums;
using Treinu.Domain.Factories;
using Treinu.Domain.Repositories;
using Treinu.Domain.Errors;

namespace Treinu.Application.Handlers.Usuarios;

public class RegistrarUsuarioHandler : IRequestHandler<RegistrarUsuarioCommand, Result<object>>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly UsuarioFactory _usuarioFactory;
    private readonly IMediator _mediator;

    public RegistrarUsuarioHandler(
        IUsuarioRepository usuarioRepository,
        UsuarioFactory usuarioFactory,
        IMediator mediator)
    {
        _usuarioRepository = usuarioRepository;
        _usuarioFactory = usuarioFactory;
        _mediator = mediator;
    }

    public async Task<Result<object>> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _usuarioRepository.VerificarExistenciaAsync(request.Email, request.Cpf);
        }
        catch (Treinu.Domain.Exceptions.RepositoryException)
        {
            return Result.Fail<object>(DomainErrors.Usuario.ConflitoEmEmailOuCpf);
        }

        UsuarioBaseProps props;
        
        if (request.TipoUsuario == PerfilEnum.ALUNO)
        {
            props = new CriarUsuarioAlunoProps(
                request.NomeCompleto, request.Email, request.Senha, request.DataNascimento,
                request.Genero, request.Cpf, request.Ativo, request.AceiteTermoAdesao,
                request.TipoUsuario, request.Contatos, request.Objetivo.GetValueOrDefault(),
                request.AvaliacoesFisicas
            );
        }
        else
        {
            props = new CriarUsuarioTreinadorProps(
                request.NomeCompleto, request.Email, request.Senha, request.DataNascimento,
                request.Genero, request.Cpf, request.Ativo, request.AceiteTermoAdesao,
                request.TipoUsuario, request.Contatos, request.Certificados,
                request.Especializacoes
            );
        }

        var usuarioResult = _usuarioFactory.Fabricar(props);
        if(usuarioResult.IsFailed) return Result.Fail<object>(usuarioResult.Errors);
        
        var usuario = usuarioResult.Value;

        await _usuarioRepository.SalvarUsuarioAsync(usuario);

        if (usuario is Treinu.Domain.Entities.Aluno aluno)
            return Result.Ok<object>(aluno.ToDto());
            
        if (usuario is Treinu.Domain.Entities.Treinador treinador)
            return Result.Ok<object>(treinador.ToDto());

        return Result.Fail<object>(DomainErrors.Geral.Desconhecido);
    }
}
