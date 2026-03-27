using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;

namespace Treinu.Infrastructure.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.NomeCompleto)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Senha)
            .IsRequired();

        builder.Property(u => u.Cpf)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(u => u.Cpf).IsUnique();

        builder.Property(u => u.Genero)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.Perfil)
            .HasConversion<string>()
            .HasMaxLength(20);

        // TPH (Table-Per-Hierarchy) Setup
        builder.HasDiscriminator(u => u.Perfil)
            .HasValue<Aluno>(PerfilEnum.ALUNO)
            .HasValue<Treinador>(PerfilEnum.TREINADOR)
            .HasValue<Administrador>(PerfilEnum.ADMIN);

        // Relationships
        builder.HasMany(u => u.Contato)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Credencial)
            .WithOne(c => c.Usuario)
            .HasForeignKey<Credencial>(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class AlunoConfiguration : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> builder)
    {
        builder.Property(a => a.Objetivo)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasMany(a => a.AvaliacaoFisica)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Treinador)
            .WithMany(t => t.Alunos)
            .HasForeignKey(a => a.TreinadorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class TreinadorConfiguration : IEntityTypeConfiguration<Treinador>
{
    public void Configure(EntityTypeBuilder<Treinador> builder)
    {
        builder.Property(t => t.Especializacoes)
            .HasColumnType("jsonb"); // Using Postgres JSONB for simple array

        builder.HasMany(t => t.Certificados)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}