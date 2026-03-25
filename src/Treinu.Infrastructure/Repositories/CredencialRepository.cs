using FluentResults;
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

    public async Task<Result> SalvarCredencialAsync(Credencial credencial)
    {
        try
        {
            await _context.Credenciais.AddAsync(credencial);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro inesperado ao salvar credencial: {ex.Message}");
        }
    }

    public async Task<Result<Credencial?>> BuscarCredencialPorEmailAsync(string email)
    {
        try
        {
            var credencial = await _context.Credenciais
                .FirstOrDefaultAsync(c => c.Email == email);
            return Result.Ok(credencial);
        }
        catch (Exception ex)
        {
            return Result.Fail<Credencial?>($"Erro inesperado ao buscar credencial por e-mail: {ex.Message}");
        }
    }

    public async Task<Result<Credencial?>> BuscarCredencialPorRefreshTokenAsync(string refreshToken)
    {
        try
        {
            var credencial = await _context.Credenciais
                .FirstOrDefaultAsync(c => c.RefreshToken == refreshToken);
            return Result.Ok(credencial);
        }
        catch (Exception ex)
        {
            return Result.Fail<Credencial?>($"Erro inesperado ao buscar credencial por refresh token: {ex.Message}");
        }
    }

    public async Task<Result> AtualizarCredencialAsync(Credencial credencial)
    {
        try
        {
            _context.Credenciais.Update(credencial);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro inesperado ao atualizar credencial: {ex.Message}");
        }
    }
}