using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Treinu.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            // Opcional: Adicionar o usuário a um grupo pessoal para receber notificações globais
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
        }

        await base.OnConnectedAsync();
    }

    public async Task EntrarNaSala(string salaId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Sala_{salaId}");
    }

    public async Task SairDaSala(string salaId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Sala_{salaId}");
    }

    public async Task NotificarDigitando(string salaId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = Context.User?.Identity?.Name ?? "Usuário"; // ou buscar via claim

        await Clients.Group($"Sala_{salaId}").SendAsync("UsuarioDigitando", new { SalaId = salaId, UsuarioId = userId, Nome = userName });
    }

    public async Task PararDigitando(string salaId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await Clients.Group($"Sala_{salaId}").SendAsync("UsuarioParouDigitando", new { SalaId = salaId, UsuarioId = userId });
    }
}
