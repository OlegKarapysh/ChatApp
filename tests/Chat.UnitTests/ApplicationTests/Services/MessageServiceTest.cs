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
}