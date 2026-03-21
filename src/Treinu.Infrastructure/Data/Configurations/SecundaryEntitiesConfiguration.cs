using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;

namespace Treinu.Infrastructure.Data.Configurations;

public class ContatoConfiguration : IEntityTypeConfiguration<Contato>
{
    public void Configure(EntityTypeBuilder<Contato> builder)
    {
        builder.ToTable("Contatos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Tipo)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(c => c.Plataforma)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(c => c.Valor)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.Descricao)
            .HasMaxLength(200);

        builder.Property(c => c.NomeExibicao)
            .HasMaxLength(50);
    }
}

public class CertificadoConfiguration : IEntityTypeConfiguration<Certificado>
{
    public void Configure(EntityTypeBuilder<Certificado> builder)
    {
        builder.ToTable("Certificados");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.ArquivoPdf)
            .IsRequired();
    }
}
