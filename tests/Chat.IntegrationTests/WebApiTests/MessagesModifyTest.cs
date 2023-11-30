namespace Chat.IntegrationTests.WebApiTests;

[Collection("Sequential")]
public sealed class MessagesModifyTest : IClassFixture<IntegrationTest>
{
    private readonly IntegrationTest _test;
    private readonly TestDbHelper _testDbHelper;

    public MessagesModifyTest(IntegrationTest test)
    {
        _test = test;
        _testDbHelper = new TestDbHelper(_test.TestAppFactory);
    }

    [Fact]
    public async Task AddMessage_AddExpectedMessage()
    {
        // Arrange.
        await _test.LoginAsync();
        const int conversationId = 16;
        var allMessagesRoute = $"api/messages/all/{conversationId}";
        var messageDto = new MessageDto { TextContent = "new message", ConversationId = conversationId };
        
        // Act.
        var messagesBeforeAdding = await _test.HttpClient.GetFromJsonAsync<IList<MessageWithSenderDto>>(allMessagesRoute);
        var addMessageResponse = await _test.HttpClient.PostAsJsonAsync("api/messages", messageDto);
        var addedMessage = await addMessageResponse.Content.ReadFromJsonAsync<MessageWithSenderDto>();
        var messagesAfterAdding = await _test.HttpClient.GetFromJsonAsync<IList<MessageWithSenderDto>>(allMessagesRoute);
        
        // Assert.
        using (new AssertionScope())
        {
            addMessageResponse.EnsureSuccessStatusCode();
            addedMessage!.Should()!.NotBeNull();
            addedMessage!.ConversationId.Should()!.Be(conversationId);
            addedMessage!.TextContent.Should()!.Be(messageDto.TextContent);
            messagesBeforeAdding!.Should()!.NotContain(x => x.Id == addedMessage.Id);
            messagesAfterAdding!.Should()!.ContainSingle(x => x.Id == addedMessage.Id);
        }
    }
    
    [Fact]
    public async Task DeleteMessage_DeletesMessage()
    {
        // Arrange.
        await _test.LoginAsync();
        const int messageId = 109;
        const int conversationId = 17;
        var allMessagesRoute = $"api/messages/all/{conversationId}";
        
        // Act.
        var messagesBeforeAdding = await _test.HttpClient.GetFromJsonAsync<IList<MessageWithSenderDto>>(allMessagesRoute);
        var deleteMessageResponse = await _test.HttpClient.DeleteAsync($"api/messages/{messageId}");
        var messagesAfterAdding = await _test.HttpClient.GetFromJsonAsync<IList<MessageWithSenderDto>>(allMessagesRoute);
        
        // Assert.
        using (new AssertionScope())
        {
            deleteMessageResponse.EnsureSuccessStatusCode();
            messagesBeforeAdding!.Should()!.ContainSingle(x => x.Id == messageId);
            messagesAfterAdding!.Should()!.NotContain(x => x.Id == messageId);
        }
    }

    [Fact]
    public async Task UpdateMessage_UpdatesExpectedMessage()
    {
        // Arrange.
        await _test.LoginAsync();
        const int conversationId = 17;
        const int messageId = 110;
        var messageDto = new MessageDto { Id = messageId, ConversationId = conversationId, TextContent = "TestUpdated" };
        var messageBeforeUpdating = _testDbHelper.GetMessageById(messageId)!;
        
        // Act.
        var updateResponse = await _test.HttpClient.PutAsJsonAsync("api/messages", messageDto);
        var updatedMessage = await updateResponse.Content.ReadFromJsonAsync<MessageDto>();
        var messageAfterUpdating = _testDbHelper.GetMessageById(messageId)!;
        
        // Assert.
        using (new AssertionScope())
        {
            updateResponse.EnsureSuccessStatusCode();
            updatedMessage!.Should()!.NotBeNull();
            updatedMessage!.TextContent.Should()!.Be(messageDto.TextContent);
            updatedMessage.ConversationId.Should()!.Be(conversationId);
            messageBeforeUpdating.TextContent.Should()!.NotBe(messageDto.TextContent);
            messageAfterUpdating.TextContent.Should()!.Be(messageDto.TextContent);
        }
    }
}