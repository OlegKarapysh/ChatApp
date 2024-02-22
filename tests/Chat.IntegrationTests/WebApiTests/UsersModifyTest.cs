namespace Chat.IntegrationTests.WebApiTests;

[Collection("Sequential")]
public sealed class UsersModifyTest : IClassFixture<IntegrationTest>
{
    private readonly IntegrationTest _test;
    private readonly TestDbHelper _testDbHelper;
    
    public UsersModifyTest(IntegrationTest test)
    {
        _test = test;
        _testDbHelper = new TestDbHelper(_test.TestAppFactory);
    }

    [Fact]
    public async Task Update_UpdatesCurrentUser()
    {
        // Arrange.
        await _test.LoginAsync();
        const string route = "api/users";
        var user = _testDbHelper.GetUserByEmail(_test.LoggedInUser.Email)!;
        var updatedFirstName = user.FirstName += "updated";
        var updatedPhone = user.PhoneNumber += "1";
        var userDto = new UserDto
        {
            Email = user.Email!, FirstName = updatedFirstName, LastName = user.LastName!,
            PhoneNumber = updatedPhone, UserName = user.UserName!
        };
        
        // Act.
        user.FirstName = updatedFirstName;
        user.PhoneNumber = updatedPhone;
        var response = await _test.HttpClient.PutAsJsonAsync(route, userDto);
        var updatedUser = await _test.HttpClient.GetFromJsonAsync<UserDto>(route);

        // Assert.
        using (new AssertionScope())
        {
            response.EnsureSuccessStatusCode();
            updatedUser!.Should()!.NotBeNull();
            updatedUser!.FirstName.Should()!.Be(updatedFirstName);
            updatedUser.PhoneNumber.Should()!.Be(updatedPhone);
        }
    }
}