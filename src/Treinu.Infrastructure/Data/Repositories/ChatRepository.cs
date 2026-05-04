using FluentResults;
using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities.Chat;
using Treinu.Domain.Repositories;

namespace Treinu.Infrastructure.Data.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _context;

    public ChatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SalaChat>> BuscarSalaPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sala = await _context.SalasChat
            .Include(s => s.Participantes)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (sala == null) return Result.Fail("Sala de chat não encontrada.");
        return Result.Ok(sala);
    }

    public async Task<Result<List<SalaChat>>> BuscarSalasDoUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        var salas = await _context.SalasChat
            .Include(s => s.Participantes)
            .Where(s => s.Participantes.Any(p => p.UsuarioId == usuarioId))
            .ToListAsync(cancellationToken);

        return Result.Ok(salas);
    }

    public async Task<Result<List<MensagemChat>>> BuscarMensagensDaSalaAsync(Guid salaId, int page, int limit, CancellationToken cancellationToken = default)
    {
        var mensagens = await _context.MensagensChat
            .Include(m => m.Remetente)
            .Where(m => m.SalaChatId == salaId)
            .OrderByDescending(m => m.DataEnvio)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return Result.Ok(mensagens);
    }

    public async Task<Result<SalaChat>> BuscarSalaDiretaAsync(Guid usuario1Id, Guid usuario2Id, CancellationToken cancellationToken = default)
    {
        var sala = await _context.SalasChat
            .Include(s => s.Participantes)
            .Where(s => s.Tipo == TipoSalaChat.Direta)
            .FirstOrDefaultAsync(s => 
                s.Participantes.Any(p => p.UsuarioId == usuario1Id) && 
                s.Participantes.Any(p => p.UsuarioId == usuario2Id), cancellationToken);

        if (sala == null) return Result.Fail("Sala de chat direta não encontrada.");
        return Result.Ok(sala);
    }

    public async Task<Result> AdicionarSalaAsync(SalaChat sala)
    {
        await _context.SalasChat.AddAsync(sala);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> AtualizarSalaAsync(SalaChat sala)
    {
        _context.SalasChat.Update(sala);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> AdicionarMensagemAsync(MensagemChat mensagem)
    {
        await _context.MensagensChat.AddAsync(mensagem);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
}
