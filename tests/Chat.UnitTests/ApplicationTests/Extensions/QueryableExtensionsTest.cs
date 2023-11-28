namespace Chat.UnitTests.ApplicationTests.Extensions;

public sealed class QueryableExtensionsTest
{
    [Theory]
    [InlineData("title", 5)]
    [InlineData("title0", 2)]
    [InlineData("title3", 1)]
    [InlineData("ItShouldFindNothing", 0)]
    public void SearchWhere_ReturnsFilteredEntitiesByTheirProperties(string filter, int expectedCount)
    {
        // Arrange.
        var allConversations = new List<Conversation>
        {
            new() { Id = 500, Title = "title01", CreatedAt = new DateTime(), UpdatedAt = new DateTime() },
            new() { Id = 501, Title = "title02", CreatedAt = new DateTime(), UpdatedAt = new DateTime() },
            new() { Id = 502, Title = "title3", CreatedAt = new DateTime(), UpdatedAt = new DateTime() },
            new() { Id = 503, Title = "title4", CreatedAt = new DateTime(), UpdatedAt = new DateTime() },
            new() { Id = 504, Title = "title5", CreatedAt = new DateTime(), UpdatedAt = new DateTime() },
        }.AsQueryable();
        
        // Act.
        var result = allConversations.SearchWhere<Conversation, ConversationBasicInfoDto>(filter);

        // Assert.
        using (new AssertionScope())
        {
            result.Count().Should()!.Be(expectedCount);
            result.Should()!.BeAssignableTo<IQueryable<Conversation>>();
        }
    }
}