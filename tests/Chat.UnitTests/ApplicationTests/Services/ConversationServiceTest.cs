using System.Linq.Expressions;
using Chat.Application.Services.Conversations;
using Chat.Application.Services.Users;
using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Entities.Conversations;
using Chat.Domain.Web;
using Chat.DomainServices.Repositories;
using Chat.DomainServices.UnitsOfWork;

namespace Chat.UnitTests.ApplicationTests.Services;

public sealed class ConversationServiceTest
{
    private const string UserName = "username";
    private const int Id = 1;
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
        const int pageSize = 5;
        const int page = 2;
        const int totalPagesCount = 2;
        var conversations = GetConversations();
        var pageSearchDto = new PagedSearchDto { Page = 2, SortingProperty = nameof(Conversation.Title) };
        var expectedTitles = new[] { "title6", "title7", "title8", "title9" };
        var expectedPageInfo = new PageInfo
        {
            CurrentPage = page, PageSize = pageSize, TotalCount = conversations.Count, TotalPages = totalPagesCount
        };
        _conversationsRepositoryMock.Setup(x => x.SearchWhere<ConversationBasicInfoDto>(pageSearchDto.SearchFilter))
                                    .Returns(conversations.AsQueryable());
        // Act.
        var result = await _sut.SearchConversationsPagedAsync(pageSearchDto);
        var resultTitles = result.Conversations?.Select(x => x.Title).ToArray();
        
        // Assert.
        result.Should()!.BeOfType<ConversationsPageDto>()!.And!.NotBeNull();
        resultTitles.Should()!.NotBeNull().And.BeEquivalentTo(expectedTitles);
        result.PageInfo.Should()!.NotBeNull().And.BeEquivalentTo(expectedPageInfo);
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
    
    private List<Conversation> GetConversations() => new()
    {
        new() { Id = 1, Title = "title1", Type = ConversationType.Dialog },
        new() { Id = 2, Title = "title2", Type = ConversationType.Dialog },
        new() { Id = 3, Title = "title3", Type = ConversationType.Dialog },
        new() { Id = 4, Title = "title4", Type = ConversationType.Group },
        new() { Id = 5, Title = "title5", Type = ConversationType.Dialog },
        new() { Id = 6, Title = "title6", Type = ConversationType.Dialog },
        new() { Id = 7, Title = "title7", Type = ConversationType.Group },
        new() { Id = 8, Title = "title8", Type = ConversationType.Dialog },
        new() { Id = 9, Title = "title9", Type = ConversationType.Dialog },
    };
}