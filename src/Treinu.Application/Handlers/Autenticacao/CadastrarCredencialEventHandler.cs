using MediatR;
using Treinu.Contracts.Events;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Autenticacao;

public class CadastrarCredencialEventHandler : INotificationHandler<UsuarioCadastradoNotification>
{
    private readonly ICredencialRepository _credencialRepository;

    public CadastrarCredencialEventHandler(ICredencialRepository credencialRepository)
    {
        _credencialRepository = credencialRepository;
    }

    public async Task Handle(UsuarioCadastradoNotification notification, CancellationToken cancellationToken)
    {
        var props = new CriarCredencialProps(
            notification.Id,
            notification.Email,
            notification.Perfil,
            notification.Ativo,
            notification.Provider,
            notification.Provider == AuthProviderEnum.LOCAL ? notification.Senha : null
        );

        var credencial = Credencial.Criar(props);

        await _credencialRepository.SalvarCredencialAsync(credencial);
    }
}

