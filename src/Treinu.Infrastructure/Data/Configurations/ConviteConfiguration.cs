using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;

namespace Treinu.Infrastructure.Data.Configurations;

public class ConviteConfiguration : IEntityTypeConfiguration<Convite>
{
    public void Configure(EntityTypeBuilder<Convite> builder)
    {
        builder.ToTable("Convites");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.Token)
            .IsRequired();

        builder.Property(c => c.Perfil)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.ExpiraEm)
            .IsRequired();

        builder.Property(c => c.CriadoEm)
            .IsRequired();

        builder.HasIndex(c => c.Token).IsUnique();
        builder.HasIndex(c => c.Email);

        builder.HasOne(c => c.Treinador)
            .WithMany(t => t.Convites)
            .HasForeignKey(c => c.TreinadorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}