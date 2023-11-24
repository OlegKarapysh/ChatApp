namespace Chat.UnitTests.WebApiTests.Controllers;

public sealed class AuthControllerTest
{
    private readonly AuthController _sut;
    private readonly Mock<IJwtService> _jwtServiceMock = new();
    private readonly Mock<IAuthService> _authServiceMock = new();

    public AuthControllerTest()
    {
        _sut = new AuthController(_authServiceMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ReturnsOkWithTokenPair()
    {
        // Arrange.
        var registrationDto = new RegistrationDto();
        var expectedTokenPairDto = new TokenPairDto();
        _authServiceMock.Setup(x => x.RegisterAsync(It.IsAny<RegistrationDto>()))
                       .ReturnsAsync(expectedTokenPairDto);

        // Act.
        var result = await _sut.RegisterAsync(registrationDto);
        var objectResult = result.Result as ObjectResult;
        
        // Assert.
        _authServiceMock.Verify(x => x.RegisterAsync(It.IsAny<RegistrationDto>()), Times.Once);
        result.Should()!.BeOfType<ActionResult<TokenPairDto>>();
        result.Result!.Should()!.BeOfType<OkObjectResult>();
        objectResult!.Should()!.NotBeNull();
        objectResult!.StatusCode.Should()!.Be((int)HttpStatusCode.OK);
        objectResult.Value!.Should()!
                    .NotBeNull()!.And!
                    .BeOfType<TokenPairDto>()!.And!
                    .BeEquivalentTo(expectedTokenPairDto);
    }
    
    [Fact]
    public async Task LoginAsync_ReturnsOkWithTokenPair()
    {
        // Arrange.
        var loginDto = new LoginDto();
        var expectedTokenPairDto = new TokenPairDto();
        _authServiceMock.Setup(x => x.LoginAsync(It.IsAny<LoginDto>()))
                        .ReturnsAsync(expectedTokenPairDto);

        // Act.
        var result = await _sut.LoginAsync(loginDto);
        var objectResult = result.Result as ObjectResult;
        
        // Assert.
        _authServiceMock.Verify(x => x.LoginAsync(It.IsAny<LoginDto>()), Times.Once);
        result.Should()!.BeOfType<ActionResult<TokenPairDto>>();
        result.Result!.Should()!.BeOfType<OkObjectResult>();
        objectResult!.Should()!.NotBeNull();
        objectResult!.StatusCode.Should()!.Be((int)HttpStatusCode.OK);
        objectResult.Value!.Should()!
                    .NotBeNull()!.And!
                    .BeOfType<TokenPairDto>()!.And!
                    .BeEquivalentTo(expectedTokenPairDto);
    }
}