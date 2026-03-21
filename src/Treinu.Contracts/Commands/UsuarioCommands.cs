using MediatR;
using Treinu.Domain.Dtos;
using Treinu.Domain.Enums;

namespace Treinu.Contracts.Commands;

public record RegistrarUsuarioCommand(
    string NomeCompleto,
    string Email,
    string Senha,
    DateTime DataNascimento,
    GeneroEnum Genero,
    string Cpf,
    bool Ativo,
    bool AceiteTermoAdesao,
    PerfilEnum TipoUsuario,
    List<ContatoDto> Contatos,
    AuthProviderEnum Provider,
    ObjetivoEnum? Objetivo = null,
    List<AvaliacaoFisicaDto>? AvaliacoesFisicas = null,
    List<CertificadoDto>? Certificados = null,
    List<string>? Especializacoes = null
) : IRequest<object>; // The original returns UsuarioDto (TreinadorDto | AlunoDto). In C# we might need to return a base class or object. I will return object and cast it, or create a BaseUsuarioDto.

// Let's refine the return type: I'll use `object` or a shared interface if possible. I'll use `object` to avoid complex inheritance now.
