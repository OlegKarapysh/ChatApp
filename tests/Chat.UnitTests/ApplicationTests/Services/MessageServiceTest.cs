using Chat.Domain.DTOs.Messages;

namespace Chat.UnitTests.ApplicationTests.Services;

public sealed class MessageServiceTest
{
    private const int Id = 1;
    private const string UserName = "username";
    private const string MessageText = "this is message";
    private readonly IMessageService _sut;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IRepository<Message, int>> _messageRepositoryMock = new();

    public MessageServiceTest()
    {
        _unitOfWorkMock.Setup(x => x.GetRepository<Message, int>()).Returns(_messageRepositoryMock.Object);
        _sut = new MessageService(_unitOfWorkMock.Object, _userServiceMock.Object);
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
        result.Should()!.BeOfType<MessagesPageDto>()!.And!.NotBeNull();
        result.Messages!.Should()!.NotBeNull()!.And!
              .BeEquivalentTo(expectedMessagesPage.Messages!,
                  o => o.WithStrictOrdering()!
                        .Excluding(x => x.CreatedAt)!
                        .Excluding(x => x.UpdatedAt));
        result.PageInfo!.Should()!.NotBeNull()!.And!.BeEquivalentTo(expectedMessagesPage.PageInfo);
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
        result.UserName.Should()!.Be(sender.UserName);
        result.ConversationId.Should()!.Be(messageDto.ConversationId);
        result.SenderId.Should()!.Be(messageDto.SenderId);
        result.TextContent.Should()!.Be(messageDto.TextContent);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
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
}