namespace Chat.Application.Services.JWT;

public interface IJwtService
{
    public const string IdClaimName = "id";
    public const string UserNameClaimName = "username";
    string CreateAccessToken(int id, string userName, string email);
    string CreateRefreshToken();
}