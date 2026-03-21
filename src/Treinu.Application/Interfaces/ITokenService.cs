namespace Treinu.Application.Interfaces;

public interface ITokenService
{
    string GerarJwt(string email, string role, string sub);
    string GerarRefreshToken();
}