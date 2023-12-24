namespace Chat.Application.Services.Users;

public sealed class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User, int> _userRepository;
    private readonly UserManager<User> _userManager;

    public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _userRepository = _unitOfWork.GetRepository<User, int>();
    }

    public async Task<IList<UserDto>> GetAllUsersAsync()
    {
        return (await _userRepository.GetAllAsync()).Select(x => x.MapToDto()).ToList();
    }

    public async Task<UserDto> GetUserDtoByIdAsync(int id)
    {
        return (await GetUserByIdAsync(id)).MapToDto();
    }

    public async Task UpdateUserAsync(UserDto userData, int id)
    {
        var user = await GetUserByIdAsync(id);
        user.MapFrom(userData);
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<UsersPageDto> SearchUsersPagedAsync(PagedSearchDto searchData)
    {
        var foundUsers = _userRepository.SearchWhere<UserDto>(searchData.SearchFilter);
        var usersCount = foundUsers.Count();
        var pageSize = PageInfo.DefaultPageSize;
        var foundUsersPage = foundUsers
                             .OrderBy(searchData.SortingProperty, searchData.SortingOrder)
                             .Skip((searchData.Page - 1) * pageSize)
                             .Take(pageSize)
                             .Select(x => x.MapToDto());
        var pageInfo = new PageInfo(usersCount, searchData.Page);
        
        return await Task.FromResult(new UsersPageDto
        {
            PageInfo = pageInfo,
            Users = foundUsersPage.ToArray()
        });
    }

    public async Task<User> GetUserByIdAsync(int? id)
    {
        if (id is null)
        {
            throw new EntityNotFoundException(nameof(User));
        }
        
        return await _userRepository.GetByIdAsync((int)id) ?? throw new EntityNotFoundException(nameof(User));
    }
    
    public async Task<User> GetUserByNameAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName) 
               ?? throw new EntityNotFoundException(nameof(User), nameof(User.UserName));
    }
}