namespace Chat.IntegrationTests.WebApiTests;

public sealed class UsersTest : IntegrationTestBase
{
    [Fact]
    public async Task GetAllUsers_ReturnsAllUsers()
    {
        // Arrange.
        const string path = "api/users/all";
        await AuthenticateAsync();
        
        // Act.
        var response = await HttpClient.GetAsync(path);
        var result = await response.Content.ReadFromJsonAsync<IList<UserDto>>();
        
        // Assert.
        response.EnsureSuccessStatusCode();
        result!.Should()!.NotBeNull();
        result!.Count.Should()!.BeGreaterOrEqualTo(1);
    }
}