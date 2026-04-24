namespace Treinu.Domain.Dtos;

public record NotificacaoDto(
    Guid Id,
    Guid UsuarioId,
    string Titulo,
    string Mensagem,
    bool Lida,
    DateTime CriadaEm
);
