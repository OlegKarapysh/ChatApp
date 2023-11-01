namespace Chat.Domain.DTOs.Authentication;

public sealed class TokenPairDto
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}