namespace Chat.IntegrationTests.WebApiTests;

public sealed class MessagesTest : IClassFixture<IntegrationTest>
{
    private readonly IntegrationTest _test;

    public MessagesTest(IntegrationTest test)
    {
        _test = test;
    }
    
    [Theory]
    [InlineData(2, 22, 5, 5, "")]
    [InlineData(5, 22, 5, 2, "")]
    [InlineData(1, 0, 0, 0, "1234123412341234123412342134123423412342342341234129")]
    [InlineData(1, 1, 1, 1, "something")]
    public async Task SearchUsersPaged_ReturnsCorrectUsersPage(
        int page, int totalCount, int totalPages, int expectedCount, string search)
    {
        // Arrange.
        await _test.LoginAsync();
        const string route = "api/messages/search";
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
            SortingProperty = nameof(Message.TextContent),
            SortingOrder = SortingOrder.Descending
        };
        var routeWithParams =
            $"{route}?{nameof(searchDto.Page)}={searchDto.Page}" +
            $"&{nameof(searchDto.SearchFilter)}={searchDto.SearchFilter}" +
            $"&{nameof(searchDto.SortingProperty)}={searchDto.SortingProperty}" +
            $"&{nameof(searchDto.SortingOrder)}={(int)searchDto.SortingOrder}";

        // Act.
        var response = await _test.HttpClient.GetAsync(routeWithParams);
        var result = await response.Content.ReadFromJsonAsync<MessagesPageDto>();

        // Assert.
        using (new AssertionScope())
        {
            response.EnsureSuccessStatusCode();
            result!.Should()!.NotBeNull();
            result!.PageInfo!.Should()!.BeEquivalentTo(expectedPageInfo);
            result.Messages!.Length.Should()!.Be(expectedCount);
            result.Messages!.Should()!.BeEquivalentTo(result.Messages.OrderByDescending(x => x.TextContent));
        }
    }
}