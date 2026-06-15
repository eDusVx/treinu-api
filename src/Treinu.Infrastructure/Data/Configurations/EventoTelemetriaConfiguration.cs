using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;

namespace Treinu.Infrastructure.Data.Configurations;

public class EventoTelemetriaConfiguration : IEntityTypeConfiguration<EventoTelemetria>
{
    public void Configure(EntityTypeBuilder<EventoTelemetria> builder)
    {
        builder.ToTable("EventosTelemetria");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Tipo)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(e => e.DataOcorrencia)
            .IsRequired();

        builder.HasOne(e => e.Usuario)
            .WithMany()
            .HasForeignKey(e => e.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
