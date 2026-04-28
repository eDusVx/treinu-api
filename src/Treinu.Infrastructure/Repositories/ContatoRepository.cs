using FluentResults;
using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities;
using Treinu.Domain.Errors;
using Treinu.Domain.Repositories;
using Treinu.Infrastructure.Data;

namespace Treinu.Infrastructure.Repositories;

public class ContatoRepository : IContatoRepository
{
    private readonly AppDbContext _context;

    public ContatoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> AdicionarContatoAsync(Contato contato, Guid usuarioId, CancellationToken cancellationToken = default)
    {
        try
        {
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId, cancellationToken);
            if (!usuarioExiste)
                return Result.Fail(DomainErrors.Usuario.AlunoNaoEncontrado);

            _context.Entry(contato).Property("UsuarioId").CurrentValue = usuarioId;
            await _context.Contatos.AddAsync(contato, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro inesperado ao salvar contato: {ex.Message}");
        }
    }

    public async Task<Result> RemoverContatoAsync(Guid contatoId, Guid usuarioId, CancellationToken cancellationToken = default)
    {
        try
        {
            var contato = await _context.Contatos
                .FirstOrDefaultAsync(c => c.Id == contatoId && EF.Property<Guid?>(c, "UsuarioId") == usuarioId, cancellationToken);

            if (contato == null)
                return Result.Fail(DomainErrors.Usuario.ContatoNaoEncontrado);

            _context.Contatos.Remove(contato);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro inesperado ao remover contato: {ex.Message}");
        }
    }
}
