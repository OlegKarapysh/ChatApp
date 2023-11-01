namespace Chat.Application.Services.JWT;

public interface IJwtService
{
    string CreateAccessToken(int id, string userName, string email);
    string CreateRefreshToken();
}