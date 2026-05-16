using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;

namespace Treinu.Infrastructure.Data.Configurations;

public class AvaliacaoPlataformaConfiguration : IEntityTypeConfiguration<AvaliacaoPlataforma>
{
    public void Configure(EntityTypeBuilder<AvaliacaoPlataforma> builder)
    {
        builder.ToTable("AvaliacoesPlataforma");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Nota).IsRequired();
        builder.Property(x => x.Comentario).HasMaxLength(1000);
        builder.Property(x => x.DataCriacao).IsRequired();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
