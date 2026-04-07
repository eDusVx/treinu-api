using Microsoft.EntityFrameworkCore;
using Treinu.Domain.Core.Mediator;
using Treinu.Domain.Entities;
using Treinu.Domain.Entities.AvaliacaoFisica;
using Treinu.Infrastructure.Extensions;

namespace Treinu.Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly IMediator _mediator;

    public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Treinador> Treinadores => Set<Treinador>();
    public DbSet<Credencial> Credenciais => Set<Credencial>();
    public DbSet<Convite> Convites => Set<Convite>();
    public DbSet<TemplateEmail> TemplatesEmail => Set<TemplateEmail>();

    public DbSet<Contato> Contatos => Set<Contato>();

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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving changes (just like the Medium tutorial pattern)
        await _mediator.DispatchDomainEventsAsync(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}