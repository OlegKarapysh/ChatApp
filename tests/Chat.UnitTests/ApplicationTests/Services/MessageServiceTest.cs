namespace Chat.UnitTests.ApplicationTests.Services;

public sealed class MessageServiceTest
{
    private const int Id = 1;
    private const string UserName = "username";
    private const string MessageText = "this is message";
    private readonly IMessageService _sut;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IOpenAiService> _openAiServiceMock = new();
    private readonly Mock<IRepository<Message, int>> _messageRepositoryMock = new();

    public MessageServiceTest()
    {
        _unitOfWorkMock.Setup(x => x.GetRepository<Message, int>()).Returns(_messageRepositoryMock.Object);
        _sut = new MessageService(_userServiceMock.Object, _unitOfWorkMock.Object, _openAiServiceMock.Object);
    }

    [Fact]
    public async Task SearchMessagesPagedAsync_ReturnsMessagesPage_WhenValidPagedSearchDto()
    {
        // Arrange.
        var (messages, expectedMessagesPage) = GetTestMessagesWithMessagesPage();
        var pageSearchDto = new PagedSearchDto
        {
            Page = expectedMessagesPage.PageInfo!.CurrentPage, SearchFilter = "filter",
            SortingProperty = nameof(Message.TextContent), SortingOrder = SortingOrder.Descending
        };
        _messageRepositoryMock.Setup(x => x.SearchWhere<MessageBasicInfoDto>(pageSearchDto.SearchFilter))
                           .Returns(messages.AsQueryable());
        // Act.
        var result = await _sut.SearchMessagesPagedAsync(pageSearchDto);
        
        // Assert.
        using (new AssertionScope())
        {
            result.Should()!.BeOfType<MessagesPageDto>()!.And!.NotBeNull();
            result.Messages!.Should()!.NotBeNull()!.And!
                  .BeEquivalentTo(expectedMessagesPage.Messages!,
                      o => o.WithStrictOrdering()!
                            .Excluding(x => x.CreatedAt)!
                            .Excluding(x => x.UpdatedAt));
            result.PageInfo!.Should()!.NotBeNull()!.And!.BeEquivalentTo(expectedMessagesPage.PageInfo);
        }
    }

    [Fact]
    public async Task GetAllConversationMessagesAsync_ReturnsConversationMessages()
    {
        // Arrange.
        var conversationMessages = TestDataGenerator.GenerateMessagesForDialog(10, Id);
        var allMessages = TestDataGenerator.GenerateMessagesForDialog(10, Id + 1)
                                           .Concat(conversationMessages);
        var expectedMessages = conversationMessages.Select(x => x.MapToDtoWithSender());
        _messageRepositoryMock.Setup(x => x.AsQueryable()).Returns(allMessages.BuildMock()!);
        
        // Act.
        var result = await _sut.GetAllConversationMessagesAsync(Id);

        // Assert.
        result.Should()!.BeEquivalentTo(expectedMessages);
    }
    
    [Fact]
    public async Task GetAllConversationMessagesAsync_ReturnsEmptyList_WhenNoMessagesInConversation()
    {
        // Arrange.
        var allMessages = TestDataGenerator.GenerateMessagesForDialog(10, Id + 1);
        _messageRepositoryMock.Setup(x => x.AsQueryable()).Returns(allMessages.BuildMock()!);
        
        // Act.
        var result = await _sut.GetAllConversationMessagesAsync(Id);

        // Assert.
        result.Should()!.BeEmpty();
    }

    [Fact]
    public void UpdateMessageAsync_ThrowsEntityNotFoundException_WhenMessageNotFound()
    {
        // Arrange.
        _messageRepositoryMock.Setup(x => x.GetByIdAsync(Id)).ReturnsAsync((Message)null!);
        
        // Act.
        var tryUpdateMessage = async () => await _sut.UpdateMessageAsync(new MessageDto(), Id);
        
        // Assert.
        tryUpdateMessage.Should()!.ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public void UpdateMessageAsync_ThrowsInvalidMessageUpdaterException_WhenInvalidUpdater()
    {
        // Arrange.
        const int invalidUpdaterId = Id + 1;
        var message = TestDataGenerator.GenerateMessage(Id);
        var messageDto = message.MapToDto();
        _messageRepositoryMock.Setup(x => x.GetByIdAsync(Id)).ReturnsAsync(message);

        // Act.
        var tryUpdateMessage = async () => await _sut.UpdateMessageAsync(messageDto, invalidUpdaterId);

        // Assert.
        tryUpdateMessage.Should()!.ThrowAsync<InvalidMessageUpdaterException>();
    }
    
    [Fact]
    public async Task UpdateMessageAsync_UpdatesMessage_WhenValidDto()
    {
        // Arrange.
        const string oldMessageText = "oldMessage";
        const string updatedMessageText = "newMessage";
        var message = TestDataGenerator.GenerateMessage(Id, oldMessageText);
        var updatedMessage = CopyMessage(message);
        updatedMessage.TextContent = updatedMessageText;
        var messageDto = updatedMessage.MapToDto();
        _messageRepositoryMock.Setup(x => x.GetByIdAsync(message.Id)).ReturnsAsync(message);
        _messageRepositoryMock.Setup(x => x.Update(
                                  It.Is<Message>(m => m.TextContent == updatedMessageText && m.SenderId == message.SenderId)))
                              .Returns(updatedMessage);

        // Act.
        var result = await _sut.UpdateMessageAsync(messageDto, messageDto.SenderId);

        // Assert.
        using (new AssertionScope())
        {
            result.Should()!.BeEquivalentTo(messageDto);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }

    [Fact]
    public async Task DeleteMessageAsync_DeletesMessage_WhenMessageFound()
    {
        // Arrange.
        _messageRepositoryMock.Setup(x => x.RemoveAsync(Id)).ReturnsAsync(true);
        
        // Act.
        var result = await _sut.DeleteMessageAsync(Id);

        // Assert.
        using (new AssertionScope())
        {
            result.Should()!.BeTrue();
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }

    [Fact]
    public async Task CreateMessageAsync_CreatesMessageAndReturnsMessageDto_WhenValidDto()
    {
        // Arrange.
        const int conversationId = 5;
        const int senderId = 10;
        var messageDto = new MessageDto
        {
            Id = Id, ConversationId = conversationId, SenderId = senderId, TextContent = MessageText
        };
        var message = new Message
        {
            Id = Id, ConversationId = conversationId, SenderId = senderId, TextContent = MessageText
        };
        var sender = new User { UserName = UserName };
        _messageRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Message>()))
                               .ReturnsAsync(message);
        _userServiceMock.Setup(x => x.GetUserByIdAsync(senderId)).ReturnsAsync(sender);
        
        // Act.
        var result = await _sut.CreateMessageAsync(messageDto);

        // Assert.
        using (new AssertionScope())
        {
            result.UserName.Should()!.Be(sender.UserName);
            result.ConversationId.Should()!.Be(messageDto.ConversationId);
            result.SenderId.Should()!.Be(messageDto.SenderId);
            result.TextContent.Should()!.Be(messageDto.TextContent);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
    
    private (List<Message> Messages, MessagesPageDto MessagesPage) GetTestMessagesWithMessagesPage()
    {
        return (new List<Message>
        {
            new() { TextContent = "text01" },
            new() { TextContent = "text09" },
            new() { TextContent = "text02" },
            new() { TextContent = "text08" },
            new() { TextContent = "text03" },
            new() { TextContent = "text07" },
            new() { TextContent = "text04" },
            new() { TextContent = "text06" },
            new() { TextContent = "text05" },
            new() { TextContent = "text10" },
            new() { TextContent = "text11" },
        }, new MessagesPageDto
        {
            Messages = new MessageBasicInfoDto[]
            {
                new() { TextContent = "text06" },
                new() { TextContent = "text05" },
                new() { TextContent = "text04" },
                new() { TextContent = "text03" },
                new() { TextContent = "text02" },
            },
            PageInfo = new PageInfo { CurrentPage = 2, PageSize = PageInfo.DefaultPageSize, TotalCount = 11, TotalPages = 3 }
        });
    }

    private Message CopyMessage(Message message) => new()
    {
        Id = message.Id,
        ConversationId = message.ConversationId,
        SenderId = message.SenderId,
        Sender = message.Sender,
        Conversation = message.Conversation,
        TextContent = message.TextContent,
        CreatedAt = message.CreatedAt,
        UpdatedAt = message.UpdatedAt
    };
}