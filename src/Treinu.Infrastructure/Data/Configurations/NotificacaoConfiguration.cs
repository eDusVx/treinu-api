using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;

namespace Treinu.Infrastructure.Data.Configurations;

public class NotificacaoConfiguration : IEntityTypeConfiguration<Notificacao>
{
    public void Configure(EntityTypeBuilder<Notificacao> builder)
    {
        builder.ToTable("Notificacoes");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.UsuarioId)
            .IsRequired();

        builder.Property(n => n.Titulo)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(n => n.Mensagem)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(n => n.Lida)
            .IsRequired();

        builder.Property(n => n.CriadaEm)
            .IsRequired();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(n => n.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
