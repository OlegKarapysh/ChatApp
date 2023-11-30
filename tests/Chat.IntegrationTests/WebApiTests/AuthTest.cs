namespace Chat.IntegrationTests.WebApiTests;

[Collection("Sequential")]
public sealed class AuthTest : IClassFixture<IntegrationTest>
{
    private readonly IntegrationTest _test;
    private readonly TestDbHelper _testDbHelper;

    public AuthTest(IntegrationTest test)
    {
        _test = test;
        _testDbHelper = new TestDbHelper(_test.TestAppFactory);
    }

    [Fact]
    public async Task Register_RegistersNewUser()
    {
        // Arrange.
        var usersCount = _testDbHelper.CountUsers();
        var expectedUsersCount = usersCount + 1;
    
        // Act.
        await _test.RegisterAsync();
        var usersCountAfterRegistration = _testDbHelper.CountUsers();
        var currentUser = await _test.HttpClient.GetFromJsonAsync<UserDto>("api/users");
    
        // Assert.
        using (new AssertionScope())
        {
            usersCountAfterRegistration.Should()!.Be(expectedUsersCount);
            currentUser!.Should()!.NotBeNull();
            currentUser!.Email.Should()!.Be(_test.RegisteredUser.Email);
            currentUser.UserName.Should()!.Be(_test.RegisteredUser.UserName);
        }
    }
    
    [Fact]
    public async Task ChangePassword_ChangesUserPassword()
    {
        // Arrange.
        const string password = "someA21!";
        const string newPassword = password + "new";
        var email = _test.RegisteredUser.Email += "a";
        var username = _test.RegisteredUser.UserName += "a";
        var registerDto = new RegistrationDto
        {
            Email = email, UserName = username, Password = password, RepeatPassword = password
        };
        var changePasswordDto = new ChangePasswordDto
        {
            CurrentPassword = password, NewPassword = newPassword, RepeatNewPassword = newPassword
        };
    
        // Act.
        await _test.RegisterAsync(registerDto);
        var oldUserPasswordSalt = _testDbHelper.GetUserByEmail(_test.RegisteredUser.Email)!.PasswordHash;
        var changePasswordResponse = await _test.HttpClient.PostAsJsonAsync("api/auth/change-password", changePasswordDto);
        var newUserPasswordSalt = _testDbHelper.GetUserByEmail(_test.RegisteredUser.Email)!.PasswordHash;
    
        // Assert.
        using (new AssertionScope())
        {
            changePasswordResponse.EnsureSuccessStatusCode();
            newUserPasswordSalt!.Should()!.NotBe(oldUserPasswordSalt!);
        }
    }
    
    [Fact]
    public async Task LoginAndRefreshTokens_RefreshesTokensAfterLogin()
    {
        // Act.
        var tokens = await _test.LoginAsync();
        var currentUser = await _test.HttpClient.GetFromJsonAsync<UserDto>("api/users");
        var refreshResponse = await _test.HttpClient.PostAsJsonAsync("api/auth/refresh", tokens);
        var newTokens = await refreshResponse.Content.ReadFromJsonAsync<TokenPairDto>();
        _test.SetAuthorizationHeader(newTokens!.AccessToken);
        var currentUserAgain = await _test.HttpClient.GetFromJsonAsync<UserDto>("api/users");
    
        // Assert.
        using (new AssertionScope())
        {
            currentUser!.Should()!.NotBeNull();
            currentUser!.Email.Should()!.Be(_test.LoggedInUser.Email);
            newTokens.Should()!.NotBeEquivalentTo(tokens);
            currentUserAgain!.Should()!.BeEquivalentTo(currentUser);
        }
    }

    [Fact]
    public async Task Login_ReturnsBadRequest_WhenPasswordInvalid()
    {
        // Arrange.
        var invalidPassword = _test.LoggedInUser.Password + "invalid";
        var loginDto = new LoginDto { Email = _test.LoggedInUser.Email, Password = invalidPassword };

        // Act.
        var loginResponse = await _test.HttpClient.PostAsJsonAsync("api/auth/login", loginDto);

        // Assert.
        loginResponse.StatusCode.Should()!.Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenEmailAlreadyRegistered()
    {
        // Arrange.
        var registeredEmail = _test.LoggedInUser.Email;
        var password = _test.RegisteredUser.Password;
        var registerDto = new RegistrationDto
        {
            Email = registeredEmail, UserName = "username", Password = password, RepeatPassword = password
        };

        // Act.
        var registerResponse = await _test.HttpClient.PostAsJsonAsync("api/auth/register", registerDto);

        // Assert.
        registerResponse.StatusCode.Should()!.Be(HttpStatusCode.BadRequest);
    }
}