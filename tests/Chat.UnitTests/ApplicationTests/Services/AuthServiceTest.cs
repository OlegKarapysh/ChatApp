namespace Chat.UnitTests.ApplicationTests.Services;

public sealed class AuthServiceTest
{
    private const string Email = "email@a.a";
    private const string UserName = "username";
    private const string Password = "someA1!";
    private const string Token = "token";
    private readonly IAuthService _sut;
    private readonly Mock<IJwtService> _jwtServiceMock = new();
    private readonly Mock<UserManager<User>> _userManagerMock;
    
    public AuthServiceTest()
    {
        _userManagerMock = MockHelper.MockUserManager();
        _sut = new AuthService(_jwtServiceMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task ChangePasswordAsync_ChangesPassword_WhenValidPassword()
    {
        // Arrange.
        var changePasswordDto = new ChangePasswordDto
        {
            CurrentPassword = Password,
            NewPassword = "someA2!"
        };
        var id = 1;
        var user = new User();
        var identityResult = IdentityResult.Success;
        _userManagerMock.Setup(x => x.FindByIdAsync(id.ToString()))
                        .ReturnsAsync(user);
        _userManagerMock.Setup(x => x.ChangePasswordAsync(
                            user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword))
                        .ReturnsAsync(identityResult);
        // Act.
        await _sut.ChangePasswordAsync(changePasswordDto, id);

        // Assert.
        using (new AssertionScope())
        {
            _userManagerMock.Verify(x => x.FindByIdAsync(id.ToString()), Times.Once);
            _userManagerMock.Verify(x => x.ChangePasswordAsync(
                user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword), Times.Once);
        }
    }

    [Fact]
    public void ChangePasswordAsync_ThrowsEntityNotFoundException_WhenIdInvalid()
    {
        // Arrange.
        const int invalidId = -1;
        _userManagerMock.Setup(x => x.FindByIdAsync(invalidId.ToString()))
                        .ReturnsAsync((User)null!);
        // Act.
        var tryChangePassword = async () => await _sut.ChangePasswordAsync(new ChangePasswordDto(), invalidId);
        
        // Assert.
        tryChangePassword.Should()!.ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public void ChangePasswordAsync_ThrowsChangePasswordFailedException_WhenInvalidPassword()
    {
        // Arrange.
        const int id = 1;
        var dto = new ChangePasswordDto();
        var user = new User();
        _userManagerMock.Setup(x => x.FindByIdAsync(id.ToString())).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword))
                        .ReturnsAsync(IdentityResult.Failed());
        // Act.
        var tryChangePassword = async () => await _sut.ChangePasswordAsync(dto, id);

        // Assert.
        tryChangePassword.Should()!.ThrowAsync<ChangePasswordFailedException>();
    }

    [Fact]
    public void LoginAsync_ThrowsInvalidEmailException_WhenEmailInvalid()
    {
        // Arrange.
        const string invalidEmail = "email";
        var loginDto = new LoginDto { Email = invalidEmail };
        _userManagerMock.Setup(x => x.FindByEmailAsync(invalidEmail)).ReturnsAsync((User)null!);
        
        // Act.
        var tryLogin = async () => await _sut.LoginAsync(loginDto);

        // Assert.
        tryLogin.Should()!.ThrowAsync<InvalidEmailException>();
    }

    [Fact]
    public void LoginAsync_ThrowsInvalidPasswordException_WhenPasswordInvalid()
    {
        // Arrange.
        var loginDto = new LoginDto
        {
            Email = Email,
            Password = Password
        };
        _userManagerMock.Setup(x => x.FindByEmailAsync(loginDto.Email)).ReturnsAsync(new User());
        _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), loginDto.Password)).ReturnsAsync(false);
        
        // Act.
        var tryLogin = async () => await _sut.LoginAsync(loginDto);
        
        // Assert.
        tryLogin.Should()!.ThrowAsync<InvalidPasswordException>();
    }

    [Fact]
    public async Task LoginAsync_ReturnsTokenPairDto_WhenValidLoginDto()
    {
        // Arrange.
        const int id = 1;
        var user = new User { UserName = UserName, Email = Email, Id = id };
        var loginDto = new LoginDto { Email = Email, Password = Password };
        var refreshToken = new RefreshToken(Token);
        var expectedTokenPair = new TokenPairDto { AccessToken = Token, RefreshToken = refreshToken.Token };
        _userManagerMock.Setup(x => x.FindByEmailAsync(Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, Password)).ReturnsAsync(true);
        _jwtServiceMock.Setup(x => x.CreateRefreshToken()).Returns(refreshToken);
        _jwtServiceMock.Setup(x => x.CreateAccessToken(id, UserName, Email)).Returns(Token);
        
        // Act.
        var result = await _sut.LoginAsync(loginDto);
        
        // Assert.
        using (new AssertionScope())
        {
            result.Should()!.BeEquivalentTo(expectedTokenPair);
            _userManagerMock.Verify(x => x.UpdateAsync(user), Times.Once);
        }
    }

    [Fact]
    public void RegisterAsync_ThrowsEmailAlreadyRegisteredException_WhenEmailRegistered()
    {
        // Arrange.
        var registerDto = new RegistrationDto { Email = Email };
        _userManagerMock.Setup(x => x.FindByEmailAsync(Email)).ReturnsAsync(new User());
        
        // Act.
        var tryRegister = async () => await _sut.RegisterAsync(registerDto);

        // Assert.
        tryRegister.Should()!.ThrowAsync<EmailAlreadyRegisteredException>();
    }
    
    [Fact]
    public void RegisterAsync_ThrowsUsernameAlreadyRegisteredException_WhenUsernameRegistered()
    {
        // Arrange.
        var registerDto = new RegistrationDto { UserName = UserName, Email = Email };
        _userManagerMock.Setup(x => x.FindByEmailAsync(Email)).ReturnsAsync((User)null!);
        _userManagerMock.Setup(x => x.FindByNameAsync(UserName)).ReturnsAsync(new User());
        
        // Act.
        var tryRegister = async () => await _sut.RegisterAsync(registerDto);

        // Assert.
        tryRegister.Should()!.ThrowAsync<UsernameAlreadyRegistered>();
    }

    [Fact]
    public void RegisterAsync_ThrowsBadRegistrationException_WhenRegistrationFailed()
    {
        // Arrange.
        var registerDto = new RegistrationDto
        {
            Email = Email, Password = Password, RepeatPassword = Password, UserName = UserName
        };
        var refreshToken = new RefreshToken(Token);
        var user = new User
        {
            UserName = UserName, Email = Email, RefreshToken = Token, TokenExpiresAt = refreshToken.ExpirationTime
        };
        _userManagerMock.Setup(x => x.FindByEmailAsync(Email)).ReturnsAsync((User)null!);
        _userManagerMock.Setup(x => x.FindByNameAsync(UserName)).ReturnsAsync((User)null!);
        _userManagerMock.Setup(x => x.CreateAsync(user, Password)).ReturnsAsync(IdentityResult.Failed());
        _jwtServiceMock.Setup(x => x.CreateRefreshToken()).Returns(refreshToken);
        
        // Act.
        var tryRegister = async () => await _sut.RegisterAsync(registerDto);
        
        // Assert.
        tryRegister.Should()!.ThrowAsync<BadRegistrationException>();
    }
}