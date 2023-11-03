using Microsoft.IdentityModel.Tokens;

namespace Chat.Application.JWT;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public DateTime Expiration => IssuedAt.Add(Lifetime);
    public DateTime NotBefore => DateTime.UtcNow;
    public DateTime IssuedAt => DateTime.UtcNow;
    public TimeSpan Lifetime { get; set; } = TimeSpan.FromHours(8);
    public Func<string> JtiGenerator => () => Guid.NewGuid().ToString();
    public SigningCredentials SigningCredentials { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
}