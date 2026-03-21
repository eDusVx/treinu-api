using FluentAssertions;
using Treinu.Domain.Entities;
using Treinu.Domain.Enums;
using Treinu.Domain.Exceptions;

namespace Treinu.UnitTests.Domain;

public class CredencialTests
{
    [Fact]
    public void Criar_Deve_Armazenar_Senha_Local_Exatamente_ComoRecebido()
    {
        var fakeHash = BCrypt.Net.BCrypt.HashPassword("senha123", 4);
        var props = new CriarCredencialProps(Guid.NewGuid(), "teste@teste.com", PerfilEnum.ALUNO, true, fakeHash);

        var credencial = Credencial.Criar(props);

        credencial.Email.Should().Be("teste@teste.com");
        credencial.Senha.Should().Be(fakeHash);
    }

    [Fact]
    public void VerificarSenha_Deve_Passar_Quando_Senha_Correta()
    {
        var fakeHash = BCrypt.Net.BCrypt.HashPassword("senha123", 4);
        var props = new CriarCredencialProps(Guid.NewGuid(), "teste@teste.com", PerfilEnum.ALUNO, true, fakeHash);
        var credencial = Credencial.Criar(props);

        var action = () => credencial.VerificarSenha("senha123");

        action.Should().NotThrow<UnauthorizedAccessException>();
    }

    [Fact]
    public void VerificarSenha_Deve_Lancar_UnauthorizedAccessException_Quando_Senha_Incorreta()
    {
        var fakeHash = BCrypt.Net.BCrypt.HashPassword("senha123", 4);
        var props = new CriarCredencialProps(Guid.NewGuid(), "teste@teste.com", PerfilEnum.ALUNO, true, fakeHash);
        var credencial = Credencial.Criar(props);

        var action = () => credencial.VerificarSenha("senhaErrada");

        action.Should().Throw<UnauthorizedAccessException>();
    }

    [Fact]
    public void RevogarRefreshToken_Deve_Limpar_Token_Quando_Chamado()
    {
        var fakeHash = BCrypt.Net.BCrypt.HashPassword("senha123", 4);
        var props = new CriarCredencialProps(Guid.NewGuid(), "teste@teste.com", PerfilEnum.ALUNO, true, fakeHash);
        var credencial = Credencial.Criar(props);
        credencial.AtualizarRefreshToken("some_token", DateTime.UtcNow.AddDays(7));
        
        credencial.RevogarRefreshToken();

        credencial.RefreshToken.Should().BeNull();
        credencial.RefreshTokenExpiryTime.Should().BeNull();
    }
}
