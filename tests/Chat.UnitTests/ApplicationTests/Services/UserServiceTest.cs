using Chat.Application.Mappings;

namespace Chat.UnitTests.ApplicationTests.Services;

public sealed class UserServiceTest
{
    private const int Id = 1;
    private const string UserName = "username";
    private const string Email = "email@gmail.com";
    private readonly IUserService _sut;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IRepository<User, int>> _userRepositoryMock = new();
    private readonly Mock<UserManager<User>> _userManagerMock;

    public UserServiceTest()
    {
        _userManagerMock = MockHelper.MockUserManager();
        _unitOfWorkMock.Setup(x => x.GetRepository<User, int>()).Returns(_userRepositoryMock.Object);
        _sut = new UserService(_unitOfWorkMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public void GetUserByIdAsync_ThrowsEntityNotFoundException_WhenEntityNotFound()
    {
        // Arrange.
        _userRepositoryMock.Setup(x => x.GetByIdAsync(Id)).ReturnsAsync((User)null!);
        
        // Act.
        var tryGetUserById = async () => await _sut.GetUserByIdAsync(Id);
        
        // Assert.
        tryGetUserById.Should()!.ThrowAsync<EntityNotFoundException>();
    }
    
    [Fact]
    public void GetUserByNameAsync_ThrowsEntityNotFoundException_WhenEntityNotFound()
    {
        // Arrange.
        var id = Id.ToString();
        _userManagerMock.Setup(x => x.FindByNameAsync(id)).ReturnsAsync((User)null!);
        
        // Act.
        var tryGetUserByName = async () => await _sut.GetUserByNameAsync(id);
        
        // Assert.
        tryGetUserByName.Should()!.ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsAllUsers()
    {
        // Arrange.
        const int usersCount = 10;
        var users = TestDataGenerator.GenerateUsers(usersCount);
        var firstUser = users.First();
        _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(users);
        
        // Act.
        var result = await _sut.GetAllUsersAsync();
        var firstUserResult = result.First();

        // Assert.
        result.Should()!.HaveCount(usersCount);
        firstUserResult.FirstName.Should()!.Be(firstUser.FirstName!);
        firstUserResult.LastName.Should()!.Be(firstUser.LastName!);
        firstUserResult.UserName.Should()!.Be(firstUser.UserName!);
        firstUserResult.Email.Should()!.Be(firstUser.Email!);
        firstUserResult.PhoneNumber.Should()!.Be(firstUser.PhoneNumber!);
    }

    [Fact]
    public async Task UpdateUserAsync_UpdatesUser_WhenValidDto()
    {
        // Arrange.
        var user = TestDataGenerator.GenerateUsers(1).First();
        var userDto = user.MapToDto();
        var expectedCallSequence = new List<string> { nameof(IRepository<User, int>.Update), nameof(IUnitOfWork.SaveChangesAsync) };
        var actualCallSequence = new List<string>();
        _userRepositoryMock.Setup(x => x.GetByIdAsync(Id)).ReturnsAsync(user);
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(default))
                       .Callback(() => actualCallSequence.Add(nameof(IUnitOfWork.SaveChangesAsync)));
        _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>()))
                           .Callback(() => actualCallSequence.Add(nameof(IRepository<User, int>.Update)));

        
        // Act.
        await _sut.UpdateUserAsync(userDto, Id);

        // Assert.
        actualCallSequence.Should()!.BeEquivalentTo(expectedCallSequence, o => o.WithStrictOrdering());
        _userRepositoryMock.Verify(x => x.Update(user), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    private List<User> GetUsers() => new()
    {
        new()
        {
            Id = 1, Email = "email1@gmail.com", UserName = "username1",
            FirstName = "firstname1", LastName = "lastname1", RefreshToken = "token1"
        },
        new()
        {
            Id = 2, Email = "email2@gmail.com", UserName = "username2",
            FirstName = "firstname2", LastName = "lastname2", RefreshToken = "token2"
        },
        new()
        {
            Id = 3, Email = "email3@gmail.com", UserName = "username3",
            FirstName = "firstname3", LastName = "lastname3", RefreshToken = "token3"
        },
        new()
        {
            Id = 4, Email = "email4@gmail.com", UserName = "username4",
            FirstName = "firstname4", LastName = "lastname4", RefreshToken = "token4"
        },
        new()
        {
            Id = 5, Email = "email5@gmail.com", UserName = "username5",
            FirstName = "firstname5", LastName = "lastname5", RefreshToken = "token5"
        },
        new()
        {
            Id = 6, Email = "email6@gmail.com", UserName = "username6",
            FirstName = "firstname6", LastName = "lastname6", RefreshToken = "token6"
        },
        new()
        {
            Id = 7, Email = "email7@gmail.com", UserName = "username7",
            FirstName = "firstname7", LastName = "lastname7", RefreshToken = "token7"
        },
        new()
        {
            Id = 8, Email = "email8@gmail.com", UserName = "username8",
            FirstName = "firstname8", LastName = "lastname8", RefreshToken = "token8"
        },
    };

    private List<UserDto> GetUserDtos() => new()
    {
        new()
        {
            Email = "email2@gmail.com", UserName = "username2", FirstName = "firstname2", LastName = "lastname2"
        },
        new()
        {
            Email = "email3@gmail.com", UserName = "username3", FirstName = "firstname3", LastName = "lastname3"
        },
        new()
        {
            Email = "email4@gmail.com", UserName = "username4", FirstName = "firstname4", LastName = "lastname4"
        },
        new()
        {
            Email = "email5@gmail.com", UserName = "username5", FirstName = "firstname5", LastName = "lastname5"
        },
        new()
        {
            Email = "email6@gmail.com", UserName = "username6", FirstName = "firstname6", LastName = "lastname6"
        },
        new()
        {
            Email = "email7@gmail.com", UserName = "username7", FirstName = "firstname7", LastName = "lastname7"
        },
        new()
        {
            Email = "email8@gmail.com", UserName = "username8", FirstName = "firstname8", LastName = "lastname8"
        },
    };
}