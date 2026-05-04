using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities.Chat;

namespace Treinu.Infrastructure.Data.Configurations.Chat;

public class SalaChatConfiguration : IEntityTypeConfiguration<SalaChat>
{
    public void Configure(EntityTypeBuilder<SalaChat> builder)
    {
        builder.ToTable("SalasChat");

        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Id).ValueGeneratedNever();

        builder.Property(s => s.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.Tipo)
            .IsRequired();

        // Relacionamentos
        builder.HasMany(s => s.Participantes)
            .WithOne(p => p.Sala)
            .HasForeignKey(p => p.SalaChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Mensagens)
            .WithOne(m => m.Sala)
            .HasForeignKey(m => m.SalaChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
