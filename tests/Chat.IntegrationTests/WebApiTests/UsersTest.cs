namespace Chat.IntegrationTests.WebApiTests;

[Collection("Sequential")]
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
        await _test.LoginAsync();
        const string route = "api/users/all";
        
        // Act.
        var response = await _test.HttpClient.GetAsync(route);
        var result = await response.Content.ReadFromJsonAsync<IList<UserDto>>();
        
        // Assert.
        using (new AssertionScope())
        {
            response.EnsureSuccessStatusCode();
            result!.Should()!.NotBeNull();
            result!.Count.Should()!.BeGreaterOrEqualTo(1);
        }
    }

    [Fact]
    public async Task GetCurrentUser_ReturnsCurrentUser()
    {
        // Arrange.
        await _test.LoginAsync();
        var expectedEmail = _test.LoggedInUser.Email;
        const string route = "api/users";

        // Act.
        var response = await _test.HttpClient.GetAsync(route);
        var result = await response.Content.ReadFromJsonAsync<UserDto>();

        // Assert.
        using (new AssertionScope())
        {
            response.EnsureSuccessStatusCode();
            result!.Should()!.NotBeNull();
            result!.Email.Should()!.Be(expectedEmail);
        }
    }

    [Theory]
    [InlineData(2, 7, 2, 2, "")]
    [InlineData(1, 0, 0, 0, "1234123412341234123412342134123423412342342341234129")]
    [InlineData(1, 1, 1, 1, "Alice")]
    public async Task SearchUsersPaged_ReturnsCorrectUsersPage(
        int page, int totalCount, int totalPages, int expectedCount, string search)
    {
        // Arrange.
        await _test.LoginAsync();
        const string route = "api/users/search";
        var expectedPageInfo = new PageInfo
        {
            CurrentPage = page,
            PageSize = PageInfo.DefaultPageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
        var searchDto = new PagedSearchDto
        {
            Page = expectedPageInfo.CurrentPage,
            SearchFilter = search,
            SortingProperty = nameof(User.UserName),
            SortingOrder = SortingOrder.Descending
        };
        var routeWithParams =
            $"{route}?{nameof(searchDto.Page)}={searchDto.Page}" +
            $"&{nameof(searchDto.SearchFilter)}={searchDto.SearchFilter}" +
            $"&{nameof(searchDto.SortingProperty)}={searchDto.SortingProperty}" +
            $"&{nameof(searchDto.SortingOrder)}={(int)searchDto.SortingOrder}";

        // Act.
        var response = await _test.HttpClient.GetAsync(routeWithParams);
        var result = await response.Content.ReadFromJsonAsync<UsersPageDto>();

        // Assert.
        using (new AssertionScope())
        {
            response.EnsureSuccessStatusCode();
            result!.Should()!.NotBeNull();
            result!.PageInfo!.Should()!.BeEquivalentTo(expectedPageInfo);
            result.Users!.Length.Should()!.Be(expectedCount);
            result.Users!.Should()!.BeEquivalentTo(result.Users.OrderByDescending(x => x.UserName));
        }
    }
}