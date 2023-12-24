using Group = Chat.Domain.Entities.Groups.Group;

namespace Chat.UnitTests.ApplicationTests.Services;

public sealed class GroupServiceTest
{
    private const int CreatorId = 1;
    private const int GroupId = 50;
    private const string GroupName = "MyGroup";
    private const string Instructions = "You are a helpful assistant";
    private readonly IGroupService _sut;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IOpenAiService> _openAiServiceMock = new();
    private readonly Mock<IRepository<Group, int>> _groupsRepositoryMock = new();
    private readonly Mock<IRepository<GroupMember, int>> _groupMembersRepositoryMock = new();
    private readonly Mock<IRepository<AssistantFile, int>> _filesRepositoryMock = new();

    public GroupServiceTest()
    {
        _unitOfWorkMock.Setup(x => x.GetRepository<Group, int>()).Returns(_groupsRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.GetRepository<GroupMember, int>()).Returns(_groupMembersRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.GetRepository<AssistantFile, int>()).Returns(_filesRepositoryMock.Object);
        _sut = new GroupService(_userServiceMock.Object, _unitOfWorkMock.Object, _openAiServiceMock.Object);
    }

    [Fact]
    public async Task GetAllGroupsInfoAsync_ReturnsAllGroupsInfoForCreator()
    {
        // Arrange.
        var creator = new User { Id = CreatorId, UserName = "User", Email = "email@mail.com" };
        var mockGroup = GetTestGroup().BuildMock()!;
        _userServiceMock.Setup(x => x.GetUserByIdAsync(CreatorId)).ReturnsAsync(creator);
        _groupsRepositoryMock.Setup(x => x.AsQueryable()).Returns(mockGroup);
        var expectedGroup = new GroupInfoDto
        {
            Id = GroupId, Name = GroupName, Instructions = Instructions, FilesCount = 1, MembersCount = 1
        };
        
        // Act.
        var result = await _sut.GetAllGroupsInfoAsync(CreatorId);
        var resultGroup = result.FirstOrDefault();

        // Assert.
        result.Should()!.HaveCount(1);
        resultGroup.Should()!.NotBeNull().And!.BeEquivalentTo(expectedGroup);
    }

    private List<Group> GetTestGroup()
    {
        return new List<Group>
        {
            new()
            {
                Id = GroupId, CreatorId = CreatorId, Name = GroupName, Instructions = Instructions,
                Members = new List<User> { new() { Id = 100, UserName = "User1", Email = "email1@mail.com" } },
                GroupMembers = new List<GroupMember> { new() { Id = 1, UserId = 100, GroupId = 1 } },
                Files = new List<AssistantFile> { new() { GroupId = 1, Name = "file1" } }
            }
        };
    }
}