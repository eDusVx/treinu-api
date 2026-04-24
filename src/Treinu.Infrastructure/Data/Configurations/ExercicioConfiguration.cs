using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;

namespace Treinu.Infrastructure.Data.Configurations;

public class ExercicioConfiguration : IEntityTypeConfiguration<Exercicio>
{
    public void Configure(EntityTypeBuilder<Exercicio> builder)
    {
        builder.ToTable("Exercicios");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Descricao)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(e => e.Tags)
            .HasMaxLength(255);

        builder.Property(e => e.ArquivoDemonstracao)
            .HasMaxLength(500);

        builder.Property(e => e.TreinadorId)
            .IsRequired();

        builder.Property(e => e.CriadoEm)
            .IsRequired();
            
        // One to Many com Treinador, porém se Treinador não tem lista de Exercicios, não precisa referenciar o navigation
        builder.HasOne<Treinador>()
            .WithMany()
            .HasForeignKey(x => x.TreinadorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
