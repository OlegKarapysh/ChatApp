namespace Chat.Application.Services.Users;

public interface IUserService
{
    Task<IList<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserDtoByIdAsync(int id);
    Task UpdateUserAsync(UserDto userData, int id);
    Task<UsersPageDto> SearchUsersPagedAsync(PagedSearchDto searchData);
    Task<User> GetUserByIdAsync(int? id);
    Task<User> GetUserByNameAsync(string userName);
}