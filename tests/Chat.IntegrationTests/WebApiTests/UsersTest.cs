namespace Chat.IntegrationTests.WebApiTests;

public sealed class UsersTest : IClassFixture<IntegrationTest>
{
    private readonly IntegrationTest _test;
    
    public UsersTest(IntegrationTest test)
    {
        _test = test;
    }
    
    [Fact]
    public async Task GetAllUsers_ReturnsAllUsers()
    {
        // Arrange.
        await _test.RegisterAsync();
        const string route = "api/users/all";
        
        // Act.
        var response = await _test.HttpClient.GetAsync(route);
        var result = await response.Content.ReadFromJsonAsync<IList<UserDto>>();
        
        // Assert.
        response.EnsureSuccessStatusCode();
        result!.Should()!.NotBeNull();
        result!.Count.Should()!.BeGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task GetCurrentUser_ReturnsCurrentUser()
    {
        // Arrange.
        await _test.LoginAsync();
        var expectedEmail = _test.LoggedInUser.Email;
        const string route = "api/users/";

        // Act.
        var response = await _test.HttpClient.GetAsync(route);
        var result = await response.Content.ReadFromJsonAsync<UserDto>();

        // Assert.
        response.EnsureSuccessStatusCode();
        result!.Should()!.NotBeNull();
        result!.Email.Should()!.Be(expectedEmail);
    }

    [Fact]
    public async Task SearchUsersPaged_ReturnsUsersPage()
    {
        // Arrange.
        await _test.LoginAsync();
        const string route = "api/users/search";
        var expectedPageInfo = new PageInfo
        {
            CurrentPage = 2,
            PageSize = PageInfo.DefaultPageSize,
            TotalCount = 7,
            TotalPages = 2
        };
        var searchDto = new PagedSearchDto
        {
            Page = expectedPageInfo.CurrentPage,
            SearchFilter = string.Empty,
            SortingProperty = nameof(User.UserName),
            SortingOrder = SortingOrder.Descending
        };
        var routeWithParams =
            $"{route}?{nameof(searchDto.Page)}={searchDto.Page}" +
            $"&{nameof(searchDto.SearchFilter)}={searchDto.SearchFilter}" +
            $"&{nameof(searchDto.SortingProperty)}={searchDto.SortingProperty}" +
            $"&{nameof(searchDto.SortingOrder)}={(int)searchDto.SortingOrder}";
        const int expectedCount = 2;

        // Act.
        var response = await _test.HttpClient.GetAsync(routeWithParams);
        var result = await response.Content.ReadFromJsonAsync<UsersPageDto>();

        // Assert.
        response.EnsureSuccessStatusCode();
        result!.Should()!.NotBeNull();
        result!.PageInfo!.Should()!.BeEquivalentTo(expectedPageInfo);
        result.Users!.Length.Should()!.Be(expectedCount);
        result.Users!.Should()!.BeEquivalentTo(result.Users.OrderByDescending(x => x.UserName));
    }
}