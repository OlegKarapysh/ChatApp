namespace Chat.IntegrationTests.WebApiTests;

public sealed class ConversationsModifyTest : IClassFixture<IntegrationTest>
{
    private readonly IntegrationTest _test;
    private readonly TestDbHelper _testDbHelper;

    public ConversationsModifyTest(IntegrationTest test)
    {
        _test = test;
        _testDbHelper = new TestDbHelper(_test.TestAppFactory);
    }

    [Fact]
    public async Task CreateDialog_CreatesDialogForCurrentUser()
    {
        // Arrange.
        await _test.LoginAsync();
        var newDialogDto = new NewDialogDto { InterlocutorUserName = "user4" };
        
        // Act.
        var dialogResponse = await _test.HttpClient.PostAsJsonAsync("api/conversations/dialogs", newDialogDto);
        var createdDialog = await dialogResponse.Content.ReadFromJsonAsync<DialogDto>();
        var userConversationIdsResponse = await _test.HttpClient.GetAsync("api/conversations/all-ids");
        var userConversationIds = await userConversationIdsResponse.Content.ReadFromJsonAsync<IList<int>>();
        
        // Assert.
        using (new AssertionScope())
        {
            dialogResponse.EnsureSuccessStatusCode();
            userConversationIdsResponse.EnsureSuccessStatusCode();
            createdDialog!.Should()!.NotBeNull();
            createdDialog!.Title.Should()!.Contain(newDialogDto.InterlocutorUserName);
            userConversationIds!.Should()!.NotBeNull();
            userConversationIds!.Should()!.Contain(createdDialog.Id);
        }
    }

    [Fact]
    public async Task AddAndRemoveConversationMember()
    {
        // Arrange.
        await _test.LoginAsync();
        const int conversationId = 21;
        var newGroupMemberDto = new NewGroupMemberDto { ConversationId = conversationId, MemberUserName = "OlehKarapysh" };

        // Act.
        var userConversationIdsBeforeAdding =
            await _test.HttpClient.GetFromJsonAsync<IList<int>>("api/conversations/all-ids");
        var addConversationMemberResponse =
            await _test.HttpClient.PostAsJsonAsync("api/conversations/members", newGroupMemberDto);
        var addedConversationDto = await addConversationMemberResponse.Content.ReadFromJsonAsync<ConversationDto>();
        var userConversationIdsAfterAdding =
            await _test.HttpClient.GetFromJsonAsync<IList<int>>("api/conversations/all-ids");
        var removeFromConversationResponse = await _test.HttpClient.DeleteAsync($"api/Conversations/{conversationId}");
        var userConversationIdsAfterRemoving =
            await _test.HttpClient.GetFromJsonAsync<IList<int>>("api/conversations/all-ids");
        
        // Assert.
        using (new AssertionScope())
        {
            removeFromConversationResponse.EnsureSuccessStatusCode();
            addConversationMemberResponse.EnsureSuccessStatusCode();
            addedConversationDto!.Id.Should()!.Be(conversationId);
            userConversationIdsBeforeAdding!.Should()!.NotBeNull()!.And!.NotContain(addedConversationDto.Id);
            userConversationIdsAfterAdding!.Should()!.NotBeNull()!.And!.Contain(addedConversationDto.Id);
            userConversationIdsAfterRemoving!.Should()!.NotBeNull()!.And!.NotContain(addedConversationDto.Id);
        }
    }
}