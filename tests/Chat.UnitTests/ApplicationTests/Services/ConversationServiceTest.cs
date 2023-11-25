namespace Chat.UnitTests.ApplicationTests.Services;

public sealed class ConversationServiceTest
{
    private const int Id = 1;
    private const string Title = "title1";
    private const string UserName = "username";
    private readonly IConversationService _sut;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IRepository<ConversationParticipants, int>> _participantsRepositoryMock = new();
    private readonly Mock<IRepository<Conversation, int>> _conversationsRepositoryMock = new();

    public ConversationServiceTest()
    {
        _unitOfWorkMock.Setup(x => x.GetRepository<ConversationParticipants, int>())
                       .Returns(_participantsRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.GetRepository<Conversation, int>())
                       .Returns(_conversationsRepositoryMock.Object);
        _sut = new ConversationService(_unitOfWorkMock.Object, _userServiceMock.Object);
    }

    [Fact]
    public async Task GetUserConversationIdsAsync_ReturnsAllConversationIds()
    {
        // Arrange.
        var conversationIds = new[] { 1, int.MaxValue };
        var conversationParticipants = new List<ConversationParticipants>
        {
            new() { ConversationId = conversationIds[0] }, new() { ConversationId = conversationIds[1] }
        };
        _participantsRepositoryMock.Setup(x =>
                                       x.FindAllAsync(It.IsAny<Expression<Func<ConversationParticipants, bool>>>()))
                                   .ReturnsAsync(conversationParticipants);
        // Act.
        var result = await _sut.GetUserConversationIdsAsync(default);

        // Assert.
        result.Should()!.BeEquivalentTo(conversationIds);
    }

    [Fact]
    public async Task SearchConversationsPagedAsync_ReturnsConversationsPage_WhenValidPageSearchDto()
    {
        // Arrange.
        var (conversations, expectedConversationsPage) = GetConversationsWithConversationsPage();
        var pageSearchDto = new PagedSearchDto
        {
            Page = expectedConversationsPage.PageInfo!.CurrentPage, SortingProperty = nameof(Conversation.Title),
            SortingOrder = SortingOrder.Descending, SearchFilter = "filter"
        };
        _conversationsRepositoryMock.Setup(x => x.SearchWhere<ConversationBasicInfoDto>(pageSearchDto.SearchFilter))
                                    .Returns(conversations.AsQueryable());
        // Act.
        var result = await _sut.SearchConversationsPagedAsync(pageSearchDto);
        
        // Assert.
        result.Should()!.BeOfType<ConversationsPageDto>()!.And!.NotBeNull();
        result.Conversations.Should()!.NotBeNull().And.BeEquivalentTo(expectedConversationsPage.Conversations!,
            o => o.WithStrictOrdering()!.Excluding(x => x.CreatedAt)!.Excluding(x => x.UpdatedAt));
        result.PageInfo.Should()!.NotBeNull().And.BeEquivalentTo(expectedConversationsPage.PageInfo);
    }

    [Fact]
    public async Task CreateOrGetGroupChatAsync_ReturnsGroup_WhenGroupAlreadyExists()
    {
        // Arrange.
        var newGroupDto = new NewGroupChatDto { CreatorId = Id, Title = Title };
        var creator = TestDataGenerator.GenerateUser();
        var existingGroup = new Conversation
        {
            Type = ConversationType.Group, Members = new List<User> { creator }, Title = Title, Id = Id
        };
        _userServiceMock.Setup(x => x.GetUserByIdAsync(newGroupDto.CreatorId)).ReturnsAsync(creator);
        _conversationsRepositoryMock.Setup(x => x.FindAllAsync(It.IsAny<Expression<Func<Conversation, bool>>>()))
                                    .ReturnsAsync(new List<Conversation> { existingGroup });
        // Act.
        var result = await _sut.CreateOrGetGroupChatAsync(newGroupDto);

        // Assert.
        result.Should()!.NotBeNull();
        result.Id.Should()!.Be(existingGroup.Id);
        result.Title.Should()!.Be(existingGroup.Title);
        result.Type.Should()!.Be(existingGroup.Type);
    }
    
    [Fact]
    public async Task CreateOrGetGroupChatAsync_ReturnsCreatedGroup_WhenGroupDoesntExist()
    {
        // Arrange.
        var newGroupDto = new NewGroupChatDto { CreatorId = Id, Title = Title };
        var creator = TestDataGenerator.GenerateUser();
        var createdGroupTitle = $"new {Title}";
        var createdGroup = new Conversation
        {
            Type = ConversationType.Group, Members = new List<User> { creator }, Title = createdGroupTitle, Id = Id
        };
        _userServiceMock.Setup(x => x.GetUserByIdAsync(newGroupDto.CreatorId)).ReturnsAsync(creator);
        _conversationsRepositoryMock.Setup(x => x.FindAllAsync(It.IsAny<Expression<Func<Conversation, bool>>>()))
                                    .ReturnsAsync(new List<Conversation>());
        _conversationsRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Conversation>()))
                                    .ReturnsAsync(createdGroup);
        // Act.
        var result = await _sut.CreateOrGetGroupChatAsync(newGroupDto);

        // Assert.
        result.Should()!.NotBeNull();
        result.Id.Should()!.Be(createdGroup.Id);
        result.Title.Should()!.Be(createdGroup.Title);
        result.Type.Should()!.Be(createdGroup.Type);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public void GetConversationByIdAsync_ThrowsEntityNotFoundException_WhenInvalidId()
    {
        // Arrange.
        _conversationsRepositoryMock.Setup(x => x.GetByIdAsync(Id))
                                    .ReturnsAsync((Conversation)null!);
        // Act.
        var tryGetById = async () => await _sut.GetConversationByIdAsync(Id);
        
        // Assert.
        tryGetById.Should()!.ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public void AddGroupMemberAsync_ThrowsWrongConversationTypeException_WhenWrongConversationType()
    {
        // Arrange.
        var newMemberDto = new NewGroupMemberDto { ConversationId = Id };
        var conversation = new Conversation { Type = ConversationType.Dialog, Id = Id };
        _conversationsRepositoryMock.Setup(x => x.GetByIdAsync(newMemberDto.ConversationId))
                                    .ReturnsAsync(conversation);
        
        // Act.
        var tryAddGroupMember = async () => await _sut.AddGroupMemberAsync(newMemberDto);
        
        // Assert.
        tryAddGroupMember.Should()!.ThrowAsync<WrongConversationTypeException>();
    }
    
    [Fact]
    public async Task AddGroupMemberAsync_AddsNewGroupMemberAndReturnsGroupDto_WhenValidGroupMemberDto()
    {
        // Arrange.
        var newMemberDto = new NewGroupMemberDto { ConversationId = Id, MemberUserName = UserName };
        var member = new User { Id = Id, UserName = UserName };
        var conversation = new Conversation { Id = Id, Title = "title1", Type = ConversationType.Group };
        _conversationsRepositoryMock.Setup(x => x.GetByIdAsync(Id)).ReturnsAsync(conversation);
        _userServiceMock.Setup(x => x.GetUserByNameAsync(UserName)).ReturnsAsync(member);
        
        // Act.
        var result = await _sut.AddGroupMemberAsync(newMemberDto);

        // Assert.
        result.Type.Should()!.Be(conversation.Type);
        result.Id.Should()!.Be(conversation.Id);
        result.Title.Should()!.Be(conversation.Title);
        _conversationsRepositoryMock.Verify(x => x.Update(conversation), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
    
    private (List<Conversation> Conversations, ConversationsPageDto ConversationsPage) GetConversationsWithConversationsPage() => new
    (
        new List<Conversation>
        {
            new() { Id = 1, Title = "title02", Type = ConversationType.Dialog },
            new() { Id = 2, Title = "title01", Type = ConversationType.Dialog },
            new() { Id = 3, Title = "title03", Type = ConversationType.Dialog },
            new() { Id = 4, Title = "title09", Type = ConversationType.Group },
            new() { Id = 5, Title = "title10", Type = ConversationType.Dialog },
            new() { Id = 6, Title = "title06", Type = ConversationType.Dialog },
            new() { Id = 7, Title = "title07", Type = ConversationType.Group },
            new() { Id = 8, Title = "title08", Type = ConversationType.Dialog },
            new() { Id = 9, Title = "title05", Type = ConversationType.Dialog },
            new() { Id = 8, Title = "title04", Type = ConversationType.Dialog },
            new() { Id = 9, Title = "title11", Type = ConversationType.Dialog },
        },
        new ConversationsPageDto
        {
            Conversations = new ConversationBasicInfoDto[]
            {
                new() { Title = "title06" },
                new() { Title = "title05" },
                new() { Title = "title04" },
                new() { Title = "title03" },
                new() { Title = "title02" },
            },
            PageInfo = new PageInfo { CurrentPage = 2, PageSize = PageInfo.DefaultPageSize, TotalCount = 11, TotalPages = 3 }
        }
    );
}