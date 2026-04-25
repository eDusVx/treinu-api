using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities.AvaliacaoFisica;

namespace Treinu.Infrastructure.Data.Configurations;

public class AvaliacaoFisicaConfiguration : IEntityTypeConfiguration<Treinu.Domain.Entities.AvaliacaoFisica.AvaliacaoFisica>
{
    public void Configure(EntityTypeBuilder<Treinu.Domain.Entities.AvaliacaoFisica.AvaliacaoFisica> builder)
    {
        builder.ToTable("AvaliacoesFisicas");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.Classificacao)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasMany(a => a.Medidas)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class MedidaConfiguration : IEntityTypeConfiguration<Medida>
{
    public void Configure(EntityTypeBuilder<Medida> builder)
    {
        builder.ToTable("Medidas");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.Chave)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
    }
}