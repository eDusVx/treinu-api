using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;

namespace Treinu.Infrastructure.Data.Configurations;

public class TreinoConfiguration : IEntityTypeConfiguration<Treino>
{
    public void Configure(EntityTypeBuilder<Treino> builder)
    {
        builder.ToTable("Treinos");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(t => t.Descricao)
            .HasMaxLength(1000);

        builder.Property(t => t.DataInicio)
            .IsRequired();

        builder.Property(t => t.DataFim)
            .IsRequired();

        builder.Property(t => t.NomeDivisaoA)
            .HasMaxLength(50);
        builder.Property(t => t.NomeDivisaoB)
            .HasMaxLength(50);
        builder.Property(t => t.NomeDivisaoC)
            .HasMaxLength(50);
        builder.Property(t => t.NomeDivisaoD)
            .HasMaxLength(50);

        builder.Property(t => t.DivisaoSegunda)
            .HasMaxLength(1);
        builder.Property(t => t.DivisaoTerca)
            .HasMaxLength(1);
        builder.Property(t => t.DivisaoQuarta)
            .HasMaxLength(1);
        builder.Property(t => t.DivisaoQuinta)
            .HasMaxLength(1);
        builder.Property(t => t.DivisaoSexta)
            .HasMaxLength(1);
        builder.Property(t => t.DivisaoSabado)
            .HasMaxLength(1);
        builder.Property(t => t.DivisaoDomingo)
            .HasMaxLength(1);

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(t => t.TreinadorId)
            .IsRequired();

        builder.Property(t => t.AlunoId)
            .IsRequired();

        builder.HasOne(t => t.Aluno)
            .WithMany()
            .HasForeignKey(t => t.AlunoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Treinador>()
            .WithMany()
            .HasForeignKey(t => t.TreinadorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Metadata.FindNavigation(nameof(Treino.Itens))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
