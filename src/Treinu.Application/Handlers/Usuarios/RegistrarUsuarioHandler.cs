using FluentResults;
using Treinu.Contracts.Commands;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Errors;
using Treinu.Domain.Exceptions;
using Treinu.Domain.Factories;
using Treinu.Domain.Factories.Interfaces;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Usuarios;

public class RegistrarUsuarioHandler(
    IUsuarioRepository usuarioRepository,
    IUsuarioFactory usuarioFactory) : IRequestHandler<RegistrarUsuarioCommand, Result<object>>
{
    private readonly IUsuarioFactory _usuarioFactory = usuarioFactory;
    private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

    public async Task<Result<object>> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _usuarioRepository.VerificarExistenciaAsync(request.Email, request.Cpf);
        }
        catch (RepositoryException)
        {
            return Result.Fail<object>(DomainErrors.Usuario.ConflitoEmEmailOuCpf);
        }

        var props = new FabricarUsuarioProps(
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
            request.Objetivo,
            request.AvaliacoesFisicas,
            request.Certificados,
            request.Especializacoes
        );

        var usuarioResult = _usuarioFactory.Fabricar(props);
        if (usuarioResult.IsFailed) return Result.Fail<object>(usuarioResult.Errors);

        var usuario = usuarioResult.Value;

        await _usuarioRepository.SalvarUsuarioAsync(usuario);

        return Result.Ok(usuario.ToDto());
    }
}