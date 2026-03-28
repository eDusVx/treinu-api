using FluentResults;
using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Errors;
using Treinu.Domain.Repositories;
using Treinu.Infrastructure.Data;

namespace Treinu.Infrastructure.Repositories;

public class ConviteRepository : IConviteRepository
{
    private readonly AppDbContext _context;

    public ConviteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> SalvarConviteAsync(Convite convite)
    {
        try
        {
            await _context.Convites.AddAsync(convite);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro ao salvar convite: {ex.Message}");
        }
    }

    public async Task<Result<Convite>> BuscarPorTokenAsync(Guid token)
    {
        try
        {
            var convite = await _context.Convites
                .FirstOrDefaultAsync(c => c.Token == token);

            if (convite == null)
                return Result.Fail<Convite>(DomainErrors.Convite.ConviteNaoEncontrado);

            return Result.Ok(convite);
        }
        catch (Exception ex)
        {
            return Result.Fail<Convite>($"Erro ao buscar convite: {ex.Message}");
        }
    }

    public async Task<Result> AtualizarConviteAsync(Convite convite)
    {
        try
        {
            _context.Convites.Update(convite);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro ao atualizar convite: {ex.Message}");
        }
    }

    public async Task<bool> ExisteConvitePendenteParaEmailAsync(string email)
    {
        return await _context.Convites
            .AnyAsync(c => c.Email == email && c.Status == ConviteStatusEnum.PENDENTE && c.ExpiraEm > DateTime.UtcNow);
    }
}