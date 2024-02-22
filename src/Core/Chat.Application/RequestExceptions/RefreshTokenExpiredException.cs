namespace Chat.Application.RequestExceptions;

public class RefreshTokenExpiredException : RequestException
{
    public RefreshTokenExpiredException() : base(
        "Refresh token expired!",
        ErrorType.TokenExpired,
        HttpStatusCode.Forbidden)
    {
    }
}