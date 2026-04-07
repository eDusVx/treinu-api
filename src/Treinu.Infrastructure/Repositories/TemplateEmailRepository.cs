using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities;
using Treinu.Domain.Repositories;
using Treinu.Infrastructure.Data;

namespace Treinu.Infrastructure.Repositories;

public class TemplateEmailRepository : ITemplateEmailRepository
{
    private readonly AppDbContext _context;

    public TemplateEmailRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TemplateEmail?> ObterPorNomeAsync(string nome)
    {
        return await _context.TemplatesEmail
            .FirstOrDefaultAsync(t => t.Nome == nome);
    }

    public async Task AdicionarAsync(TemplateEmail template)
    {
        await _context.TemplatesEmail.AddAsync(template);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(TemplateEmail template)
    {
        _context.TemplatesEmail.Update(template);
        await _context.SaveChangesAsync();
    }
}
