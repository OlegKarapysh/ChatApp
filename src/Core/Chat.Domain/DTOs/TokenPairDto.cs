namespace Chat.Domain.DTOs;

public sealed class TokenPairDto
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}