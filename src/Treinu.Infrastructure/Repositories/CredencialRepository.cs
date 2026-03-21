using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;
using Treinu.Infrastructure.Data;

namespace Treinu.Infrastructure.Repositories;

public class CredencialRepository : ICredencialRepository
{
    private readonly AppDbContext _context;

    public CredencialRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SalvarCredencialAsync(Credencial credencial)
    {
        await _context.Credenciais.AddAsync(credencial);
        await _context.SaveChangesAsync();
    }

    public async Task<Credencial?> BuscarCredencialPorEmailAsync(string email)
    {
        return await _context.Credenciais
            .FirstOrDefaultAsync(c => c.Email == email);
    }
}
