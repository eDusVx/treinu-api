using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Entities;
using Treinu.Domain.Entities.AvaliacaoFisica;

namespace Treinu.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Treinador> Treinadores => Set<Treinador>();
    public DbSet<Credencial> Credenciais => Set<Credencial>();
    
    public DbSet<Contato> Contatos => Set<Contato>();
    public DbSet<Certificado> Certificados => Set<Certificado>();
    
    public DbSet<AvaliacaoFisica> AvaliacoesFisicas => Set<AvaliacaoFisica>();
    public DbSet<Documento> Documentos => Set<Documento>();
    public DbSet<Questionario> Questionarios => Set<Questionario>();
    public DbSet<Medida> Medidas => Set<Medida>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply all IEntityTypeConfiguration classes found in this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
