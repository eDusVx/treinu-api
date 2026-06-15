using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;

namespace Treinu.Infrastructure.Data.Configurations;

public class MetaConfiguration : IEntityTypeConfiguration<Meta>
{
    public void Configure(EntityTypeBuilder<Meta> builder)
    {
        builder.ToTable("Metas");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.Tipo)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(m => m.ValorAlvo)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(m => m.DataLimite)
            .IsRequired();

        builder.Property(m => m.DataCriacao)
            .IsRequired();

        builder.Property(m => m.Ativa)
            .IsRequired();

        builder.HasOne(m => m.Aluno)
            .WithMany()
            .HasForeignKey(m => m.AlunoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
