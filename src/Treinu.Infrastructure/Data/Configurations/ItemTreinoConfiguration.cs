using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;

namespace Treinu.Infrastructure.Data.Configurations;

public class ItemTreinoConfiguration : IEntityTypeConfiguration<ItemTreino>
{
    public void Configure(EntityTypeBuilder<ItemTreino> builder)
    {
        builder.ToTable("ItensTreino");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedNever();

        builder.Property(i => i.Series)
            .IsRequired();

        builder.Property(i => i.Repeticoes)
            .HasMaxLength(50);

        builder.Property(i => i.Carga)
            .HasMaxLength(50);

        builder.Property(i => i.Pausa)
            .HasMaxLength(50);

        builder.Property(i => i.Observacoes)
            .HasMaxLength(255);

        builder.Property(i => i.Ordem)
            .IsRequired();

        // Foreign keys
        builder.HasOne<Treino>()
            .WithMany(t => t.Itens)
            .HasForeignKey(i => i.TreinoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Exercicio)
            .WithMany()
            .HasForeignKey(i => i.ExercicioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
