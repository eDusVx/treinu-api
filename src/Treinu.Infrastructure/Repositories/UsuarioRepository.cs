using FluentResults;
using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Errors;
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

    public async Task<Result> AtualizarUsuarioAsync(Usuario usuario)
    {
        try
        {
            if (_context.Entry(usuario).State == EntityState.Detached)
            {
                _context.Usuarios.Update(usuario);
            }
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Erro inesperado ao atualizar usuário: {ex.Message}");
        }
    }

    public async Task<Usuario?> BuscarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<Result<Treinador>> BuscarTreinadorPorIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var treinador = await _context.Usuarios
                .OfType<Treinador>()
                .Include(t => t.Contato)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (treinador == null)
                return Result.Fail<Treinador>(DomainErrors.Usuario.TreinadorNaoEncontrado);

            return Result.Ok(treinador);
        }
        catch (Exception ex)
        {
            return Result.Fail<Treinador>($"Erro inesperado ao buscar treinador: {ex.Message}");
        }
    }

    public async Task<Result<Aluno>> BuscarAlunoPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var aluno = await _context.Usuarios
                .OfType<Aluno>()
                .Include(a => a.Contato)
                .Include(a => a.AvaliacaoFisica)
                    .ThenInclude(av => av.Medidas)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (aluno == null)
                return Result.Fail<Aluno>(DomainErrors.Usuario.AlunoNaoEncontrado);

            return Result.Ok(aluno);
        }
        catch (Exception ex)
        {
            return Result.Fail<Aluno>($"Erro inesperado ao buscar aluno: {ex.Message}");
        }
    }

    public async Task<Result<(int Total, IEnumerable<Usuario> Usuarios)>> BuscarUsuariosPaginadoAsync(
        PerfilEnum? tipoUsuario,
        int page, int limit, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.Usuarios
                .Include(u => u.Contato)
                .AsQueryable();

            query = query.Include(u => ((Aluno)u).AvaliacaoFisica);

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
            return Result.Fail<(int Total, IEnumerable<Usuario> Usuarios)>(
                $"Erro inesperado ao buscar usuários: {ex.Message}");
        }
    }
}