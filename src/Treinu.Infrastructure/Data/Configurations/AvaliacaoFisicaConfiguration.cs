using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities.AvaliacaoFisica;
using Treinu.Domain.Enums;

namespace Treinu.Infrastructure.Data.Configurations;

public class AvaliacaoFisicaConfiguration : IEntityTypeConfiguration<AvaliacaoFisica>
{
    public void Configure(EntityTypeBuilder<AvaliacaoFisica> builder)
    {
        builder.ToTable("AvaliacoesFisicas");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Tipo)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasDiscriminator(a => a.Tipo)
            .HasValue<Documento>(TipoAvaliacaoEnum.DOCUMENTO)
            .HasValue<Questionario>(TipoAvaliacaoEnum.QUESTIONARIO);
    }
}

public class DocumentoConfiguration : IEntityTypeConfiguration<Documento>
{
    public void Configure(EntityTypeBuilder<Documento> builder)
    {
        builder.Property(d => d.Nome)
            .HasMaxLength(255);
            
        builder.Property(d => d.Arquivo)
            .HasMaxLength(1000);
    }
}

public class QuestionarioConfiguration : IEntityTypeConfiguration<Questionario>
{
    public void Configure(EntityTypeBuilder<Questionario> builder)
    {
        builder.Property(q => q.Classificacao)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasMany(q => q.Medidas)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class MedidaConfiguration : IEntityTypeConfiguration<Medida>
{
    public void Configure(EntityTypeBuilder<Medida> builder)
    {
        builder.ToTable("Medidas");
        
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Chave)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
            
        // decimal defaults mapped to PG's numeric mapping usually works well.
    }
}
