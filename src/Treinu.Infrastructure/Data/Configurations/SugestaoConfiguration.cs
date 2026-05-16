using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;

namespace Treinu.Infrastructure.Data.Configurations;

public class SugestaoConfiguration : IEntityTypeConfiguration<Sugestao>
{
    public void Configure(EntityTypeBuilder<Sugestao> builder)
    {
        builder.ToTable("Sugestoes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Titulo).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Descricao).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.DataCriacao).IsRequired();
        builder.Property(x => x.Lido).IsRequired();
        
        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
