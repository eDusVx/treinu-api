using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;

namespace Treinu.Infrastructure.Data.Configurations;

public class ConfiguracaoNotificacaoConfiguration : IEntityTypeConfiguration<ConfiguracaoNotificacao>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoNotificacao> builder)
    {
        builder.ToTable("ConfiguracoesNotificacao");
        builder.HasKey(c => c.Id);
        
        builder.HasOne<Usuario>()
            .WithOne()
            .HasForeignKey<ConfiguracaoNotificacao>(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
