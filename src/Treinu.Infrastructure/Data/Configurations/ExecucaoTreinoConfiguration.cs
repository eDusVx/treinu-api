using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;

namespace Treinu.Infrastructure.Data.Configurations;

public class ExecucaoTreinoConfiguration : IEntityTypeConfiguration<ExecucaoTreino>
{
    public void Configure(EntityTypeBuilder<ExecucaoTreino> builder)
    {
        builder.ToTable("ExecucoesTreino");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.DataInicio).IsRequired();
        builder.Property(x => x.Concluido).IsRequired();

        builder.HasMany(x => x.Exercicios)
            .WithOne()
            .HasForeignKey(x => x.ExecucaoTreinoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ExecucaoExercicioConfiguration : IEntityTypeConfiguration<ExecucaoExercicio>
{
    public void Configure(EntityTypeBuilder<ExecucaoExercicio> builder)
    {
        builder.ToTable("ExecucoesExercicio");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.SeriesRealizadas).IsRequired();
        builder.Property(x => x.RepeticoesRealizadas).IsRequired();
        builder.Property(x => x.CargaUtilizada).HasColumnType("decimal(18,2)").IsRequired();
    }
}
