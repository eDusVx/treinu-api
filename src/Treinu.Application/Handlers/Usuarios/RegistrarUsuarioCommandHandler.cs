using MediatR;
using Treinu.Contracts.Commands;
using Treinu.Contracts.Events;
using Treinu.Domain.Enums;
using Treinu.Domain.Factories;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Usuarios;

public class RegistrarUsuarioCommandHandler : IRequestHandler<RegistrarUsuarioCommand, object>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly UsuarioFactory _usuarioFactory;
    private readonly IMediator _mediator;

    public RegistrarUsuarioCommandHandler(
        IUsuarioRepository usuarioRepository,
        UsuarioFactory usuarioFactory,
        IMediator mediator)
    {
        _usuarioRepository = usuarioRepository;
        _usuarioFactory = usuarioFactory;
        _mediator = mediator;
    }

    public async Task<object> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
    {
        await _usuarioRepository.VerificarExistenciaAsync(request.Email, request.Cpf);

        UsuarioBaseProps props;
        
        if (request.TipoUsuario == PerfilEnum.ALUNO)
        {
            props = new CriarUsuarioAlunoProps(
                request.NomeCompleto,
                request.Email,
                request.Senha,
                request.DataNascimento,
                request.Genero,
                request.Cpf,
                request.Ativo,
                request.AceiteTermoAdesao,
                request.TipoUsuario,
                request.Contatos,
                request.Provider,
                request.Objetivo.GetValueOrDefault(),
                request.AvaliacoesFisicas
            );
        }
        else
        {
            props = new CriarUsuarioTreinadorProps(
                request.NomeCompleto,
                request.Email,
                request.Senha,
                request.DataNascimento,
                request.Genero,
                request.Cpf,
                request.Ativo,
                request.AceiteTermoAdesao,
                request.TipoUsuario,
                request.Contatos,
                request.Provider,
                request.Certificados,
                request.Especializacoes
            );
        }

        var usuario = _usuarioFactory.Fabricar(props);

        await _usuarioRepository.SalvarUsuarioAsync(usuario);

        // Publish domain events dynamically or specifically trigger the integration event
        // The typescript version triggered a Domain Event that the CadastrarCredencial.handler heard.
        var integrationEvent = new UsuarioCadastradoNotification(
            usuario.Id,
            usuario.Email,
            usuario.Senha,
            usuario.Perfil,
            usuario.Ativo,
            usuario.Provider
        );

        await _mediator.Publish(integrationEvent, cancellationToken);

        if (usuario is Treinu.Domain.Entities.Aluno aluno)
            return aluno.ToDto();
            
        if (usuario is Treinu.Domain.Entities.Treinador treinador)
            return treinador.ToDto();

        throw new InvalidOperationException("Unrecognized user type returned from factory");
    }
}
