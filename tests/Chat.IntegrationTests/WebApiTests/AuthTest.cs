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
    public async Task RegistrationFlow_RegistersUserAndAllowsToChangePasswordAndTokens()
    {
        // Arrange.
        var usersCount = _testDbHelper.CountUsers();
        var expectedUsersCount = usersCount + 1;
        var newPassword = _test.RegisteredUser.Password + "new";
        var changePasswordDto = new ChangePasswordDto
        {
            CurrentPassword = _test.RegisteredUser.Password,
            NewPassword = newPassword, RepeatNewPassword = newPassword
        };

        // Act.
        var tokens = await _test.RegisterAsync();
        var usersCountAfterRegistration = _testDbHelper.CountUsers();
        var currentUser = await _test.HttpClient.GetFromJsonAsync<UserDto>("api/users");
        var oldUserPasswordSalt = _testDbHelper.GetUserByEmail(_test.RegisteredUser.Email)!.PasswordHash;
        var changePasswordResponse = await _test.HttpClient.PostAsJsonAsync("api/auth/change-password", changePasswordDto);
        var newUserPasswordSalt = _testDbHelper.GetUserByEmail(_test.RegisteredUser.Email)!.PasswordHash;
        var refreshResponse = await _test.HttpClient.PostAsJsonAsync("api/auth/refresh", tokens);
        var newTokens = await refreshResponse.Content.ReadFromJsonAsync<TokenPairDto>();
        _test.SetAuthorizationHeader(newTokens!.AccessToken);
        var currentUserAgain = await _test.HttpClient.GetFromJsonAsync<UserDto>("api/users");

        // Assert.
        using (new AssertionScope())
        {
            usersCountAfterRegistration.Should()!.Be(expectedUsersCount);
            changePasswordResponse.EnsureSuccessStatusCode();
            currentUser!.Should()!.NotBeNull();
            currentUser!.Email.Should()!.Be(_test.RegisteredUser.Email);
            currentUser.UserName.Should()!.Be(_test.RegisteredUser.UserName);
            newUserPasswordSalt!.Should()!.NotBe(oldUserPasswordSalt!);
            newTokens.Should()!.NotBeEquivalentTo(tokens);
            currentUserAgain!.Should()!.BeEquivalentTo(currentUser);
        }
    }

    [Fact]
    public async Task LoginFlow_LogsInUserAndAllowsToChangePasswordAndTokens()
    {
        // Arrange.
        var newPassword = _test.LoggedInUser.Password + "new";
        var changePasswordDto = new ChangePasswordDto
        {
            CurrentPassword = _test.LoggedInUser.Password,
            NewPassword = newPassword, RepeatNewPassword = newPassword
        };

        // Act.
        var tokens = await _test.LoginAsync();
        var currentUser = await _test.HttpClient.GetFromJsonAsync<UserDto>("api/users");
        var oldUserPasswordSalt = _testDbHelper.GetUserByEmail(_test.LoggedInUser.Email)!.PasswordHash;
        var changePasswordResponse = await _test.HttpClient.PostAsJsonAsync("api/auth/change-password", changePasswordDto);
        var newUserPasswordSalt = _testDbHelper.GetUserByEmail(_test.LoggedInUser.Email)!.PasswordHash;
        var refreshResponse = await _test.HttpClient.PostAsJsonAsync("api/auth/refresh", tokens);
        var newTokens = await refreshResponse.Content.ReadFromJsonAsync<TokenPairDto>();
        _test.SetAuthorizationHeader(newTokens!.AccessToken);
        var currentUserAgain = await _test.HttpClient.GetFromJsonAsync<UserDto>("api/users");

        // Assert.
        using (new AssertionScope())
        {
            changePasswordResponse.EnsureSuccessStatusCode();
            currentUser!.Should()!.NotBeNull();
            currentUser!.Email.Should()!.Be(_test.LoggedInUser.Email);
            newUserPasswordSalt!.Should()!.NotBe(oldUserPasswordSalt!);
            newTokens.Should()!.NotBeEquivalentTo(tokens);
            currentUserAgain!.Should()!.BeEquivalentTo(currentUser);
        }
    }
}