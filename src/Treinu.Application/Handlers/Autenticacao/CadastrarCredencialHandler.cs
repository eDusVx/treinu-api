using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Events;
using Treinu.Domain.Repositories;

namespace Treinu.Application.Handlers.Autenticacao;

public class CadastrarCredencialHandler : INotificationHandler<UsuarioCadastradoEvent>
{
    private readonly ICredencialRepository _credencialRepository;

    public CadastrarCredencialHandler(ICredencialRepository credencialRepository)
    {
        _credencialRepository = credencialRepository;
    }

    public async Task Handle(UsuarioCadastradoEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var props = new CriarCredencialProps(
                notification.Id,
                notification.Email,
                notification.Perfil,
                notification.Ativo,
                notification.Senha
            );

            var credencialResult = Credencial.Criar(props);
            if (credencialResult.IsFailed)
                throw new InvalidOperationException(credencialResult.Errors.First().Message);

            var result = await _credencialRepository.SalvarCredencialAsync(credencialResult.Value);
            if (result.IsFailed)
                throw new InvalidOperationException(result.Errors.First().Message);
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            throw new InvalidOperationException(
                $"Erro inesperado ao gerar credenciais para usuário {notification.Id}: {ex.Message}", ex);
        }
    }
}