namespace Chat.IntegrationTests.WebApiTests;

[Collection("Sequential")]
public sealed class ConversationsTest : IClassFixture<IntegrationTest>
{
    private readonly IntegrationTest _test;
    private readonly TestDbHelper _testDbHelper;

    public ConversationsTest(IntegrationTest test)
    {
        _test = test;
        _testDbHelper = new TestDbHelper(_test.TestAppFactory);
    }

    [Fact]
    public async Task GetAllUserConversations_ReturnsAllUserConversations()
    {
        // Arrange.
        await _test.LoginAsync();
        
        // Act.
        var conversationIdsResponse = await _test.HttpClient.GetAsync("api/conversations/all-ids");
        var conversationIds = await conversationIdsResponse.Content.ReadFromJsonAsync<IList<int>>();
        var userConversationsResponse = await _test.HttpClient.GetAsync("api/conversations/all");
        var result = await userConversationsResponse.Content.ReadFromJsonAsync<IList<ConversationDto>>();
        var resultIds = result?.Select(x => x.Id).ToList() ?? new List<int>();

        // Assert.
        using (new AssertionScope())
        {
            userConversationsResponse.EnsureSuccessStatusCode();
            conversationIdsResponse.EnsureSuccessStatusCode();
            conversationIds!.Should()!.NotBeNull()!.And!.HaveCountGreaterThanOrEqualTo(1);
            result!.Should()!.NotBeNull();
            resultIds.Should()!.BeEquivalentTo(conversationIds!);
        }
    }

    [Theory]
    [InlineData(2, 8, 2, 3, "")]
    [InlineData(1, 0, 0, 0, "1234123412341234123412342134123423412342342341234129")]
    [InlineData(1, 1, 1, 1, "billy's")]
    public async Task SearchConversationsPaged_ReturnsCorrectConversationsPage(
        int page, int totalCount, int totalPages, int expectedCount, string search)
    {
        // Arrange.
        await _test.LoginAsync();
        const string route = "api/conversations/search";
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
            SortingProperty = nameof(Conversation.Title),
            SortingOrder = SortingOrder.Descending
        };
        var routeWithParams =
            $"{route}?{nameof(searchDto.Page)}={searchDto.Page}" +
            $"&{nameof(searchDto.SearchFilter)}={searchDto.SearchFilter}" +
            $"&{nameof(searchDto.SortingProperty)}={searchDto.SortingProperty}" +
            $"&{nameof(searchDto.SortingOrder)}={(int)searchDto.SortingOrder}";

        // Act.
        var response = await _test.HttpClient.GetAsync(routeWithParams);
        var result = await response.Content.ReadFromJsonAsync<ConversationsPageDto>();

        // Assert.
        using (new AssertionScope())
        {
            response.EnsureSuccessStatusCode();
            result!.Should()!.NotBeNull();
            result!.PageInfo!.Should()!.BeEquivalentTo(expectedPageInfo);
            result.Conversations!.Length.Should()!.Be(expectedCount);
            result.Conversations!.Should()!.BeEquivalentTo(result.Conversations.OrderByDescending(x => x.Title));
        }
    }
}