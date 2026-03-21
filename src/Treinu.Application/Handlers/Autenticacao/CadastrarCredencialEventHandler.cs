using MediatR;
using Treinu.Domain.Entities;
using Treinu.Domain.Events;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Autenticacao;

public class CadastrarCredencialEventHandler : INotificationHandler<UsuarioCadastradoEvent>
{
    private readonly ICredencialRepository _credencialRepository;

    public CadastrarCredencialEventHandler(ICredencialRepository credencialRepository)
    {
        _credencialRepository = credencialRepository;
    }

    public async Task Handle(UsuarioCadastradoEvent notification, CancellationToken cancellationToken)
    {
        var props = new CriarCredencialProps(
            notification.Id,
            notification.Email,
            notification.Perfil,
            notification.Ativo,
            notification.Senha
        );

        var credencial = Credencial.Criar(props);

        await _credencialRepository.SalvarCredencialAsync(credencial);
    }
}

