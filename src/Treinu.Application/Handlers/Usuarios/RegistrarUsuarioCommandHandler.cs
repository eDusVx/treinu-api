using MediatR;
using Treinu.Contracts.Commands;
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
                request.Certificados,
                request.Especializacoes
            );
        }

        var usuario = _usuarioFactory.Fabricar(props);

        await _usuarioRepository.SalvarUsuarioAsync(usuario);

        // Events will be dispatched automatically by SaveChangesAsync in DbContext

        if (usuario is Treinu.Domain.Entities.Aluno aluno)
            return aluno.ToDto();
            
        if (usuario is Treinu.Domain.Entities.Treinador treinador)
            return treinador.ToDto();

        throw new InvalidOperationException("Unrecognized user type returned from factory");
    }
}
