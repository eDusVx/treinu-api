using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities.Chat;

namespace Treinu.Infrastructure.Data.Configurations.Chat;

public class ParticipanteSalaConfiguration : IEntityTypeConfiguration<ParticipanteSala>
{
    public void Configure(EntityTypeBuilder<ParticipanteSala> builder)
    {
        builder.ToTable("ParticipantesSala");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.MensagensNaoLidas)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.IsAdmin)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.DataEntrada)
            .IsRequired();

        // Relacionamento com Usuario
        builder.HasOne(p => p.Usuario)
            .WithMany()
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
