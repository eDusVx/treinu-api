using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities.Chat;

namespace Treinu.Infrastructure.Data.Configurations.Chat;

public class MensagemChatConfiguration : IEntityTypeConfiguration<MensagemChat>
{
    public void Configure(EntityTypeBuilder<MensagemChat> builder)
    {
        builder.ToTable("MensagensChat");

        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.Conteudo)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(m => m.Tipo)
            .IsRequired();

        builder.Property(m => m.DataEnvio)
            .IsRequired();

        // Relacionamento com Usuario (Remetente)
        builder.HasOne(m => m.Remetente)
            .WithMany()
            .HasForeignKey(m => m.RemetenteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
