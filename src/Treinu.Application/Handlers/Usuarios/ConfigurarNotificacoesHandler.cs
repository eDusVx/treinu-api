using Treinu.Domain.Core.Mediator;
using FluentResults;
using Treinu.Contracts.Commands.Usuarios;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Usuarios;

public class ConfigurarNotificacoesHandler(IUsuarioRepository usuarioRepository)
    : IRequestHandler<ConfigurarNotificacoesCommand, Result>
{
    public async Task<Result> Handle(ConfigurarNotificacoesCommand request, CancellationToken cancellationToken)
    {
        var config = await usuarioRepository.ObterConfiguracaoNotificacaoAsync(request.UsuarioId, cancellationToken);

        if (config == null)
        {
            config = ConfiguracaoNotificacao.CriarPadrao(request.UsuarioId);
        }

        config.Atualizar(
            request.ReceberEmail,
            request.ReceberPush,
            request.AlertaVencimentoAvaliacao,
            request.AlertaVencimentoTreino,
            request.AlertaNovoTreino
        );

        await usuarioRepository.SalvarConfiguracaoNotificacaoAsync(config, cancellationToken);
        
        return Result.Ok();
    }
}
