using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;

namespace Treinu.Infrastructure.Data.Configurations;

public class CredencialConfiguration : IEntityTypeConfiguration<Credencial>
{
    public void Configure(EntityTypeBuilder<Credencial> builder)
    {
        builder.ToTable("Credenciais");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.UsuarioId)
            .IsRequired();

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.Property(c => c.Senha)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(c => c.RefreshToken)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(c => c.RefreshTokenExpiryTime)
            .IsRequired(false);

        builder.Property(c => c.TipoUsuario)
            .HasConversion(
                v => v.ToString(),
                v => (PerfilEnum)Enum.Parse(typeof(PerfilEnum), v)
            )
            .IsRequired()
            .HasColumnType("varchar(30)");

        builder.Property(c => c.Ativo)
            .IsRequired();
    }
}