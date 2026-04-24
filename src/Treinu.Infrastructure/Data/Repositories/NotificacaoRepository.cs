using FluentResults;
using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;

namespace Treinu.Infrastructure.Data.Repositories;

public class NotificacaoRepository : INotificacaoRepository
{
    private readonly AppDbContext _context;

    public NotificacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Notificacao>>> BuscarNotificacoesPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        var list = await _context.Notificacoes
            .Where(n => n.UsuarioId == usuarioId)
            .OrderByDescending(n => n.CriadaEm)
            .ToListAsync(cancellationToken);
            
        return Result.Ok(list);
    }

    public async Task<Result<Notificacao>> BuscarNotificacaoPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var notificacao = await _context.Notificacoes.FindAsync(new object[] { id }, cancellationToken);
        if (notificacao == null) return Result.Fail("Notificação não encontrada.");
        return Result.Ok(notificacao);
    }

    public async Task<Result> AdicionarNotificacaoAsync(Notificacao notificacao)
    {
        await _context.Notificacoes.AddAsync(notificacao);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> AtualizarNotificacaoAsync(Notificacao notificacao)
    {
        _context.Notificacoes.Update(notificacao);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
}
