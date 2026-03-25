using FluentResults;
using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Repositories;
using Treinu.Infrastructure.Data;

namespace Treinu.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> VerificarExistenciaAsync(string email, string cpf)
    {
        try
        {
            var result = new Result();
            var existeEmail = await _context.Usuarios.AnyAsync(u => u.Email == email);
            if (existeEmail) result.WithError($"O E-mail {email} já está em uso.");

            var existeCpf = await _context.Usuarios.AnyAsync(u => u.Cpf == cpf);
            if (existeCpf) result.WithError($"O Cpf {cpf} já está em uso.");

            return result;
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro inesperado ao verificar existência de usuário: {ex.Message}");
        }
    }

    public async Task<Result> SalvarUsuarioAsync(Usuario usuario)
    {
        try
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro inesperado ao salvar usuário: {ex.Message}");
        }
    }

    public async Task<Result<(int Total, IEnumerable<Usuario> Usuarios)>> BuscarUsuariosPaginadoAsync(PerfilEnum? tipoUsuario,
        int page, int limit, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.Usuarios
                .Include(u => u.Contato)
                .AsQueryable();

            query = query.Include(u => ((Aluno)u).AvaliacaoFisica)
                .Include(u => ((Treinador)u).Certificados);

            if (tipoUsuario.HasValue) query = query.Where(u => u.Perfil == tipoUsuario.Value);

            var total = await query.CountAsync(cancellationToken);

            var skip = (page - 1) * limit;

            var usuariosList = await query
                .OrderBy(u => u.NomeCompleto)
                .Skip(skip)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return Result.Ok((total, (IEnumerable<Usuario>)usuariosList));
        }
        catch (Exception ex)
        {
            return Result.Fail<(int Total, IEnumerable<Usuario> Usuarios)>($"Erro inesperado ao buscar usuários: {ex.Message}");
        }
    }
}