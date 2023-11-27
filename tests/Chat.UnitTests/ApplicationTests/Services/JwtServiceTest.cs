namespace Chat.UnitTests.ApplicationTests.Services;

public sealed class JwtServiceTest
{
    private const string SecretKey = "335052193436408A7506EF0186A17100";
    private readonly IJwtService _sut;
    private readonly JwtOptions _jwtOptions = new()
    {
        Issuer = "Chat.WebApi",
        Audience = "\"https://localhost:7090\"",
        SecretKey = SecretKey,
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(SecretKey)), SecurityAlgorithms.HmacSha256)
    };

    public JwtServiceTest()
    {
        _sut = new JwtService(Options.Create(_jwtOptions));
    }

    [Fact]
    public void CreateRefreshToken_ReturnsRefreshTokenInBase64()
    {
        // Arrange.
        const int expectedTokenLength = 44;
        var base64StringRegex = new Regex("^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$");
        
        // Act.
        var refreshToken = _sut.CreateRefreshToken();
        
        // Assert.
        refreshToken.Token.Should()!
                    .NotBeNullOrWhiteSpace()!.And!
                    .HaveLength(expectedTokenLength)!.And!
                    .MatchRegex(base64StringRegex);
    }
    
    [Fact]
    public void GetIdClaim_ThrowsException_WhenInvalidClaimsPrincipal()
    {
        // Arrange.
        var claimsPrincipal = new ClaimsPrincipal();
        
        // Act.
        var tryGetIdClaim = () => _sut.GetIdClaim(claimsPrincipal);

        // Assert.
        tryGetIdClaim.Should()!.Throw<MissingClaimException>();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(int.MaxValue)]
    public void GetIdClaim_ReturnsId_WhenValidClaimsPrincipal(int id)
    {
        // Arrange.
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(IJwtService.IdClaimName, id.ToString())
        }));

        // Act.
        var resultId = _sut.GetIdClaim(claimsPrincipal);

        // Assert.
        resultId.Should()!.Be(id);
    }

    [Theory]
    [InlineData(1, "a", "b")]
    [InlineData(int.MaxValue, "username", "email")]
    [InlineData(0, "", "")]
    public void CreateAccessToken_ReturnsAccessTokenWithClaims_WhenValidClaims(int id, string userName, string email)
    {
        // Arrange.
        var jwtRegex = new Regex(@"^[A-Za-z0-9-_=]+\.[A-Za-z0-9-_=]+\.[A-Za-z0-9-_.+/=]*$");
        
        // Act.
        var result = _sut.CreateAccessToken(id, userName, email);
        var resultId = _sut.GetIdClaim(result);
        
        // Assert.
        result.Should()!.NotBeNullOrWhiteSpace()!.And!.MatchRegex(jwtRegex);
        resultId.Should()!.Be(id);
    }
}